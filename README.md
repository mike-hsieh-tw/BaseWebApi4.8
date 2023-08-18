# BaseWebApi4.8
框架為 Asp.net4.8，使用了 jwt, swagger 套件進行 權限驗證、介面Ui。
文章位址: https://mike.coderbridge.io/2023/08/18/ASPNet-48-WebApi-with-Swagger-Jwt/

## 主要內容:
1. 基本框架開發。
2. Jwt權限驗證。
3. Swagger介面UI。
4. Token敏感資訊加密: 由於任何人都可以對Token解密，所以對機敏資訊加密。
5. IP驗證過濾器: 一個Token對應一個IP。
6. Token延展器: 當該Token剩餘時間小於3分鐘，Server會回傳新的Token給Client。

基本展示:
1. 登入取得Token
![image](https://github.com/mike-hsieh-tw/BaseWebApi4.8/assets/60645233/81b84b54-c73a-425f-82de-4d279783eb1e)

2. 使用Token獲取資料
![image](https://github.com/mike-hsieh-tw/BaseWebApi4.8/assets/60645233/3cf37f4a-18ea-4747-b482-9ac57b486632)

3. Token敏感資訊加密
![image](https://github.com/mike-hsieh-tw/BaseWebApi4.8/assets/60645233/53a16022-6dc8-46ea-a184-0fd4ebb727c4)

4. 剩餘時間小於三分鐘，自動取得延長Token
![image](https://github.com/mike-hsieh-tw/BaseWebApi4.8/assets/60645233/2a4b4eca-9744-4e5c-90f9-5459bc5a4efc)

5. 使用錯誤Token，驗證失敗
![image](https://github.com/mike-hsieh-tw/BaseWebApi4.8/assets/60645233/85320ab5-c0b3-4a46-b930-56f9bf068815)


