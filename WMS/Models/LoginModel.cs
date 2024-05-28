using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WMS.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "請輸入使用者名稱")]
        public string Username { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; }

        internal static object FirstOrDefault(Func<object, bool> p)
        {
            throw new NotImplementedException();
        }
    }
}