using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HL7_TCP.Web.Models
{
    public class TcpSendResults
    {
        public int NumberMsgsSent { get; set; }
        public TimeSpan TimeElapsed { get; set; }
        public string ExceptionDuringSend { get; set; }
    }
}