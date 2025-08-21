### 对于项目分层的新理解 2025-05-30

1. 我有我这个项目的分层又有了新的理解，你看看对不对。业务层方法 负责 组装业务步骤。第一步干什么，第二部干什么。每一个步骤或者业务单独模块，可以放到业务层模板Module文件夹下，业务层组装抽象层的 IModule 来实现业务。如果需要调用数据层的时候，Command 和query 层的抽象类 ICommand 和IQuery。在Command 和query 里的各个方法里根据业务需求区组装 各种数据单个或者多个批量操作数据库的代码。Repository仓储层提供 单一的增删改查的方法 和开启提交事物的方法等。读写分离层Command 和query  组装各种数据操作。
2. 在每个过客


## 国密Linux 测试请求命令

```bash
#!/usr/bin/env bash
# 统一说明：控制器为 TestController，路由前缀 /test
# 请按需替换端口（默认 5000/8080 等）

BASE="http://localhost:5000/test"

# ------------------------------------------------------------
# 1) SM4-CBC
# ------------------------------------------------------------

# 1.a) GET /test/crypto/sm4-cbc
# 说明：自动生成 key/iv，固定明文，返回密文与解密回显
curl -s "${BASE}/crypto/sm4-cbc" | jq

# 1.b) POST /test/crypto/sm4-cbc
# body: {plaintext, keyHex(16B), ivHex(16B)}
# 说明：自定义 key/iv/明文；返回密文与解密回显
curl -s -X POST "${BASE}/crypto/sm4-cbc" \
  -H "Content-Type: application/json" \
  -d '{"plaintext":"cbc-demo","keyHex":"00112233445566778899aabbccddeeff","ivHex":"0102030405060708090a0b0c0d0e0f10"}' | jq

# 1.c) POST /test/crypto/sm4-cbc/decrypt
# body: {ciphertextBase64, keyHex(16B), ivHex(16B)}
# 说明：仅解密（示例使用你上一条消息中的固定样例）
curl -s -X POST "${BASE}/crypto/sm4-cbc/decrypt" \
  -H "Content-Type: application/json" \
  -d '{
        "ciphertextBase64":"bUmtXFsl1+DCzKhkHbg0dA==",
        "keyHex":"00112233445566778899aabbccddeeff",
        "ivHex":"0102030405060708090a0b0c0d0e0f10"
      }' | jq

# ------------------------------------------------------------
# 2) SM4-GCM
# ------------------------------------------------------------

# 2.a) GET /test/crypto/sm4-gcm
# 说明：自动生成 key/iv(12B) 与默认 aad；返回 cipher||tag 与解密回显
curl -s "${BASE}/crypto/sm4-gcm" | jq

# 2.b) POST /test/crypto/sm4-gcm
# body: {plaintext, keyHex(16B), ivHex(12B 推荐), aad}
# 说明：自定义 key/iv/aad/明文；返回 cipher||tag 与解密回显
curl -s -X POST "${BASE}/crypto/sm4-gcm" \
  -H "Content-Type: application/json" \
  -d '{"plaintext":"gcm-demo","keyHex":"00112233445566778899aabbccddeeff","ivHex":"a1a2a3a4a5a6a7a8a9aaabac","aad":"api-auth-v1"}' | jq

# 2.c) POST /test/crypto/sm4-gcm/decrypt
# body: {cipherAndTagBase64, keyHex(16B), ivHex(12B 推荐), aad}
# 说明：仅解密（示例使用你上一条消息中的固定样例）
curl -s -X POST "${BASE}/crypto/sm4-gcm/decrypt" \
  -H "Content-Type: application/json" \
  -d '{
        "cipherAndTagBase64":"52Tzw4loVooVr1YvlCPBkJMIEHqKogdH",
        "keyHex":"00112233445566778899aabbccddeeff",
        "ivHex":"a1a2a3a4a5a6a7a8a9aaabac",
        "aad":"api-auth-v1"
      }' | jq

# ------------------------------------------------------------
# 3) SM2
# ------------------------------------------------------------

# 3.a) GET /test/crypto/sm2/demo
# 说明：生成随机密钥与两种格式密文（C1C3C2 / C1C2C3），用于下一步解密
curl -s "${BASE}/crypto/sm2/demo" | jq

# 3.b) POST /test/crypto/sm2/decrypt/c1c3c2
# body: {cipherBase64, privateKeyPem}
# 说明：SM2 解密（C1C3C2）；请将上一步的 privateKeyPem 与 cipherC1C3C2Base64 替换进去
# curl 示例（请替换字段内容后再执行）：
# curl -s -X POST "${BASE}/crypto/sm2/decrypt/c1c3c2" \
#   -H "Content-Type: application/json" \
#   -d '{
#         "cipherBase64": "替换为 cipherC1C3C2Base64",
#         "privateKeyPem": "替换为 -----BEGIN PRIVATE KEY----- ... -----END PRIVATE KEY-----"
#       }' | jq

# 3.c) POST /test/crypto/sm2/decrypt/c1c2c3
# body: {cipherBase64, privateKeyPem}
# 说明：SM2 解密（C1C2C3）；同上将字段替换为 demo 返回值
# curl 示例（请替换字段内容后再执行）：
# curl -s -X POST "${BASE}/crypto/sm2/decrypt/c1c2c3" \
#   -H "Content-Type: application/json" \
#   -d '{
#         "cipherBase64": "替换为 cipherC1C2C3Base64",
#         "privateKeyPem": "替换为 -----BEGIN PRIVATE KEY----- ... -----END PRIVATE KEY-----"
#       }' | jq


```