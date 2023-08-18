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

namespace WebApiApp.Extensions
{
    /// <summary>
    /// 檢查當前IP是否與Token中的IP是否相同
    /// </summary>
    public class IpValidationFilter : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            try
            {
                string currentIp = HttpRequestMessageExtensions.GetClientIpString(actionContext.Request)?.Trim();
                string tokenIp = TokenHelper.GetIpInToken(HttpContext.Current.User.Identity);

                //  驗證IP不同 &&  Action沒有AllowAnonymousAttribute => 表示驗證失敗
                if (currentIp != tokenIp &&
                    actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>(false).Any() == false)
                {
                    // 回傳驗證失敗
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                        new ApiResponseModel
                        {
                            Result = "Error",
                            Message = "Authentication failed, please sign in again.",
                            Data = null,
                        }, new JsonMediaTypeFormatter());
                }

                base.OnAuthorization(actionContext);
            }
            // Handling Authorize: Basic <base64(username:password)> format.
            catch (Exception e)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}