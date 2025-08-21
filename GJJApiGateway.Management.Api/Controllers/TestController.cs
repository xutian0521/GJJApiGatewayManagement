using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using GJJApiGateway.Management.Common.Utilities;

namespace GJJApiGateway.Management.Api.Controllers;

/// <summary>
/// 自测控制器：仅用于联调测试接口
/// 路由前缀：/test
/// </summary>
[ApiController]
[Route("[controller]")]
public class TestController: ControllerBase
{

    // 统一错误返回（供部分端点调用；不影响你已有的 Problem(...) 使用）
    private IActionResult Fail(string title, Exception ex, int status = StatusCodes.Status500InternalServerError)
    {
        var pd = new ProblemDetails
        {
            Title    = title,
            Detail   = ex.Message,
            Status   = status,
            Type     = "about:blank",
            Instance = HttpContext?.Request?.Path.Value
        };
        return StatusCode(status, pd);
    }

    // -----------------------------------------------------------------
    // 1) SM4-CBC：加解密自测（GET 快速 / POST 自定义）
    // -----------------------------------------------------------------

    /// <summary>
    /// 1.a) GET /test/crypto/sm4-cbc
    /// 说明：自动生成 key/iv，使用固定示例明文；返回加密结果并内部解密回显
    /// </summary>
    [HttpGet("crypto/sm4-cbc")]
    public IActionResult Sm4CbcQuick()
    {
        try
        {
            var demo = new GM_Demo();
            var result = demo.Sm4CbcDemo(); // 自动生成 key/iv，固定示例明文
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(title: "SM4-CBC demo failed", detail: ex.Message);
        }
    }

    /// <summary>
    /// 1.b) POST /test/crypto/sm4-cbc
    /// body: { "plaintext": "...", "keyHex": "...(16B)", "ivHex": "...(16B)" }
    /// 说明：自带或随机 key/iv，返回密文与解密回显
    /// </summary>
    public class Sm4CbcReq
    {
        public string? Plaintext { get; set; }
        public string? KeyHex { get; set; }   // 16字节Hex（可空=随机）
        public string? IvHex { get; set; }    // 16字节Hex（可空=随机）
    }

    [HttpPost("crypto/sm4-cbc")]
    public IActionResult Sm4CbcCustom([FromBody] Sm4CbcReq req)
    {
        try
        {
            var demo = new GM_Demo();
            var result = demo.Sm4CbcDemo(req.Plaintext, req.KeyHex, req.IvHex);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(title: "SM4-CBC custom demo failed", detail: ex.Message);
        }
    }

    /// <summary>
    /// 1.c) POST /test/crypto/sm4-cbc/decrypt
    /// body: { "ciphertextBase64":"...", "keyHex":"...(16B)", "ivHex":"...(16B)" }
    /// 说明：仅解密；传入密文+key/iv，返回明文
    /// </summary>
    public class Sm4CbcDecReq
    {
        public string CiphertextBase64 { get; set; } = ""; // 必填
        public string KeyHex { get; set; } = "";           // 必填：16字节 Hex
        public string IvHex  { get; set; } = "";           // 必填：16字节 Hex
    }

    [HttpPost("crypto/sm4-cbc/decrypt")]
    public IActionResult Sm4CbcDecrypt([FromBody] Sm4CbcDecReq req)
    {
        try { return Ok(new GM_Demo().Sm4CbcDecryptDemo(req.CiphertextBase64, req.KeyHex, req.IvHex)); }
        catch (Exception ex) { return Fail("SM4-CBC decrypt failed", ex); }
    }

    // -----------------------------------------------------------------
    // 2) SM4-GCM：加解密自测（GET 快速 / POST 自定义 / POST 解密）
    // -----------------------------------------------------------------

    /// <summary>
    /// 2.a) GET /test/crypto/sm4-gcm
    /// 说明：自动生成 key/iv（12B）与默认 aad；返回加密结果并内部解密回显
    /// </summary>
    [HttpGet("crypto/sm4-gcm")]
    public IActionResult Sm4GcmQuick()
    {
        try
        {
            var demo = new GM_Demo();
            var result = demo.Sm4GcmDemo(); // 自动生成 key/iv，默认 AAD
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(title: "SM4-GCM demo failed", detail: ex.Message);
        }
    }

    /// <summary>
    /// 2.b) POST /test/crypto/sm4-gcm
    /// body: { "plaintext":"...", "keyHex":"...(16B)", "ivHex":"...(12B推荐)", "aad":"..." }
    /// 说明：自带或随机 key/iv/aad，返回密文（cipher||tag）与解密回显
    /// </summary>
    public class Sm4GcmReq
    {
        public string? Plaintext { get; set; }
        public string? KeyHex { get; set; }   // 16字节 Hex（可空=随机）
        public string? IvHex { get; set; }    // 推荐12字节 Hex（可空=随机）
        public string? Aad { get; set; }      // 附加认证数据（可空）
    }

    [HttpPost("crypto/sm4-gcm")]
    public IActionResult Sm4GcmCustom([FromBody] Sm4GcmReq req)
    {
        try
        {
            var demo = new GM_Demo();
            var result = demo.Sm4GcmDemo(req.Plaintext, req.KeyHex, req.IvHex, req.Aad);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem(title: "SM4-GCM custom demo failed", detail: ex.Message);
        }
    }

    /// <summary>
    /// 2.c) POST /test/crypto/sm4-gcm/decrypt
    /// body: { "cipherAndTagBase64":"...", "keyHex":"...(16B)", "ivHex":"...(12B推荐)", "aad":"..." }
    /// 说明：仅解密；传入 cipher||tag + key/iv + aad，返回明文
    /// </summary>
    public class Sm4GcmDecReq
    {
        public string CipherAndTagBase64 { get; set; } = ""; // 必填：cipher||tag 的 Base64
        public string KeyHex { get; set; } = "";             // 必填：16字节 Hex
        public string IvHex  { get; set; } = "";             // 必填：推荐12字节 Hex
        public string? Aad   { get; set; } = "api-auth-v1";  // 可选：与加密时一致
    }

    [HttpPost("crypto/sm4-gcm/decrypt")]
    public IActionResult Sm4GcmDecrypt([FromBody] Sm4GcmDecReq req)
    {
        try { return Ok(new GM_Demo().Sm4GcmDecryptDemo(req.CipherAndTagBase64, req.KeyHex, req.IvHex, req.Aad)); }
        catch (Exception ex) { return Fail("SM4-GCM decrypt failed", ex); }
    }

    // -----------------------------------------------------------------
    // 3) SM2：演示（生成密钥与两种密文）与两种格式的解密
    // -----------------------------------------------------------------

    /// <summary>
    /// 3.a) GET /test/crypto/sm2/demo
    /// 说明：生成一套随机密钥与两种密文（C1C3C2 / C1C2C3），便于马上调用解密接口
    /// </summary>
    [HttpGet("crypto/sm2/demo")]
    public IActionResult Sm2Demo()
    {
        try
        {
            var pack = new GM_Demo().Sm2EncryptDemo("hello-sm2");
            return Ok(pack);
        }
        catch (Exception ex)
        {
            return Fail("SM2 demo pack failed", ex);
        }
    }

    /// <summary>
    /// 3.b) POST /test/crypto/sm2/decrypt/c1c3c2
    /// body: { "cipherBase64":"...", "privateKeyPem":"-----BEGIN PRIVATE KEY-----..."}
    /// 说明：SM2 解密（C1C3C2 格式）
    /// </summary>
    public class Sm2DecReq
    {
        public string CipherBase64 { get; set; } = "";   // 必填
        public string PrivateKeyPem { get; set; } = "";  // 必填：PKCS#8 PEM
    }

    [HttpPost("crypto/sm2/decrypt/c1c3c2")]
    public IActionResult Sm2DecryptC1C3C2([FromBody] Sm2DecReq req)
    {
        try
        {
            var result = new GM_Demo().Sm2DecryptC1C3C2(req.CipherBase64, req.PrivateKeyPem);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Fail("SM2 decrypt (C1C3C2) failed", ex);
        }
    }

    /// <summary>
    /// 3.c) POST /test/crypto/sm2/decrypt/c1c2c3
    /// body: { "cipherBase64":"...", "privateKeyPem":"-----BEGIN PRIVATE KEY-----..."}
    /// 说明：SM2 解密（C1C2C3 格式）
    /// </summary>
    [HttpPost("crypto/sm2/decrypt/c1c2c3")]
    public IActionResult Sm2DecryptC1C2C3([FromBody] Sm2DecReq req)
    {
        try
        {
            var result = new GM_Demo().Sm2DecryptC1C2C3(req.CipherBase64, req.PrivateKeyPem);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Fail("SM2 decrypt (C1C2C3) failed", ex);
        }
    }
}
