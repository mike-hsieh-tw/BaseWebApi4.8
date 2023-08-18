using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Text;

namespace WebApiApp.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        private const string _HttpContext = "MS_HttpContext";
        private const string _RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
        private const string _OwinContext = "MS_OwinContext";

        /// <summary>
        /// 取得Request所有資訊
        /// </summary>
        public static string GetRequestDetailInfo()
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;

            StringBuilder sbRequest = new StringBuilder();
            for (int i = 0; i < request.Headers.Count; i++)
            {
                sbRequest.AppendLine(request.Headers.GetKey(i).ToString() + "=" + request.Headers[i] + "▼");
            }
            for (int i = 0; i < request.ServerVariables.Count; i++)
            {
                sbRequest.AppendLine(request.ServerVariables.GetKey(i).ToString() + "=" + request.ServerVariables[i] + "▼");
            }

            return sbRequest.ToString();
        }

        public static string GetClientIpString(this HttpRequestMessage request)
        {
            //Web-hosting
            if (request.Properties.ContainsKey(_HttpContext))
            {
                dynamic ctx = request.Properties[_HttpContext];
                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }
            //Self-hosting
            if (request.Properties.ContainsKey(_RemoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[_RemoteEndpointMessage];
                if (remoteEndpoint != null)
                {
                    return remoteEndpoint.Address;
                }
            }
            //Owin-hosting
            if (request.Properties.ContainsKey(_OwinContext))
            {
                dynamic ctx = request.Properties[_OwinContext];
                if (ctx != null)
                {
                    return ctx.Request.RemoteIpAddress;
                }
            }
            if (System.Web.HttpContext.Current != null)
            {
                return System.Web.HttpContext.Current.Request.UserHostAddress;
            }
            // Always return all zeroes for any failure
            return "0.0.0.0";
        }

        public static IPAddress GetClientIpAddress(this HttpRequestMessage request)
        {
            var ipString = request.GetClientIpString();
            IPAddress ipAddress = new IPAddress(0);
            if (IPAddress.TryParse(ipString, out ipAddress))
            {
                return ipAddress;
            }

            return ipAddress;
        }
    }
}