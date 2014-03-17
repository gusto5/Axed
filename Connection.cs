using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Axed
{
    class Connection
    {
        public Random random;
        private int Count;
        private string SSO;
        private Socket mSocket;
        public bool inRoom;

        public Connection(int count, string sso)
        {
            this.random = new Random();
            this.Count = count;
            this.SSO = sso;
        }

        public void Load()
        {
            try
            {
                this.mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.mSocket.Connect("s1.defendport.com", 1337);

                this.Send("F_" + Encoder.encodeLength(this.SSO.Length) + this.SSO);

                inRoom = false;

                this.ListenThread();
            }
            catch { }

            Console.ReadLine();
        }

        public void ListenThread()
        {
            int mRead;
            byte[] mBuffer = new byte[512];

            while (true)
            {
                mRead = 0;

                try
                {
                    mRead = mSocket.Receive(mBuffer, 0, 512, SocketFlags.None);
                }
                catch
                {
                    break;
                }

                if (mRead == 0)
                {
                    break;
                }

                string mString = Encoding.UTF8.GetString(mBuffer, 0, mRead);

               // Console.WriteLine(string.Format("REC >> {0}", mString));

                if (mString.Contains("@BJJ"))
                {
                    Send("@L");

                    Console.WriteLine("Logged in woo!");


                    Send(Encoder.encodeLength(391) + Encoder.encodeInt(Program.RoomId) + Encoder.encodeInt(0) + Encoder.encodeInt(0));
                }

                if (mString.StartsWith("@S"))
                {
                    Send("FA" + Encoder.encodeInt(Program.RoomId));
                }

                if (mString.StartsWith("GF"))
                {
                    Send(Encoder.encodeLength(390));
                    inRoom = true;

                    String figure = "hd-180-5.lg-270-64.sh-300-64.hr-110-45.cc-260-62.ch-210-66";
                    Send(Encoder.encodeLength(44) + Encoder.encodeLength("M".Length) + "M" + Encoder.encodeLength(figure.Length) + figure);
                }

                if (mString.StartsWith("J|"))
                {
                    Send("FA" + Encoder.encodeInt(Program.RoomId));
                }
            }
            Load();
        }

        public void Send(string data)
        {
            try
            {
                data = "@" + Encoder.encodeLength(data.Length) + data;

                //Console.WriteLine(string.Format("SENT << {0}", data));

                byte[] buffer = Encoding.Default.GetBytes(data);
                mSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            }
            catch { }
        }

        public void SendRaw(string data)
        {
            byte[] buffer = Encoding.Default.GetBytes(data);
            mSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }
    }
}
