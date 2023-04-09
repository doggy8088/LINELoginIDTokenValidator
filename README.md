# LINELoginIdTokenValidator

## 初始化專案

```sh
dotnet new console -n LINELoginIDTokenValidator
cd LINELoginIDTokenValidator

dotnet new gitignore

git init
git add .
git commit -m "Initial commit"

dotnet add package System.CommandLine --prerelease

git add .
git commit -m "Add System.CommandLine package"

dotnet add package IdentityModel
dotnet add package System.IdentityModel.Tokens.Jwt
```

## 使用方式

1. 先透過 LINE Login 取得以 ES256 演算法簽發的 ID Token 字串

    要讓 LINE Login 發出 `ES256` 簽發的 ID Token 必須要認真看完我的 [如何將一個 ASP.NET Core 網站快速加入 LINE Login 功能 (OpenID Connect)](https://blog.miniasp.com/post/2022/04/08/LINE-Login-with-OpenID-Connect-in-ASPNET-Core) 文章，確保從 Token Endpoint 取得 Access Token 與 ID Token 時，要額外送出一個 `id_token_key_type=JWK` 的參數才行！

2. 使用此工具驗證 ID Token 的有效性

    ```sh
    dotnet run -- --channel-id "YOUR_CHANNEL_ID" --token "YOUR_ID_TOKEN"
    ```

    ![image](https://user-images.githubusercontent.com/88981/230779870-6dfb7dcc-2142-4410-850b-df2c3722de60.png)

