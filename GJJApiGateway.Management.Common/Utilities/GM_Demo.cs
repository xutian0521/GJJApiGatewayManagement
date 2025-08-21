using System;
using System.Text;

namespace GJJApiGateway.Management.Common.Utilities;

/// <summary>
/// 国密 Demo：仅用于联调/自测的包装类（不包含业务逻辑）
/// 说明：本类的方法与返回模型保持你之前的实现，未改动逻辑，仅调整顺序与注释。
/// </summary>
public class GM_Demo
{
    // ---------------------------------------------------------------------
    // A) 通用返回模型（SM4 加密/解密・SM2 解密）
    // ---------------------------------------------------------------------

    // A.1 SM4-CBC 加解密返回
    public class Sm4CbcResult
    {
        public string Algo { get; set; } = "SM4-CBC-PKCS7";
        public string KeyHex { get; set; } = "";
        public string IvHex { get; set; } = "";
        public string CiphertextBase64 { get; set; } = "";
        public string PlaintextBack { get; set; } = "";
    }

    public class Sm4CbcDecResult
    {
        public string Algo { get; set; } = "SM4-CBC-PKCS7";
        public string KeyHex { get; set; } = "";
        public string IvHex { get; set; } = "";
        public string CiphertextBase64 { get; set; } = "";
        public string Plaintext { get; set; } = "";
    }

    // A.2 SM4-GCM 加解密返回
    public class Sm4GcmResult
    {
        public string Algo { get; set; } = "SM4-GCM-128";
        public string KeyHex { get; set; } = "";
        public string IvHex { get; set; } = "";
        public string AadBase64 { get; set; } = "";
        public string CipherAndTagBase64 { get; set; } = "";
        public string PlaintextBack { get; set; } = "";
    }

    public class Sm4GcmDecResult
    {
        public string Algo { get; set; } = "SM4-GCM-128";
        public string KeyHex { get; set; } = "";
        public string IvHex { get; set; } = "";
        public string AadBase64 { get; set; } = "";
        public string CipherAndTagBase64 { get; set; } = "";
        public string Plaintext { get; set; } = "";
    }

    // A.3 SM2 解密返回
    public class Sm2DecResult
    {
        public string Algo { get; set; } = "SM2";
        public string Mode { get; set; } = "C1C3C2"; // 或 C1C2C3
        public string CipherBase64 { get; set; } = "";
        public string Plaintext { get; set; } = "";
        public string? Note { get; set; } // 可选说明
    }

    // A.4 （可选）SM2 演示包：生成随机密钥并输出两种密文，便于马上解密测试
    public class Sm2DemoPack
    {
        public string Algo { get; set; } = "SM2";
        public string Plaintext { get; set; } = "hello-sm2";
        public string PrivateKeyPem { get; set; } = "";
        public string PublicKeyPem { get; set; } = "";
        public string CipherC1C3C2Base64 { get; set; } = "";
        public string CipherC1C2C3Base64 { get; set; } = "";
    }

    // ---------------------------------------------------------------------
    // B) SM4：CBC 加解密（1）与 GCM 加解密（2）
    // ---------------------------------------------------------------------

    /// <summary>
    /// 1）SM4-CBC：加密敏感字段（例如：应用密钥）
    /// 支持自定义 key/iv（Hex）和明文；为空则自动生成/使用默认
    /// </summary>
    public Sm4CbcResult Sm4CbcDemo(string? plaintext = null, string? keyHex = null, string? ivHex = null)
    {
        var key = string.IsNullOrWhiteSpace(keyHex)
            ? GuomiCryptoHelper.RandomBytes(16)
            : GuomiCryptoHelper.FromHex(keyHex);

        var iv = string.IsNullOrWhiteSpace(ivHex)
            ? GuomiCryptoHelper.RandomBytes(16)
            : GuomiCryptoHelper.FromHex(ivHex);

        if (key.Length != 16) throw new ArgumentException("SM4 key must be 16 bytes.");
        if (iv.Length != 16)  throw new ArgumentException("SM4 CBC IV must be 16 bytes.");

        var plain = Encoding.UTF8.GetBytes(plaintext ?? "top-secret-value-123");

        var cipher = GuomiCryptoHelper.SM4EncryptCbc(plain, key, iv, pkcs7: true);
        var back   = GuomiCryptoHelper.SM4DecryptCbc(cipher, key, iv, pkcs7: true);

        return new Sm4CbcResult
        {
            KeyHex = GuomiCryptoHelper.ToHex(key),
            IvHex  = GuomiCryptoHelper.ToHex(iv),
            CiphertextBase64 = GuomiCryptoHelper.ToBase64(cipher),
            PlaintextBack = Encoding.UTF8.GetString(back)
        };
    }

    /// <summary>
    /// 1.b）SM4-CBC：仅解密（传入密文 + key/iv）
    /// </summary>
    public Sm4CbcDecResult Sm4CbcDecryptDemo(string ciphertextBase64, string keyHex, string ivHex)
    {
        var key = GuomiCryptoHelper.FromHex(keyHex);
        var iv  = GuomiCryptoHelper.FromHex(ivHex);
        if (key.Length != 16) throw new ArgumentException("SM4 key must be 16 bytes.");
        if (iv.Length != 16)  throw new ArgumentException("SM4 CBC IV must be 16 bytes.");

        var cipher = GuomiCryptoHelper.FromBase64(ciphertextBase64);
        var plain  = GuomiCryptoHelper.SM4DecryptCbc(cipher, key, iv, pkcs7: true);

        return new Sm4CbcDecResult {
            KeyHex = keyHex.ToLowerInvariant(),
            IvHex  = ivHex.ToLowerInvariant(),
            CiphertextBase64 = ciphertextBase64,
            Plaintext = Encoding.UTF8.GetString(plain)
        };
    }

    /// <summary>
    /// 2）SM4-GCM：带完整性校验（推荐用于 Token 载荷等）
    /// 支持自定义 key/iv（Hex）、AAD 与明文；为空则自动生成/默认
    /// </summary>
    public Sm4GcmResult Sm4GcmDemo(string? plaintext = null, string? keyHex = null, string? ivHex = null, string? aad = "api-auth-v1")
    {
        var key = string.IsNullOrWhiteSpace(keyHex)
            ? GuomiCryptoHelper.RandomBytes(16)
            : GuomiCryptoHelper.FromHex(keyHex);

        var iv = string.IsNullOrWhiteSpace(ivHex)
            ? GuomiCryptoHelper.RandomBytes(12) // 推荐 12 字节
            : GuomiCryptoHelper.FromHex(ivHex);

        if (key.Length != 16) throw new ArgumentException("SM4 key must be 16 bytes.");
        if (iv.Length < 8)    throw new ArgumentException("SM4-GCM IV should be 12 bytes (>=8 allowed).");

        var aadBytes = string.IsNullOrEmpty(aad) ? Array.Empty<byte>() : Encoding.UTF8.GetBytes(aad);
        var plain    = Encoding.UTF8.GetBytes(plaintext ?? "{\"appId\":1001,\"ts\":1690000000}");

        var cipherAndTag = GuomiCryptoHelper.SM4EncryptGcm(plain, key, iv, aadBytes, macBits: 128);
        var back         = GuomiCryptoHelper.SM4DecryptGcm(cipherAndTag, key, iv, aadBytes, macBits: 128);

        return new Sm4GcmResult
        {
            KeyHex = GuomiCryptoHelper.ToHex(key),
            IvHex  = GuomiCryptoHelper.ToHex(iv),
            AadBase64 = GuomiCryptoHelper.ToBase64(aadBytes),
            CipherAndTagBase64 = GuomiCryptoHelper.ToBase64(cipherAndTag),
            PlaintextBack = Encoding.UTF8.GetString(back)
        };
    }

    /// <summary>
    /// 2.b）SM4-GCM：仅解密（传入 cipher||tag + key/iv + aad）
    /// </summary>
    public Sm4GcmDecResult Sm4GcmDecryptDemo(string cipherAndTagBase64, string keyHex, string ivHex, string? aad = "api-auth-v1")
    {
        var key = GuomiCryptoHelper.FromHex(keyHex);
        var iv  = GuomiCryptoHelper.FromHex(ivHex);
        if (key.Length != 16) throw new ArgumentException("SM4 key must be 16 bytes.");
        if (iv.Length < 8)    throw new ArgumentException("SM4-GCM IV should be 12 bytes (>=8 allowed).");

        var aadBytes = string.IsNullOrEmpty(aad) ? Array.Empty<byte>() : Encoding.UTF8.GetBytes(aad);
        var cipherAndTag = GuomiCryptoHelper.FromBase64(cipherAndTagBase64);
        var plain  = GuomiCryptoHelper.SM4DecryptGcm(cipherAndTag, key, iv, aadBytes, macBits: 128);

        return new Sm4GcmDecResult {
            KeyHex = keyHex.ToLowerInvariant(),
            IvHex  = ivHex.ToLowerInvariant(),
            AadBase64 = GuomiCryptoHelper.ToBase64(aadBytes),
            CipherAndTagBase64 = cipherAndTagBase64,
            Plaintext = Encoding.UTF8.GetString(plain)
        };
    }

    // ---------------------------------------------------------------------
    // C) SM2：演示 + 两种格式解密（3）
    // ---------------------------------------------------------------------

    /// <summary>
    /// 3）SM2：加解密（C1C3C2）＋签名验签（演示）
    /// </summary>
    public void Sm2AllInOne()
    {
        var (priPem, pubPem) = GuomiCryptoHelper.SM2GenerateKeyPairPem();

        var data   = Encoding.UTF8.GetBytes("hello-kylin-sm2");
        var cipher = GuomiCryptoHelper.SM2Encrypt(data, pubPem); // 默认 C1C3C2
        var plain  = GuomiCryptoHelper.SM2Decrypt(cipher, priPem);
        var text   = Encoding.UTF8.GetString(plain);

        var sign = GuomiCryptoHelper.SM2Sign(data, priPem, userId: "1234567812345678");
        var ok   = GuomiCryptoHelper.SM2Verify(data, sign, pubPem, userId: "1234567812345678");
    }

    /// <summary>
    /// 3.a）SM2 演示：生成一套密钥并输出两种格式密文（C1C3C2 / C1C2C3）
    /// </summary>
    public Sm2DemoPack Sm2EncryptDemo(string plaintext = "hello-sm2")
    {
        var (priPem, pubPem) = GuomiCryptoHelper.SM2GenerateKeyPairPem();
        var data = Encoding.UTF8.GetBytes(plaintext);

        var c1c3c2 = GuomiCryptoHelper.SM2Encrypt(data, pubPem, c1c3c2: true);
        var c1c2c3 = GuomiCryptoHelper.SM2Encrypt(data, pubPem, c1c3c2: false);

        return new Sm2DemoPack
        {
            Plaintext = plaintext,
            PrivateKeyPem = priPem,
            PublicKeyPem  = pubPem,
            CipherC1C3C2Base64 = GuomiCryptoHelper.ToBase64(c1c3c2),
            CipherC1C2C3Base64 = GuomiCryptoHelper.ToBase64(c1c2c3)
        };
    }

    /// <summary>
    /// 3.b）SM2 解密：C1C3C2 格式
    /// </summary>
    public Sm2DecResult Sm2DecryptC1C3C2(string cipherBase64, string privateKeyPem)
    {
        var cipher = GuomiCryptoHelper.FromBase64(cipherBase64);
        var plain  = GuomiCryptoHelper.SM2Decrypt(cipher, privateKeyPem, c1c3c2: true);
        return new Sm2DecResult
        {
            Mode = "C1C3C2",
            CipherBase64 = cipherBase64,
            Plaintext = Encoding.UTF8.GetString(plain)
        };
    }

    /// <summary>
    /// 3.c）SM2 解密：C1C2C3 格式
    /// </summary>
    public Sm2DecResult Sm2DecryptC1C2C3(string cipherBase64, string privateKeyPem)
    {
        var cipher = GuomiCryptoHelper.FromBase64(cipherBase64);
        var plain  = GuomiCryptoHelper.SM2Decrypt(cipher, privateKeyPem, c1c3c2: false);
        return new Sm2DecResult
        {
            Mode = "C1C2C3",
            CipherBase64 = cipherBase64,
            Plaintext = Encoding.UTF8.GetString(plain)
        };
    }
}
