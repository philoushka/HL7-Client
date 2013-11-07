using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HL7_TCP.Web.Models
{
    public class SendHL7ViewModel
    {
        public SendHL7ViewModel()
        {
            NumMessages = 1;
        }
        public int? NumMessages { get; set; }
        public string HL7MessageToSend { get; set; }
        public string DestinationServer { get; set; }
        public int? DestinationPort { get; set; }

        public string DestinationDetails { get { return "{0}:{1}".FormatWith(DestinationServer, DestinationPort); } }
    }
}