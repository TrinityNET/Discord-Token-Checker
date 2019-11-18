using EzConsoleClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace TrinityCord
{
    class Checker
    {
        public void CheckTokens()
        {
            EzConsole.WriteLine("\n Starting...\n", ConsoleColor.Green, false);
            foreach (string token in Variables.Tokens)
            {
                try
                {
                    using (var wc = new WebClient())
                    {
                        if (!Variables.ProxiesChecked)
                        {
                            foreach (string proxy in Variables.Proxies)
                            {
                                WebProxy wp = new WebProxy(proxy);
                                wc.Proxy = wp;
                            }
                        }
                        else
                        {
                            foreach (string proxy in Variables.WorkingProxies)
                            {
                                WebProxy wp = new WebProxy(proxy);
                                wc.Proxy = wp;
                            }
                        }

                        wc.Headers.Add("Content-Type", "application/json");
                        wc.Headers.Add(HttpRequestHeader.Authorization, token);
                        wc.DownloadString("https://discordapp.com/api/v7/users/@me/guilds");
                        Variables.WorkingTokens.Add(token);
                        Console.Title = $"TrinityCord | Tokens: {Variables.Tokens.Count} | Proxies: {Variables.Proxies.Count} | Checked Proxies: {Variables.CheckedProxies}/{Variables.Proxies.Count} | Working Tokens: {Variables.WorkingTokens.Count} | Invalid Tokens: {Variables.NonWorkingTokens.Count}";
                        EzConsole.WriteLine($" [+] Working token found - {token}", ConsoleColor.Green, false);
                    }
                }
                catch (WebException e)
                {
                    HttpWebResponse response = (HttpWebResponse)e.Response;

                    if (response.StatusCode == HttpStatusCode.Unauthorized) // 401 Token Invalid
                    {
                        Variables.NonWorkingTokens.Add(token);
                        Console.Title = $"TrinityCord | Tokens: {Variables.Tokens.Count} | Proxies: {Variables.Proxies.Count} | Checked Proxies: {Variables.CheckedProxies}/{Variables.Proxies.Count} | Working Tokens: {Variables.WorkingTokens.Count} | Invalid Tokens: {Variables.NonWorkingTokens.Count}";
                        EzConsole.WriteLine(" [X] Invalid token!", ConsoleColor.Red, false);
                    }
                    else
                    {
                        EzConsole.WriteLine(" [X] Proxy rate limited! Switching proxy...", ConsoleColor.Red, false);
                    }
                }
            }

            string date = DateTime.Now.ToString("dd MMM HH-mm-ss");

            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }

            if (!Directory.Exists($"Logs\\{date}"))
            {
                Directory.CreateDirectory($"Logs\\{date}");
            }

            File.WriteAllLines($"Logs\\{date}\\Tokens.txt", Variables.WorkingTokens);

            if (!Variables.ProxiesChecked)
            {
                File.WriteAllLines($"Logs\\{date}\\Proxies.txt", Variables.Proxies);
            }
            else
            {
                File.WriteAllLines($"Logs\\{date}\\WorkingProxies.txt", Variables.WorkingProxies);
            }

            EzConsole.WriteLine($"\n [+] Successfully checked all tokens! Saved to {Directory.GetCurrentDirectory()}\\Logs\\{date}", ConsoleColor.Cyan, false);
        }
    }
}
