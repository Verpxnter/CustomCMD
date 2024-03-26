using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace CustomCMD
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Willkommen in der Custom Konsole erstellt von Verpxnter(Jaden)");
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("");
                Console.Write("Console> ");
                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    continue;
                }

                if (input.Equals("test"))
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
                        Console.WriteLine("Gebe einen Webhook sowie eine Nachricht an. (svw 'webhook' 'message')");
                        continue;
                    }
                }

                Console.WriteLine($"Der angegebene Command wurde nicht gefunden!");
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
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Success> Die Nachricht wurde erfolgreich an Discord gesendet!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error> Der Webhook wurde nicht gefunden!");
                }
            }
        }
    }
}