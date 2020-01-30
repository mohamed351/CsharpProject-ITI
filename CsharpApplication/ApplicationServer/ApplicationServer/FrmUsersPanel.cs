using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace OurGameSever
{
    public partial class FrmUsersPanel : MaterialSkin.Controls.MaterialForm
    {
        Thread _thread;
        TcpListener _listener;
        Socket _socket;
        public FrmUsersPanel()
        {
            InitializeComponent();
            this.FormClosed += FrmUsersPanel_FormClosed;
           
            string[] sp = ServerSettings.Default.IpAddress.Split('.');
            byte[] ipaddress = new byte[4];
            for (int i = 0; i < ipaddress.Length; i++)
            {
                ipaddress[i] = Convert.ToByte(sp[i]);
            }

            _listener = new TcpListener(new IPAddress(ipaddress), ServerSettings.Default.PortNumber);

            _listener.Start();


            _thread = new Thread(NetWorkWork);
            _thread.Start();
          
            
        }

        private void FrmUsersPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
           
           
            _listener.Stop();
            _thread.Abort();
            
            
        }

        public void NetWorkWork()
        {
            while (true)
            {



              

                _socket = _listener.AcceptSocket();



                NetworkStream stream = new NetworkStream(_socket);
                string fullstring = "";
                while (stream.DataAvailable)
                {
                  char i = (char) stream.ReadByte();
                    fullstring += i;

                }
                //check the value

                MessageBox.Show(fullstring);

                stream.Close();
            }
         
            
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            //using (frmUsers frm = new frmUsers())
            //{
            //    if(frm.ShowDialog() == DialogResult.OK)
            //    {

            //    }
            //}
        }

        private void FrmUsersPanel_Load(object sender, EventArgs e)
        {

        }
    }
}
