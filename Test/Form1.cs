using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using Test;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Configuration;

namespace Test
{
    public partial class Form1 : Form
    {
        private NetworkStream socketStream;
        private BinaryWriter writer;
        private BinaryReader reader;
        private Socket connection;
        private Thread readThread;

        DbUtil db = new DbUtil();//ado
        Test2DataContext t = new Test2DataContext();//linq

        string s = "";
        string ss = "";
        public Form1()
        {
            InitializeComponent();
          
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        
            using (SqlConnection connectionString = db.GetSqlConnection(db.GetConnectionString()))
            {
                using (SqlCommand sqlCommand = new SqlCommand("Select * from Users;", connectionString))
                {
                    SqlDataReader sqlReader = sqlCommand.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        string user = sqlReader.GetString(0);
                        string pass = sqlReader.GetString(1);
                        if (user.Equals("ghadeer"))
                        {
                            textBox1.Text = pass;
                            s = pass;
                        }
                    }
                }
            }
            // Using LINQ
            User userw = (from us in t.GetTable<User>()
                         where us.pass.Equals("1234")
                         select us).Single<User>();
            ss = userw.user1.ToString();
            textBox3.Text = ss;
        }



        public void Send()
        {
            TcpListener listener;

            try
            {

                listener = new TcpListener(5001);
                listener.Start();
                while (true)
                {
                    connection = listener.AcceptSocket();
                    socketStream = new NetworkStream(connection);
                    writer = new BinaryWriter(socketStream);
                    reader = new BinaryReader(socketStream);

                    string message = "";
                    do
                    {
                        try
                        {
                           writer.Write(s);
                           message = reader.ReadString();
                           Action updateLabel = () => textBox2.Text = message;
                           textBox1.Invoke(updateLabel);
                           writer.Write(ss);
                        }
                        catch (Exception)
                        {
                            break;
                        }

                    } while (s != "TERMINATE" && connection.Connected);
                    writer.Close();
                    reader.Close();
                    socketStream.Close();
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Client c = new Client();
            c.Show();
            readThread = new Thread(new ThreadStart(Send));
            readThread.Start();
        }
    }
}