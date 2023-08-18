using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiApp.Extensions
{
    public class UserInfo
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Company { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}