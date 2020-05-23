using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Configuration;
using System.IO;

namespace Test
{
    public partial class Client : Form
    {
        private NetworkStream output;
        private BinaryWriter writer;
        private BinaryReader reader;
        private Socket connection;
        private Thread readThread;

        public Client()
        {
            InitializeComponent();
            readThread = new Thread(new ThreadStart(Play));
            readThread.Start();
        }
        public void Play()
        {
            try
            {
                TcpClient client;


                client = new TcpClient();
                client.Connect("localhost", 5001);
                output = client.GetStream();
                writer = new BinaryWriter(output);
                reader = new BinaryReader(output);

                string message = "";
                string w = "";
                do
                {
                    try
                    {
                        message = reader.ReadString();
                        Action updateLabel = () => textBox1.Text = message;
                        textBox1.Invoke(updateLabel);
                        writer.Write("ghadeer");
                        w = reader.ReadString();
                        Action updateLabel1 = () => textBox2.Text =w;
                        textBox2.Invoke(updateLabel1);

                    }
                    catch (Exception ex)
                    {
                    }
                } while (message != "ter");


                writer.Close();
                reader.Close();
                output.Close();
                connection.Close();


            }

            catch (Exception ex)
            {
            }

        }
    }
}