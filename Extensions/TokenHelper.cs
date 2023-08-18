using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace WebApiApp.Extensions
{
    public static class TokenHelper
    {
        /// <summary>
        /// 取得Token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GenarateToken(UserInfo userInfo, string ip)
        {
            //  先查看是否有使用者資訊 及 IP
            if (userInfo == null || string.IsNullOrWhiteSpace(ip))
            {
                //  拋出錯誤，取得Tokeng失敗
                throw new Exception("Genarate token fail, please sign in.");
            }

            var issuer = ConfigurationManager.AppSettings["JwtIssuer"];
            var aesKey = ConfigurationManager.AppSettings["JwtAesKey"];

            #region 產生憑證
            var key = ConfigurationManager.AppSettings["JwtKey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
            #endregion

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("user_id", AesEncrypt.Encrypt(userInfo.UserId, aesKey)));
            permClaims.Add(new Claim("user_role", AesEncrypt.Encrypt(userInfo.Role, aesKey)));
            permClaims.Add(new Claim("user_company", AesEncrypt.Encrypt(userInfo.Company, aesKey)));
            permClaims.Add(new Claim("user_email", AesEncrypt.Encrypt(userInfo.Email, aesKey)));
            permClaims.Add(new Claim("user_ip", AesEncrypt.Encrypt(ip, aesKey)));

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(
                issuer, //Issure    
                issuer,  //Audience    
                permClaims, //  其他資料
                expires: DateTime.Now.AddSeconds(600), // DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );

            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt_token;
        }

        /// <summary>
        /// 取得 Token Claims
        /// 
        /// 大致資料如下:
        /// {
        ///     "jti": "666769ae-5412-4ef3-9286-24b0dec51f99",
        ///     "user_id": "mary.chang",
        ///     "user_role": "2",
        ///     "user_company": "C02",
        ///     "user_email": "mary.chang@gmail.com",
        ///     "user_ip": "::1",
        ///     "exp": "1692235825",
        ///     "iss": "http://localhost/",
        ///     "aud": "http://localhost/"
        /// }
        /// </summary>
        /// <returns></returns>
        public static object GetTokenClaims(System.Security.Principal.IIdentity iidentity, bool onlyUserInfo = false)
        {
            var aesKey = ConfigurationManager.AppSettings["JwtAesKey"];

            object claimsObj = null;

            var identity = iidentity as ClaimsIdentity;
            if (identity != null)
            {
                dynamic claimsDynamicObj = new ExpandoObject();

                var claims = identity.Claims.ToDictionary(p => p.Type, p => p.Value);

                foreach (var item in claims)
                {
                    // Value: 若是使用者相關訊息，就要解密
                    if (item.Key.StartsWith("user_"))
                    {
                        ((IDictionary<String, object>)claimsDynamicObj)[item.Key] = AesEncrypt.Decrypt(item.Value, aesKey);
                    }
                    // 其他 Token Claims 資訊
                    else if (onlyUserInfo == false)
                    {
                        ((IDictionary<String, object>)claimsDynamicObj)[item.Key] = item.Value;
                    }
                }

                claimsObj = claimsDynamicObj;
            }

            return claimsObj;
        }

        public static string GetIpInToken(System.Security.Principal.IIdentity iidentity)
        {
            var aesKey = ConfigurationManager.AppSettings["JwtAesKey"];

            var identity = iidentity as ClaimsIdentity;
            if (identity != null)
            {
                var claims = identity.Claims.ToDictionary(p => p.Type, p => p.Value);

                foreach (var item in claims)
                {
                    // Value: 若是使用者相關訊息，就要解密
                    if (item.Key.StartsWith("user_ip"))
                    {
                        return AesEncrypt.Decrypt(item.Value, aesKey);
                    }
                }
            }

            return string.Empty;
        }
    }
}