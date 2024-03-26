using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using Newtonsoft.Json;

namespace CustomCMD
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Made by Jaden --- ? / help");
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("");
                Console.Write("Console> ");
                string input = Console.ReadLine();

                if (input.Equals("help") || input.Equals("?"))
                {
                    Console.WriteLine("Command -- List");
                    Console.WriteLine();
                    Console.WriteLine("cl/clear -> Löscht einträge aus der Konsole.");
                    Console.WriteLine("ip -> Bekomme deine IP.");
                    Console.WriteLine("ping -> Pinge eine IP.");
                    Console.WriteLine("svw -> Sende eine Discord Nachricht via Webhook.");
                    continue;
                }


                if (string.IsNullOrEmpty(input))
                {
                    continue;
                }

                if (input.Equals("ip"))
                {
                    string hostName = Dns.GetHostName();
                    IPHostEntry ipEntry = Dns.GetHostEntry(hostName);
                    IPAddress[] addr = ipEntry.AddressList;

                    Console.WriteLine("Deine IP: " + addr[0].ToString());
                    continue;
                }

                if (input.StartsWith("ping"))
                {
                    string[] words = input.Split(' ');

                    if (words.Length != 1)
                    {
                        string ipOrAddress = words[1];
                        try
                        {
                            Ping ping = new Ping();
                            PingReply pingReply = ping.Send(ipOrAddress);

                            Console.WriteLine($"Antwort von {ipOrAddress}: Status = {pingReply.Status}");
                            Console.WriteLine($"Round Trip Time (RTT) = {pingReply.RoundtripTime}ms");
                            Console.WriteLine($"Time to live (TTL) = {pingReply.Options.Ttl}");
                            continue;
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            msgError("IP wurde nicht gefunden.");
                            continue;
                        }
                    } else
                    {
                        argError("Gebe eine IP an!");
                        continue;
                    }
                }

                if (input.Equals("clear") || input.Equals("cl"))
                {
                    Console.Clear();
                    Console.WriteLine("Willkommen in der Custom Konsole erstellt von Verpxnter(Jaden)");
                    Console.WriteLine("");
                    continue;
                }

                if (input.StartsWith("svw"))
                {
                    string[] words = input.Split(' ');

                    if (words.Length > 2)
                    {
                        string message = string.Join(" ", words.Skip(2));
                        string webhookUrl = words[1];

                        SendMessageToWebhook(webhookUrl, message).Wait();

                        continue;
                    }
                    else
                    {
                        argError("Gebe einen Webhook sowie eine Nachricht an. (svw 'webhook' 'message')");
                        continue;
                    }
                }

                msgError("Der angegebene Command wurde nicht gefunden!");
            }
        }

        private static async Task SendMessageToWebhook(string webhookUrl, string message)
        {
            using (HttpClient client = new HttpClient())
            {
                var data = new
                {
                    content = message
                };

                var content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(webhookUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    msgSuccess("Die Nachricht wurde erfolgreich an den Webhook gesendet!");
                }
                else
                {
                    msgError("Der angegebene Webhook wurde nicht gefunden!");
                }
            }
        }

        private static void msgSuccess(String message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            string msg = "Success> " + message;
            Console.WriteLine(msg);
        }

        private static void msgError(String message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            string msg = "Error> " + message;
            Console.WriteLine(msg);
        }

        private static void argError(String message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            string msg = "Args> " + message;
            Console.WriteLine(msg);
        }

    }

}