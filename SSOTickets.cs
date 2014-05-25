using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.IO;

namespace Axed
{
    class SSOTickets
    {
        public static List<String> Accounts;

        public static void Load()
        {
            Accounts = new List<string>();

            if (File.Exists("sso-cache.txt"))
            {
                foreach (String line in File.ReadAllLines("sso-cache.txt"))
                {
                    Accounts.Add(line);
                }
            }
            else
            {
                List<String> SSO = new List<string>();

                foreach (String line in File.ReadAllLines("bot-log.txt"))
                {
                    if (!line.StartsWith("Credentials"))
                    {
                        continue;
                    }

                    string[] credentials = line.Replace("Credentials: ", "").Split(',');

                    String username = credentials[0];
                    String password = credentials[1];

                    Console.WriteLine(password);

                    using (var client = new SSOClient())
                    {
                        var values = new NameValueCollection { { "credentials.username", username }, { "credentials.password", password }, { "_login_remember_me", "true" } };

                        client.UploadValues("http://vebbo.fr/account/submit", values);
                        string clientSource = client.DownloadString("http://vebbo.fr/client?novote");

                        foreach (string source in clientSource.Split(Environment.NewLine.ToCharArray()))
                        {

                            Console.WriteLine(source);
                            if (source.Contains("\"sso.ticket"))
                            {
                                string sso = source.Replace("\"sso.ticket\" : \"", "").Replace("\", ", "").Replace(" ", "");

                                Console.WriteLine("SSO Ticket >> " + sso);

                                //File.AppendAllText("sso-cache.txt", sso + Environment.NewLine);

                                SSO.Add(sso);
                                Accounts.Add(sso);
                            }
                        }
                    }
                }

                File.WriteAllLines("sso-cache.txt", SSO);
            }

            Console.WriteLine("Loaded " + Accounts.Count + " SSO tickets!");
        }
    }
}
