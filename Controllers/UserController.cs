using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
using WebApiApp.Extensions;

namespace WebApiApp.Controllers
{
    [Authorize] //表示需要通過Jwt驗證
    public class UserController : ApiController
    {
        /// <summary>
        /// 取得當前使用者Request所有資訊
        /// </summary>
        /// <returns></returns>
        [Route("User/GetCurrentRequestDetailInfo")]
        [HttpGet]
        public HttpResponseMessage GetCurrentRequestDetailInfo()
        {
            //  回傳驗證資料
            return Request.CreateResponse(HttpStatusCode.OK, new ApiResponseModel
            {
                Result = "Ok",
                Message = "No error.",
                Data = Extensions.HttpRequestMessageExtensions.GetRequestDetailInfo(),
            }, new JsonMediaTypeFormatter());
        }

        /// <summary>
        /// 取得當前使用者
        /// </summary>
        /// <returns></returns>
        [Route("User/GetCurrentUser")]
        [HttpGet]
        public HttpResponseMessage GetCurrentUser()
        {
            //  回傳驗證資料
            return Request.CreateResponse(HttpStatusCode.OK, new ApiResponseModel
            {
                Result = "Ok",
                Message = "No error.",
                Data = TokenHelper.GetTokenClaims(this.User.Identity)
            }, new JsonMediaTypeFormatter());
        }

        /// <summary>
        /// 取得使用者IP
        /// </summary>
        /// <returns></returns>
        [Route("User/GetUserIp")]
        [HttpGet]
        public HttpResponseMessage GetUserIp()
        {

            //  回傳驗證資料
            return Request.CreateResponse(HttpStatusCode.OK, new ApiResponseModel
            {
                Result = "Ok",
                Message = "No error.",
                Data = this.Request.GetClientIpString()
            }, new JsonMediaTypeFormatter());
        }

        /// <summary>
        /// 取得使用者資料
        /// </summary>
        /// <returns></returns>
        [Route("User/GetAll")]
        [HttpGet]
        public HttpResponseMessage GetUserInfoList()
        {
            //  回傳驗證資料
            return Request.CreateResponse(HttpStatusCode.OK, new ApiResponseModel
            {
                Result = "Ok",
                Message = "No error.",
                Data = QueryUserInfoList(),
            }, new JsonMediaTypeFormatter());
        }

        /// <summary>
        /// 登出Api
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("User/Logout")]
        [HttpPost]
        public HttpResponseMessage Logout(string token)
        {
            //  回傳驗證資料
            return Request.CreateResponse(HttpStatusCode.OK, new ApiResponseModel
            {
                Result = "Ok",
                Message = "No error.",
                Data = null,
            }, new JsonMediaTypeFormatter());
        }

        /// <summary>
        /// 登入Api
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("User/Login")]
        [HttpPost]
        public HttpResponseMessage Login(UserInfo input)
        {
            //  回傳物件
            object content;

            //  Fake 登入驗證
            UserInfo userInfo = CheckAndRtnUser(input.UserId, input.Password);

            //  驗證成功
            if (userInfo != null)
            {
                content = new ApiResponseModel
                {
                    Result = "Ok",
                    Message = "No error.",
                    Data = userInfo,
                    Token = TokenHelper.GenarateToken(userInfo, this.Request.GetClientIpString())
                };
            }
            else
            {
                content = new ApiResponseModel
                {
                    Result = "Error",
                    Message = "Login fail.",
                    Data = new { },
                    Token = string.Empty
                };
            }

            //  回傳驗證資料
            return Request.CreateResponse(HttpStatusCode.OK, content, new JsonMediaTypeFormatter());
        }

        /// <summary>
        /// 模擬使用者假資料
        /// </summary>
        /// <returns></returns>
        private List<UserInfo> QueryUserInfoList()
        {
            return new List<UserInfo>() {
                new UserInfo()
                {
                    UserId = "tom.wang",
                    UserName = "Tom",
                    Role = "1",
                    Company = "C01",
                    Email = "tom.wang@gmail.com",
                    Password= "tom.wang168",
                    PhoneNumber= "0912345678"
                },
                new UserInfo()
                {
                    UserId = "mary.chang",
                    UserName = "Mart",
                    Role = "2",
                    Company = "C02",
                    Email = "mary.chang@gmail.com",
                    Password= "mary.chang168",
                    PhoneNumber= "0987654321"
                }
            };
        }

        /// <summary>
        /// 檢查使用者登入訊息，並回傳使用者資料
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        private UserInfo CheckAndRtnUser(string userId, string pwd)
        {
            //  驗證並回傳使用者資料
            UserInfo currUserInfo = QueryUserInfoList().FirstOrDefault(p => p.UserId == userId && p.Password == pwd);

            return currUserInfo;
        }

        /// <summary>
        /// 取得使用者資訊，並回傳使用者資料
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private UserInfo GetUserByKey(string userId)
        {
            //  驗證並回傳使用者資料
            UserInfo currUserInfo = QueryUserInfoList().FirstOrDefault(p => p.UserId == userId);

            return currUserInfo;
        }
    }
}