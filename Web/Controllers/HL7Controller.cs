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
                DestinationServer = Cookies.GetCookieVal("Server"),
                HL7MessageToSend = Cookies.GetCookieVal("HL7Message-").Replace(NewLineToken, Environment.NewLine),
                DestinationPort = Cookies.GetCookieVal("Port").ToNullableInt(),
                NumMessages = Cookies.GetCookieVal("NumMsgs").ToNullableInt()
            };
        }

        [HttpPost]
        public ActionResult SendHL7Message(SendHL7ViewModel model)
        {
            if (TryValidateModel(model))
            {
                SetUserValuesToCookie(model);

                HL7Send hl7Service = new HL7Send();
                ViewBag.Message = hl7Service.SendBatchMessages(model);
            }
            return View("Index", model);
        }

        private void SetUserValuesToCookie(SendHL7ViewModel model)
        {
            Cookies.SetCookie("NumMsgs", model.NumMessages);
            Cookies.SetCookie("HL7Message-", model.HL7MessageToSend.Replace(Environment.NewLine, NewLineToken));
            Cookies.SetCookie("Port", model.DestinationPort);
            Cookies.SetCookie("Server", model.DestinationServer);
        }
    }
}
