using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiApp.Extensions
{
    /// <summary>
    /// Api 回傳內容
    /// </summary>
    public class ApiResponseModel
    {
        /// <summary>
        /// 結果: Ok, Error
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 回傳資料
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
    }
}