using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7_TCP
{
    public static class Config
    {
        public static int SocketReceiveTimeOutMilliseconds { get { return int.Parse(ConfigurationManager.AppSettings["SocketReceiveTimeOutMilliseconds"]); } }
    }
}
