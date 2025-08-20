using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.GM;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;

namespace GJJApiGateway.Management.Common.Utilities;

/// <summary>
/// 国密算法帮助类（SM2/SM3/SM4）
/// 依赖：Org.BouncyCastle
/// 跨平台、纯托管；适用于 .NET 8 + Linux（含银河麒麟）
/// </summary>
public static class GuomiCryptoHelper
{
    // ---------------------------
    // 基础：Hex / Base64 工具
    // ---------------------------
    public static string ToHex(byte[] data) =>
        BitConverter.ToString(data).Replace("-", "").ToLowerInvariant();

    public static byte[] FromHex(string hex)
    {
        if (string.IsNullOrWhiteSpace(hex)) return Array.Empty<byte>();
        if (hex.Length % 2 != 0) throw new ArgumentException("Hex length must be even.");
        var bytes = new byte[hex.Length / 2];
        for (int i = 0; i < bytes.Length; i++)
            bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        return bytes;
    }

    public static string ToBase64(byte[] data) => Convert.ToBase64String(data);
    public static byte[] FromBase64(string b64) => Convert.FromBase64String(b64);

    public static byte[] RandomBytes(int length)
    {
        var b = new byte[length];
        RandomNumberGenerator.Fill(b);
        return b;
    }

    // ---------------------------
    // SM3 摘要
    // ---------------------------
    public static byte[] SM3Hash(byte[] data)
    {
        var d = new SM3Digest();
        d.BlockUpdate(data, 0, data.Length);
        var output = new byte[d.GetDigestSize()];
        d.DoFinal(output, 0);
        return output;
    }

    public static string SM3HashHex(string text, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return ToHex(SM3Hash(encoding.GetBytes(text)));
    }

    // ---------------------------
    // SM4 对称加密
    // ---------------------------

    /// <summary>
    /// SM4-CBC 加密（默认 PKCS7 填充）
    /// key: 16字节；iv: 16字节
    /// 返回密文字节数组
    /// </summary>
    public static byte[] SM4EncryptCbc(byte[] plain, byte[] key16, byte[] iv16, bool pkcs7 = true)
    {
        if (key16 == null || key16.Length != 16) throw new ArgumentException("SM4 key must be 16 bytes.");
        if (iv16 == null || iv16.Length != 16) throw new ArgumentException("SM4 IV must be 16 bytes.");

        var cipher = pkcs7
            ? (IBufferedCipher)new PaddedBufferedBlockCipher(new CbcBlockCipher(new SM4Engine()), new Pkcs7Padding())
            : new BufferedBlockCipher(new CbcBlockCipher(new SM4Engine()));

        cipher.Init(true, new ParametersWithIV(new KeyParameter(key16), iv16));

        var outBuf = new byte[cipher.GetOutputSize(plain.Length)];
        var len = cipher.ProcessBytes(plain, 0, plain.Length, outBuf, 0);
        len += cipher.DoFinal(outBuf, len);
        return outBuf.Take(len).ToArray();
    }

    /// <summary>
    /// SM4-CBC 解密（默认 PKCS7 填充）
    /// </summary>
    public static byte[] SM4DecryptCbc(byte[] cipherBytes, byte[] key16, byte[] iv16, bool pkcs7 = true)
    {
        if (key16 == null || key16.Length != 16) throw new ArgumentException("SM4 key must be 16 bytes.");
        if (iv16 == null || iv16.Length != 16) throw new ArgumentException("SM4 IV must be 16 bytes.");

        var cipher = pkcs7
            ? (IBufferedCipher)new PaddedBufferedBlockCipher(new CbcBlockCipher(new SM4Engine()), new Pkcs7Padding())
            : new BufferedBlockCipher(new CbcBlockCipher(new SM4Engine()));

        cipher.Init(false, new ParametersWithIV(new KeyParameter(key16), iv16));

        var outBuf = new byte[cipher.GetOutputSize(cipherBytes.Length)];
        var len = cipher.ProcessBytes(cipherBytes, 0, cipherBytes.Length, outBuf, 0);
        len += cipher.DoFinal(outBuf, len);
        return outBuf.Take(len).ToArray();
    }

    /// <summary>
    /// SM4-GCM 加密（推荐：iv 12字节，macBits 128）
    /// 返回：cipher || tag（合并后的字节数组）
    /// </summary>
    public static byte[] SM4EncryptGcm(byte[] plain, byte[] key16, byte[] iv12, byte[]? aad = null, int macBits = 128)
    {
        if (key16 == null || key16.Length != 16) throw new ArgumentException("SM4 key must be 16 bytes.");
        if (iv12 == null || iv12.Length < 8) throw new ArgumentException("SM4-GCM IV should be 12 bytes (>=8 allowed).");

        var gcm = new GcmBlockCipher(new SM4Engine());
        gcm.Init(true, new AeadParameters(new KeyParameter(key16), macBits, iv12, aad));

        var outBuf = new byte[gcm.GetOutputSize(plain.Length)];
        var len = gcm.ProcessBytes(plain, 0, plain.Length, outBuf, 0);
        len += gcm.DoFinal(outBuf, len);
        return outBuf.Take(len).ToArray();
    }

    /// <summary>
    /// SM4-GCM 解密；输入必须是 cipher || tag 的合并字节数组
    /// 解密失败会抛出 InvalidCipherTextException（MAC 校验失败）
    /// </summary>
    public static byte[] SM4DecryptGcm(byte[] cipherAndTag, byte[] key16, byte[] iv12, byte[]? aad = null, int macBits = 128)
    {
        if (key16 == null || key16.Length != 16) throw new ArgumentException("SM4 key must be 16 bytes.");
        if (iv12 == null || iv12.Length < 8) throw new ArgumentException("SM4-GCM IV should be 12 bytes (>=8 allowed).");

        var gcm = new GcmBlockCipher(new SM4Engine());
        gcm.Init(false, new AeadParameters(new KeyParameter(key16), macBits, iv12, aad));

        var outBuf = new byte[gcm.GetOutputSize(cipherAndTag.Length)];
        var len = gcm.ProcessBytes(cipherAndTag, 0, cipherAndTag.Length, outBuf, 0);
        len += gcm.DoFinal(outBuf, len);
        return outBuf.Take(len).ToArray();
    }

    // ---------------------------
    // SM2 公钥密码：密钥、加解密、签名
    // ---------------------------

    /// <summary>
    /// 生成 SM2 密钥对（PEM：PKCS#8 私钥，SubjectPublicKeyInfo 公钥）
    /// </summary>
    public static (string PrivateKeyPem, string PublicKeyPem) SM2GenerateKeyPairPem()
    {
        // SM2 曲线：sm2p256v1
        var ecParam = ECNamedCurveTable.GetByName("sm2p256v1");
        if (ecParam == null) throw new InvalidOperationException("Cannot get curve sm2p256v1.");

        var domain = new ECDomainParameters(ecParam.Curve, ecParam.G, ecParam.N, ecParam.H, ecParam.GetSeed());
        var gen = new ECKeyPairGenerator("EC");
        gen.Init(new ECKeyGenerationParameters(domain, new SecureRandom()));
        var kp = gen.GenerateKeyPair();

        var privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo((AsymmetricKeyParameter)kp.Private);
        var publicKeyInfo  = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo((AsymmetricKeyParameter)kp.Public);

        string priPem, pubPem;
        using (var sw = new StringWriter())
        {
            var pw = new PemWriter(sw);
            pw.WriteObject(privateKeyInfo);
            pw.Writer.Flush();
            priPem = sw.ToString();
        }
        using (var sw = new StringWriter())
        {
            var pw = new PemWriter(sw);
            pw.WriteObject(publicKeyInfo);
            pw.Writer.Flush();
            pubPem = sw.ToString();
        }
        return (priPem, pubPem);
    }

    /// <summary>
    /// SM2 加密（默认 C1C3C2 格式）
    /// </summary>
    public static byte[] SM2Encrypt(byte[] plain, string publicKeyPem, bool c1c3c2 = true)
    {
        var pub = ReadPublicKey(publicKeyPem) as ECPublicKeyParameters
                  ?? throw new ArgumentException("Invalid SM2 public key PEM.");
        var mode = c1c3c2 ? SM2Engine.Mode.C1C3C2 : SM2Engine.Mode.C1C2C3;
        var engine = new SM2Engine(mode);
        engine.Init(true, new ParametersWithRandom(pub, new SecureRandom()));
        return engine.ProcessBlock(plain, 0, plain.Length);
    }

    /// <summary>
    /// SM2 解密（默认 C1C3C2 格式）
    /// </summary>
    public static byte[] SM2Decrypt(byte[] cipher, string privateKeyPem, bool c1c3c2 = true)
    {
        var pri = ReadPrivateKey(privateKeyPem) as ECPrivateKeyParameters
                  ?? throw new ArgumentException("Invalid SM2 private key PEM.");
        var mode = c1c3c2 ? SM2Engine.Mode.C1C3C2 : SM2Engine.Mode.C1C2C3;
        var engine = new SM2Engine(mode);
        engine.Init(false, pri);
        return engine.ProcessBlock(cipher, 0, cipher.Length);
    }

    /// <summary>
    /// SM2 签名（SM3 摘要，含默认 userId="1234567812345678"）
    /// 返回 DER 编码 (r,s) 签名
    /// </summary>
    public static byte[] SM2Sign(byte[] data, string privateKeyPem, string userId = "1234567812345678")
    {
        var pri = ReadPrivateKey(privateKeyPem) as ECPrivateKeyParameters
                  ?? throw new ArgumentException("Invalid SM2 private key PEM.");
        var signer = new SM2Signer(new SM3Digest());
        var withId = new ParametersWithID(new ParametersWithRandom(pri, new SecureRandom()),
                                          Encoding.ASCII.GetBytes(userId));
        signer.Init(true, withId);
        signer.BlockUpdate(data, 0, data.Length);
        return signer.GenerateSignature();
    }

    /// <summary>
    /// SM2 验签（SM3 摘要，userId 需与签名时一致）
    /// 传入 DER 编码 (r,s)
    /// </summary>
    public static bool SM2Verify(byte[] data, byte[] signatureDer, string publicKeyPem, string userId = "1234567812345678")
    {
        var pub = ReadPublicKey(publicKeyPem) as ECPublicKeyParameters
                  ?? throw new ArgumentException("Invalid SM2 public key PEM.");
        var signer = new SM2Signer(new SM3Digest());
        var withId = new ParametersWithID(pub, Encoding.ASCII.GetBytes(userId));
        signer.Init(false, withId);
        signer.BlockUpdate(data, 0, data.Length);
        return signer.VerifySignature(signatureDer);
    }

    // ---------------------------
    // PEM 读写（内部）
    // ---------------------------
    private static AsymmetricKeyParameter ReadPrivateKey(string pem)
    {
        using var sr = new StringReader(pem);
        var obj = new PemReader(sr).ReadObject();
        return obj switch
        {
            AsymmetricCipherKeyPair kp => (AsymmetricKeyParameter)kp.Private,
            AsymmetricKeyParameter ap  => ap,
            _ => throw new ArgumentException("Unsupported private key PEM.")
        };
    }

    private static AsymmetricKeyParameter ReadPublicKey(string pem)
    {
        using var sr = new StringReader(pem);
        var obj = new PemReader(sr).ReadObject();
        return obj switch
        {
            AsymmetricKeyParameter ap => ap,
            _ => throw new ArgumentException("Unsupported public key PEM.")
        };
    }
    
}