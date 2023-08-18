using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security;


[assembly: OwinStartup(typeof(WebApiApp.Startup))]

namespace WebApiApp
{
    /// <summary>
    /// iss：發行人
    /// exp：到期时间
    /// sub：主题
    /// aud：用户
    /// nbf：在此之前不可用
    /// iat：發佈時間
    /// jti：JWT ID 用於標識該 JWT
    /// </summary>
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //  設定JwtBearerAuthenticationOptions
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    //  設定驗證參數
                    AuthenticationMode = AuthenticationMode.Active,
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true, // 是否驗證Issuer
                        ValidateAudience = true,// 是否驗證Audience
                        ValidateIssuerSigningKey = true,    // 是否驗證SecurityKey
                        ValidIssuer = ConfigurationManager.AppSettings["JwtIssuer"], //some string, normally web url,   // 發行者
                        ValidAudience = ConfigurationManager.AppSettings["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["JwtKey"])), // 憑證
                        ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha512 },    //  驗證算法，必須限制一種，否則會出現資安問題
                    }
                });
        }
    }
}
