using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HL7_TCP;

namespace HL7_TCP.WinFormClient
{
    public partial class HL7Client : Form
    {
        private const string DefaultHL7Message = @"MSH|^~\&|ADM|XXFOO|||20131105112744||QRY|foo.2013115122744467|D|2.1|
QRD|20131105112744|R|I|FOO1|||99^RD|A0-B20130306201633940^9879594577|MPI|
QRF|UPI||||9879594577^Regekhsot^Annabelle^Reg Only^F^19110408^PO Box 877^^Anytown^UT^98238-5125^255-644-8956^^^S|
";

        public HL7Client()
        {
            InitializeComponent();

        }

        private void btnFire_Click(object sender, EventArgs e)
        {
            portsResults.Items.Clear();
            SendMessages();
        }
      private  Dictionary<int, int> portResultsFromSender;
        private void SendMessages()
        {
             portResultsFromSender = new Dictionary<int, int>();
            BackgroundWorker bw = new BackgroundWorker { WorkerReportsProgress = true };

            int numMessagesToSend = int.Parse(txtNumToSend.Text);

            bw.DoWork += new DoWorkEventHandler(delegate(object o, DoWorkEventArgs args)
            {
                BackgroundWorker b = o as BackgroundWorker;

                var tcpSender = new TcpSender { DestinationServer = txtEndpoint.Text.Trim(), DestinationPort = int.Parse(txtPort.Text.Trim()) };
                for (int i = 1; i <= numMessagesToSend; i++)
                {
                    var response = tcpSender.SendHL7(DefaultHL7Message);

                    if (portResultsFromSender.ContainsKey(response.Port) == false)
                    { portResultsFromSender.Add(response.Port, 1); }
                    else
                    {
                        portResultsFromSender[response.Port] += 1;
                    }

                    b.ReportProgress((int)((double)i / numMessagesToSend * 100));                   
                }
            });

            // what to do when progress changed (update the progress bar for example)
            bw.ProgressChanged += new ProgressChangedEventHandler(delegate(object o, ProgressChangedEventArgs args)
            {
                lblProgress.Text = string.Format("{0}% Completed", args.ProgressPercentage);
            });

            // what to do when worker completes its task (notify the user)
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(delegate(object o, RunWorkerCompletedEventArgs args)
            {
                label1.Text = "Finished!";

                portsResults.Visible = true;

                foreach (var item in portResultsFromSender.OrderByDescending(x=>x.Value)  )
                {
                    portsResults.Items.Add(string.Format("Port {0}: {1} messages",item.Key, item.Value));
                }
                
            });

            bw.RunWorkerAsync();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblProgress.Text = "";
            portsResults.Visible = false;
            txtMsg.Text = DefaultHL7Message;
        }
    }
}
