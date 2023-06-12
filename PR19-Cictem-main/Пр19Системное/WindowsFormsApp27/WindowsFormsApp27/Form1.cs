using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp27
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
        }
        
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }
       
      
        private void button2_Click(object sender, EventArgs e)
        {
            
            UdpClient udp = new UdpClient();
            byte[] message = Encoding.Default.GetBytes(textBox2.Text);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(textBox4.Text), 15000);
            udp.Send(message, message.Length, ep);
            udp.Close();
        }
        bool stopReceive = false;
        Thread workReceive = null;
        UdpClient udp = null;

        // Функция запускаемая из дополниетльного потока
        void Receive()
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(textBox5.Text), 15000);
                udp = new UdpClient(ep);
                while (true)
                {
                    IPEndPoint remote = null;
                    byte[] message = udp.Receive(ref remote);
                    ShowMessage(Encoding.Default.GetString(message));
                    if (stopReceive == true) break;
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }
            finally
            {
                if (udp != null) udp.Close();
            }
        }


        // Специальный код доступа к свойствам объектов  из других потоков
        delegate void SetTextCallback(string message);
        void ShowMessage(string message)
        {
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback dt = new SetTextCallback(ShowMessage);
                this.Invoke(dt, new object[] { message });
            }
            else
            {
                this.textBox1.Text = message;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ThreadStart tstart = new ThreadStart(Receive);
            workReceive = new Thread(tstart);
            workReceive.Start();
        }
    }
}
