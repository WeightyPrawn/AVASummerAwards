using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Awards.Helpers
{
    public static class AuthHelper
    {
        public static bool IsValidUserDomain(string user)
        {
            Regex regex = new Regex("(?<=@).*", RegexOptions.IgnoreCase);
            var domain = regex.Match(user);
            return (domain.Value == "avanade.com") ? true : false;
        }

        public static bool IsAdminUser(string user)
        {
            return (user == "patrik.hegethorn@avanade.com") ? true : false;
        }
    }
}