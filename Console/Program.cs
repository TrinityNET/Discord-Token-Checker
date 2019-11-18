using EzConsoleClass;
using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace TrinityCord
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "TrinityCord | Tokens: 0 | Proxies: 0 | Checked Proxies: 0 | Working Tokens: 0 | Invalid Tokens: 0";

            Watermark();

            #region File Manager
            if (!File.Exists("proxies.txt") && !File.Exists("tokens.txt"))
            {
                File.Create("proxies.txt");
                File.Create("tokens.txt");
                EzConsole.WriteLine(" [!] Files `tokens.txt` and `proxies.txt` have been created for you, please fill them in!", ConsoleColor.Red, true);
            }

            if (!File.Exists("proxies.txt"))
            {
                File.Create("proxies.txt");
                EzConsole.WriteLine(" [!] Please put your proxies into the 'proxies.txt' file!", ConsoleColor.Red, true);
            }

            if (!File.Exists("tokens.txt"))
            {
                File.Create("tokens.txt");
                EzConsole.WriteLine(" [!] Please put the Discord tokens into the 'tokens.txt' file!", ConsoleColor.Red, true);
            }
            #endregion

            #region Loader
            int tokens = 0;
            int proxies = 0;

            foreach(string token in File.ReadAllLines("tokens.txt"))
            {
                tokens++;
                Variables.Tokens.Add(token);
            }

            foreach(string proxy in File.ReadAllLines("proxies.txt"))
            {
                proxies++;
                Variables.Proxies.Add(proxy);
            }

            if (tokens <= 0)
            {
                EzConsole.WriteLine($" [+] `tokens.txt` is empty, please input Discord token(s)", ConsoleColor.Red, true);
            }

            if (proxies <= 0)
            {
                EzConsole.WriteLine($" [+] No proxies found in `proxies.txt`!", ConsoleColor.Red, true);
            }

            Console.Title = $"TrinityCord | Tokens: {tokens} | Proxies: {proxies} | Checked Proxies: 0 | Working Tokens: 0 | Invalid Tokens: 0";

            EzConsole.WriteLine($" [+] Successfully loaded {tokens} tokens and {proxies} proxies!", ConsoleColor.Green, false);
            #endregion

            #region Proxy Checking

            EzConsole.Write($"\n [+] Would you like to check proxies before continuing? (y/n): ", ConsoleColor.Green, false);

            switch (Console.ReadLine())
            {
                case "n":
                    goto Start;
                case "N":
                    goto Start;
            }

            string ip = string.Empty;

            foreach (string proxy in Variables.Proxies)
            {
                ip = proxy.Split(':')[0];
                var ping = new Ping();
                var reply = ping.Send(ip);
                if (reply.Status == IPStatus.Success)
                {
                    Variables.WorkingProxies.Add(proxy);
                    Variables.CheckedProxies++;
                    EzConsole.WriteLine($" [+] {ip} - ({reply.RoundtripTime}ms)", ConsoleColor.Green, false);
                    Console.Title = $"TrinityCord | Tokens: {tokens} | Proxies: {proxies} | Checked Proxies: {Variables.CheckedProxies}/{Variables.Proxies.Count} | Working Tokens: 0 | Invalid Tokens: 0";
                }
                else
                {
                    Variables.NonWorkingProxies++;
                    Variables.CheckedProxies++;
                    EzConsole.WriteLine($" [-] {ip}", ConsoleColor.Red, false);
                    Console.Title = $"TrinityCord | Tokens: {tokens} | Proxies: {proxies} | Checked Proxies: {Variables.CheckedProxies}/{Variables.Proxies.Count} | Working Tokens: 0 | Invalid Tokens: 0";
                }
            }

            Variables.ProxiesChecked = true;
            EzConsole.WriteLine($" [+] Successfully loaded {Variables.WorkingProxies.Count} working proxies and removed {Variables.NonWorkingProxies} dead proxies!", ConsoleColor.Green, false);

        #endregion

        Start:
            StartChecker();

            Console.Read();
        }

        static void StartChecker()
        {
            Checker c = new Checker();
            Task.Run(() => c.CheckTokens());
            Console.ReadLine();
        }

        static void Watermark()
        {
            EzConsole.WriteLine("            ████████╗██████╗ ██╗███╗   ██╗██╗████████╗██╗   ██╗ ██████╗ ██████╗ ██████╗ ██████╗ ", ConsoleColor.Green, false);
            EzConsole.WriteLine("            ╚══██╔══╝██╔══██╗██║████╗  ██║██║╚══██╔══╝╚██╗ ██╔╝██╔════╝██╔═══██╗██╔══██╗██╔══██╗", ConsoleColor.Green, false);
            EzConsole.WriteLine("               ██║   ██████╔╝██║██╔██╗ ██║██║   ██║    ╚████╔╝ ██║     ██║   ██║██████╔╝██║  ██║", ConsoleColor.Green, false);
            EzConsole.WriteLine("               ██║   ██╔══██╗██║██║╚██╗██║██║   ██║     ╚██╔╝  ██║     ██║   ██║██╔══██╗██║  ██║", ConsoleColor.Green, false);
            EzConsole.WriteLine("               ██║   ██║  ██║██║██║ ╚████║██║   ██║      ██║   ╚██████╗╚██████╔╝██║  ██║██████╔╝", ConsoleColor.Green, false);
            EzConsole.WriteLine("               ╚═╝   ╚═╝  ╚═╝╚═╝╚═╝  ╚═══╝╚═╝   ╚═╝      ╚═╝    ╚═════╝ ╚═════╝ ╚═╝  ╚═╝╚═════╝ ", ConsoleColor.Green, false);
            EzConsole.WriteLine("                                                                                                ", ConsoleColor.Green, false);
        }
    }
}
