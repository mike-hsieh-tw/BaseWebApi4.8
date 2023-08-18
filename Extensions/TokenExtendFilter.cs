using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http;
using System.Net.Http.Formatting;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.JsonWebTokens;

namespace WebApiApp.Extensions
{
    /// <summary>
    /// Token延長過濾器
    /// </summary>
    public class TokenExtendFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                object claims = TokenHelper.GetTokenClaims(HttpContext.Current.User.Identity);

                if (((IDictionary<string, object>)claims).ContainsKey("exp"))
                {
                    //  過期時間戳
                    long expireTimeSpan = Convert.ToInt64((claims as dynamic).exp);

                    //  過期時間
                    DateTime expireDT = TimeSpanHelper.ToDateTime(expireTimeSpan);

                    //  剩餘時間
                    var remainSecond = TimeSpanHelper.GetTimeSpanDuration(DateTime.Now, expireDT);

                    //  若剩餘時間小於3分鐘，則延長Token
                    if (remainSecond <= 180)
                    {
                        #region 延長Token
                        var data = (ApiResponseModel)((System.Net.Http.ObjectContent)actionExecutedContext.Response.Content).Value;

                        //  取得延長的Token
                        data.Token = TokenHelper.GenarateToken(new UserInfo
                        {
                            UserId = (claims as dynamic).user_id,
                            Role = (claims as dynamic).user_role,
                            Company = (claims as dynamic).user_company,
                            Email = (claims as dynamic).user_email,
                        }, actionExecutedContext.Request.GetClientIpString());
                        #endregion

                        //  回傳資料
                        actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, data, new JsonMediaTypeFormatter());
                    }
                }
            }
            // Handling Authorize: Basic <base64(username:password)> format.
            catch (Exception e)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}