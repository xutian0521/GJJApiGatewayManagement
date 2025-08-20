### 对于项目分层的新理解 2025-05-30

1. 我有我这个项目的分层又有了新的理解，你看看对不对。业务层方法 负责 组装业务步骤。第一步干什么，第二部干什么。每一个步骤或者业务单独模块，可以放到业务层模板Module文件夹下，业务层组装抽象层的 IModule 来实现业务。如果需要调用数据层的时候，Command 和query 层的抽象类 ICommand 和IQuery。在Command 和query 里的各个方法里根据业务需求区组装 各种数据单个或者多个批量操作数据库的代码。Repository仓储层提供 单一的增删改查的方法 和开启提交事物的方法等。读写分离层Command 和query  组装各种数据操作。
2. 在每个过客


## 国密Linux 测试请求命令

```bash
# CBC
curl -s http://localhost:5000/health/crypto/sm4-cbc | jq
# GCM
curl -s http://localhost:5000/health/crypto/sm4-gcm | jq


# CBC（16字节key/iv）
curl -s -X POST http://localhost:5000/health/crypto/sm4-cbc \
  -H "Content-Type: application/json" \
  -d '{"plaintext":"cbc-demo","keyHex":"00112233445566778899aabbccddeeff","ivHex":"0102030405060708090a0b0c0d0e0f10"}' | jq

# GCM（推荐12字节IV；可带AAD）
curl -s -X POST http://localhost:5000/health/crypto/sm4-gcm \
  -H "Content-Type: application/json" \
  -d '{"plaintext":"gcm-demo","keyHex":"00112233445566778899aabbccddeeff","ivHex":"a1a2a3a4a5a6a7a8a9aaabac","aad":"api-auth-v1"}' | jq
  

# CBC 解密
curl -s -X POST http://localhost:5000/health/crypto/sm4-cbc/decrypt \
  -H "Content-Type: application/json" \
  -d '{
        "ciphertextBase64":"bUmtXFsl1+DCzKhkHbg0dA==",
        "keyHex":"00112233445566778899aabbccddeeff",
        "ivHex":"0102030405060708090a0b0c0d0e0f10"
      }' | jq

# GCM 解密（注意 AAD 必须与加密时一致）
curl -s -X POST http://localhost:5000/health/crypto/sm4-gcm/decrypt \
  -H "Content-Type: application/json" \
  -d '{
        "cipherAndTagBase64":"52Tzw4loVooVr1YvlCPBkJMIEHqKogdH",
        "keyHex":"00112233445566778899aabbccddeeff",
        "ivHex":"a1a2a3a4a5a6a7a8a9aaabac",
        "aad":"api-auth-v1"
      }' | jq

# 生成一套随机密钥和两种格式的密文（默认明文 "hello-sm2"）
curl -s http://localhost:5000/health/crypto/sm2/demo | jq

# 解密 C1C3C2
curl -s -X POST http://localhost:5000/health/crypto/sm2/decrypt/c1c3c2 \
  -H "Content-Type: application/json" \
  -d '{
        "cipherBase64": "这里替换为 cipherC1C3C2Base64",
        "privateKeyPem": "这里粘贴上一步的 privateKeyPem（带 -----BEGIN PRIVATE KEY-----...）"
      }' | jq

# 解密 C1C2C3
curl -s -X POST http://localhost:5000/health/crypto/sm2/decrypt/c1c2c3 \
  -H "Content-Type: application/json" \
  -d '{
        "cipherBase64": "这里替换为 cipherC1C2C3Base64",
        "privateKeyPem": "同上"
      }' | jq


```