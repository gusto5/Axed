using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Axed
{
    class Program
    {
        public static Random random = new Random();
        public static List<Connection> Connections = new List<Connection>();
        public static int RoomId;

        static void Main(string[] args)
        {
            Console.Title = "Axed: Phoenix Bot Program";

            Console.WriteLine("##################");
            Console.WriteLine("##    Axed      ##");
            Console.WriteLine("##################");
            Console.WriteLine("## By Quackster ##");
            Console.WriteLine("##################");
            Console.WriteLine();

            Console.WriteLine("Loading SSO tickets");
            SSOTickets.Load();

            int i = 0;

            RoomId = 4378;//random.Next(1000, 4000);

            foreach (String sso in SSOTickets.Accounts)
            {
                Connection connection = new Connection(i++, sso);

                Connections.Add(connection);

                Thread thread = new Thread(new ThreadStart(connection.Load));
                thread.Start();
            }

            Thread walker = new Thread(new ThreadStart(Walk));
            walker.Start();


            Console.Read();
        }

        public static void Walk()
        {
            while (true)
            {
                foreach (Connection connection in Connections)
                {
                    if (!connection.inRoom)
                        continue;

                    connection.Send("AK" + Encoder.encodeInt(random.Next(0, 30)) + Encoder.encodeInt(random.Next(0, 30)));

                    if (random.Next(100) % 2 == 0)
                    {
                        String message = "POOLS CLOSED DUE TO AIDS";
                        connection.Send(Encoder.encodeLength(55) + Encoder.encodeLength(message.Length) + message);
                    }
                    else
                    {
                        String message = "WE ARE THE /B/BROTHERS OF 4CHAN";
                        connection.Send(Encoder.encodeLength(55) + Encoder.encodeLength(message.Length) + message);
                    }
                }
                
                Thread.Sleep(2000);
            }
        }
    }
}
