using GJJApiGateway.Management.Common.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GJJApiGateway.Management.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController: ControllerBase
{
     // ====== 1) SM4-CBC 快速自测：GET /health/crypto/sm4-cbc ======
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

    // ====== 2) SM4-GCM 快速自测：GET /health/crypto/sm4-gcm ======
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

    // ====== 3) SM4-CBC 自定义测试：POST /health/crypto/sm4-cbc ======
    // body: { "plaintext": "...", "keyHex": "...", "ivHex": "..." }
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

    // ====== 4) SM4-GCM 自定义测试：POST /health/crypto/sm4-gcm ======
    // body: { "plaintext": "...", "keyHex": "...", "ivHex": "...", "aad": "..." }
    public class Sm4GcmReq
    {
        public string? Plaintext { get; set; }
        public string? KeyHex { get; set; }   // 16字节Hex（可空=随机）
        public string? IvHex { get; set; }    // 推荐12字节Hex（可空=随机）
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
    
    // ====== 5) 新增：SM4-CBC 仅解密（入参是密文 + key/iv）======
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

    // ====== 6) 新增：SM4-GCM 仅解密（入参是 cipher||tag + key/iv/+aad）======
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
    
    // 2.1 （可选）生成一套密钥并加密一段明文，方便你马上测试解密
    // GET /health/crypto/sm2/demo
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

    // 2.2 解密（C1C3C2）
    // POST /health/crypto/sm2/decrypt/c1c3c2
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

    // 2.3 解密（C1C2C3）
    // POST /health/crypto/sm2/decrypt/c1c2c3
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