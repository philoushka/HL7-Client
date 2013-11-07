using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HL7_TCP
{
    public class TcpSender
    {

        public string DestinationServer { get; set; }
        public int Port { get; set; }

        public bool SendHL7(string hl7Message)
        {
            try
            {
                byte[] bytesToXmit = BuildBytesToTransmit(hl7Message);
                using (Socket s = ConnectSocket(DestinationServer, Port))
                {
                    if (s == null) return false;
                    s.Send(bytesToXmit, bytesToXmit.Length, 0);
                    byte[] bytesReceived = new byte[1024];
                    int totalBytesReceived = s.Receive(bytesReceived, bytesReceived.Length, 0);
                    string hl7Response = Encoding.ASCII.GetString(bytesReceived, 0, totalBytesReceived);
                    return DetermineSuccessFromResponseHL7Message(hl7Response);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        private byte[] BuildBytesToTransmit(string hl7Message)
        {
            char FileSep = Convert.ToChar(28);
            const string MsgTransmit = "\v{0}{1}\r";
            string wrappedMessage = string.Format(MsgTransmit, hl7Message, FileSep.ToString());
            return Encoding.ASCII.GetBytes(wrappedMessage);
        }
        private bool DetermineSuccessFromResponseHL7Message(string response)
        {
            return (response.Contains("MSA|AA"));
        }

        private Socket ConnectSocket(string server, int port)
        {
            Socket s = null;
            IPHostEntry hostEntry = Dns.GetHostEntry(server);

            foreach (IPAddress address in hostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(address, port);
                Socket tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                tempSocket.ReceiveTimeout = 3000;
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

        public bool DestinationTestConnect()
        {
            try
            {
                using (Socket s = ConnectSocket(DestinationServer, Port))
                {
                    return (s != null);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
