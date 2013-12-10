using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using HL7_TCP.Extensions;

namespace HL7_TCP
{
    public class TcpSendResult
    {
        public bool SuccessfulSend { get; set; }
        public string Response { get; set; }
        public int Port { get; set; }
    }

    public class TcpSender
    {
        private const string MsgTransmit = "\v{0}{1}\r";
        private char FileSep = Convert.ToChar(28);


        /// <summary>
        /// The destination endpoint. This could be an IP address, a hostname, or a DNS alias.
        /// </summary>
        public string DestinationServer { get; set; }

        /// <summary>
        /// The port on the destination server to send to.
        /// </summary>
        public int DestinationPort { get; set; }

        /// <summary>
        /// Wrap the given HL7 message in HL7 control characters, and send that to the destination/port. 
        /// </summary>
        /// <param name="hl7Message">The HL7 message you want to send.</param>
        /// <returns>Boolean successful send.</returns>
        public TcpSendResult SendHL7(string hl7Message)
        {
            try
            {
                byte[] bytesToXmit = BuildBytesToTransmit(hl7Message);
                using (Socket s = ConnectSocket(DestinationServer, DestinationPort))
                {
                    if (s == null) return new TcpSendResult { SuccessfulSend = false };
                    s.Send(bytesToXmit, bytesToXmit.Length, 0);
                    byte[] bytesReceived = new byte[1024];
                    int totalBytesReceived = s.Receive(bytesReceived, bytesReceived.Length, 0);
                    string hl7Response = Encoding.ASCII.GetString(bytesReceived, 0, totalBytesReceived);
                    return new TcpSendResult { SuccessfulSend = true, Response = hl7Response, Port = int.Parse(hl7Response.Split(' ').Last()) };
                }
            }
            catch (Exception e)
            {
                return new TcpSendResult { SuccessfulSend = false };
            }
        }
        /// <summary>
        /// Wraps your HL7 message to transmit with the leading (v-tab) and trailing (FileSep + New Line)control characters.
        /// </summary>
        /// <param name="hl7Message">The message to be wrapped with HL7 control characters</param>
        /// <returns>A byte array of the message wrapped with HL7 control characters</returns>
        private byte[] BuildBytesToTransmit(string hl7Message)
        {
            string wrappedMessage = MsgTransmit.FormatWith(hl7Message, FileSep.ToString());
            return Encoding.ASCII.GetBytes(wrappedMessage);
        }
        /// <summary>
        /// Determine whether the HL7 response from the destination.endpoint indicates a successful receipt of the sent message. It may include an ACK or a NACK or nothing.
        /// </summary>
        /// <param name="response">The response from the destination/endpoint</param>
        /// <returns>Whether we detected a successful receipt from the server.</returns>
        private bool DetermineSuccessFromResponseHL7Message(string response)
        {
            return (response.HasValue() && response.Contains("MSA|AA"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="server">The destination endpoint to connect to. This could be an IP address, or a DNS hostname or alias.</param>
        /// <param name="port">The numeric port to connect to.</param>
        /// <returns></returns>
        private Socket ConnectSocket(string server, int port)
        {
            Socket s = null;
            IPHostEntry hostEntry = Dns.GetHostEntry(server);

            foreach (IPAddress address in hostEntry.AddressList.Where(x=>x.AddressFamily== AddressFamily.InterNetwork))
            {
                IPEndPoint ipe = new IPEndPoint(address, port);
                Socket tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                tempSocket.ReceiveTimeout = Config.SocketReceiveTimeOutMilliseconds;
                tempSocket.Connect(ipe);

                if (tempSocket.Connected)
                {
                    s = tempSocket;
                    break;
                }
                else
                {
                    continue;
                }
            }
            return s;
        }

        /// <summary>
        /// Peek-test the supplied destination/port for connectivity.
        /// </summary>
        /// <returns></returns>
        public bool DestinationTestConnect()
        {
            try
            {
                using (Socket s = ConnectSocket(DestinationServer, DestinationPort))
                {
                    return (s != null);
                }
            }
            catch (Exception) { return false; }
        }
    }
}
