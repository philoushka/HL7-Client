using HL7_TCP.Web.Models;
using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;

namespace HL7_TCP.Web.Controllers
{
    public class HL7Controller : Controller
    {
        /// <summary>
        /// JavaScript replaces newline \r\n with underscore. Use this value as a replacement to for newlines. 
        /// Replace in when saving to cookie, and replace out with newline when reading from cookie/
        /// </summary>
        const string NewLineToken = @"{-}";
        public ActionResult Index()
        {
            var vm = GatherValuesFromCookies();
            return View(vm);
        }

        private SendHL7ViewModel GatherValuesFromCookies()
        {
            return new SendHL7ViewModel
            {
                DestinationServer = GetCookieVal("Server"),
                HL7MessageToSend = GetCookieVal("HL7Message-").Replace(NewLineToken, Environment.NewLine),
                DestinationPort = GetCookieVal("Port").ToNullableInt(),
                NumMessages = GetCookieVal("NumMsgs").ToNullableInt()
            };
        }

        private string GetCookieVal(string key)
        {
            return (Request.Cookies[key] != null)
                ? Request.Cookies[key].Value
                : string.Empty;
        }

        [HttpPost]
        public ActionResult SendHL7Message(SendHL7ViewModel model)
        {
            if (TryValidateModel(model))
            {
                SetUserValuesToCookie(model);

                HL7Send hl7Service = new HL7Send();
                ViewBag.Message =  hl7Service.SendBatchMessages(model);   
            }

            return View("Index", model);
        }

       

        private void SetUserValuesToCookie(SendHL7ViewModel model)
        {
            SetCookie("NumMsgs", model.NumMessages);
            SetCookie("HL7Message-", model.HL7MessageToSend.Replace(Environment.NewLine, NewLineToken));
            SetCookie("Port", model.DestinationPort);
            SetCookie("Server", model.DestinationServer);
        }

        private void SetCookie(string key, object val)
        {
            if (Response.Cookies[key] == null)
            {
                HttpCookie cookie = new HttpCookie(key, val.ToString());
                cookie.Expires = DateTime.Now.AddYears(1);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                Response.Cookies[key].Value = val.ToString();
            }
        }


    }


}
