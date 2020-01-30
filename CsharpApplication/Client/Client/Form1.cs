using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            new Thread(RunIt);
        }
        Thread _thread;
        private void button1_Click(object sender, EventArgs e)
        {
            TcpClient client = new TcpClient("127.0.0.1", 50003);
          
            Users users = new Users()
            {

                Name = txtName.Text,
                Age = Convert.ToInt32(txtAge.Text),
                Password = txtPassword.Text,
                UserName = txtUserName.Text

            };
           string userObject = JsonConvert.SerializeObject(users);
           

           byte[] vs =  ASCIIEncoding.Default.GetBytes(userObject);
           
            client.GetStream().Write(vs, 0, vs.Length);

        }
        public static void RunIt()
        {

        }
    }
}
