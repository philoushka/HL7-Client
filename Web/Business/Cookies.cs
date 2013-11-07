using System;
using System.Web;

namespace HL7_TCP.Web
{
    public static class Cookies
    {
        public static void SetCookie(string key, object val)
        {
            var cookies = HttpContext.Current.Response.Cookies;
            if (cookies[key] == null)
            {
                HttpCookie cookie = new HttpCookie(key, val.ToString());
                cookie.Expires = DateTime.Now.AddYears(1);
                cookies.Add(cookie);
            }
            else
            {
                cookies[key].Value = val.ToString();
            }
        }

        public static string GetCookieVal(string key)
        {
            var cookies = HttpContext.Current.Request.Cookies;
            return (cookies[key] != null)
                ? cookies[key].Value
                : string.Empty;
        }
    }
}