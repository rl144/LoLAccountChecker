using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LoL_Account_Checker
{
    class Utils
    {
        public static List<LoginData> GetLogins(string file)
        {
            var logins = new List<LoginData>();

            try
            {
                var sr = new StreamReader(file);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var accountData = line.Split(new[] { ':' });

                    if (accountData.Count() < 2)
                    {
                        continue;
                    }

                    var loginData = new LoginData(accountData[0], accountData[1]);

                    logins.Add(loginData);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return logins;
        }

        public static void ExportAsHtml(string file, List<AccountData> accounts, bool exportErrors)
        {
            var sb = new StringBuilder();

            sb.Append("<!DOCTYPE html>\n");
            sb.Append("<html>\n");
            sb.Append(" <header>\n");
            sb.Append("     <title>LoL Account Checker</title>\n");
            sb.Append("     <style>\n");
            sb.Append("         table { width: 100%; border: 1px solid #000000; }");
            sb.Append("         th, td { border: 1px solid #000000; }");
            sb.Append("         .a { background-color: #EEEEEE; }");
            sb.Append("         .b { background-color: #FFFFFF; }");
            sb.Append("     </style>\n");
            sb.Append("     <script type="text/javascript">function sortTable(t,e,o){var l,n=t.tBodies[0],a=Array.prototype.slice.call(n.rows,0);for(o=-(+o||-1),a=a.sort(function(t,l){return o*t.cells[e].textContent.trim().localeCompare(l.cells[e].textContent.trim())}),l=0;l<a.length;++l)n.appendChild(a[l])}function makeSortable(t){var e,o=t.tHead;if(o&&(o=o.rows[0])&&(o=o.cells),o)for(e=o.length;--e>=0;)(function(e){var l=1;o[e].addEventListener("click",function(){sortTable(t,e,l=1-l)})})(e)}function makeAllSortable(t){t=t||document.body;for(var e=t.getElementsByTagName("table"),o=e.length;--o>=0;)makeSortable(e[o])}window.onload=function(){makeAllSortable()};</script>\n");
            sb.Append(" </header>");
            sb.Append(" <body>");

            sb.Append("     <table>\n");
            sb.Append("     <thead>\n")
            sb.Append("         <tr>\n");
            sb.Append("             <th>Username</th>\n");
            sb.Append("             <th>Password</th>\n");
            sb.Append("             <th>Summoner Name</th>\n");
            sb.Append("             <th>Level</th>\n");
            sb.Append("             <th>Email Status</th>\n");
            sb.Append("             <th>RP</th>\n");
            sb.Append("             <th>IP</th>\n");
            sb.Append("             <th>Champions</th>\n");
            sb.Append("             <th>Skins</th>\n");
            sb.Append("             <th>Rune Pages</th>\n");
            sb.Append("             <th>Rank</th>\n");
            sb.Append("             <th>Last Play</th>\n");
            sb.Append("         </tr>\n");
            sb.Append("     </thead>\n")
            sb.Append("     <tbody>\n")

            var i = 0;
            foreach (var account in accounts.OrderByDescending(a => a.Result == Client.Result.Success))
            {
                if (account.Result == Client.Result.Error && !exportErrors)
                {
                    continue;
                }

                var c = i % 2 == 0 ? "a" : "b";

                sb.Append("         <tr class=" + c + ">\n");
                sb.Append("             <td>" + account.Username + "</td>\n");
                sb.Append("             <td>" + account.Password + "</td>\n");

                if (account.Result == Client.Result.Error)
                {
                    sb.Append("             <td>" + account.ErrorMessage + "</td>\n");
                    sb.Append("         </tr>\n");
                    i++;
                    continue;
                }

                sb.Append("             <td>" + account.SummonerName + "</td>\n");
                sb.Append("             <td>" + account.Level + "</td>\n");
                sb.Append("             <td>" + account.EmailStatus + "</td>\n");
                sb.Append("             <td>" + account.RpBalance + "</td>\n");
                sb.Append("             <td>" + account.Ipbalance + "</td>\n");
                sb.Append("             <td>" + account.Champions + "</td>\n");
                sb.Append("             <td>" + account.Skins + "</td>\n");
                sb.Append("             <td>" + account.RunePages + "</td>\n");
                sb.Append("             <td>" + account.SoloQRank + "</td>\n");
                sb.Append("             <td>" + account.LastPlay + "</td>\n");
                sb.Append("         </tr>\n");

                i++;
            }
            sb.Append("     </tbody>\n")
            sb.Append("     </table>\n");
            sb.Append(" </body>");

            var sw = new StreamWriter(file);
            sw.Write(sb.ToString());
            sw.Close();
        }

        public static void ExportAsText(string file, List<AccountData> accounts, bool exportErrors)
        {
            var sb = new StringBuilder();

            var hr = "---------------------------" + Environment.NewLine;

            sb.Append(hr);

            foreach (var account in accounts.OrderByDescending(a => a.Result == Client.Result.Success))
            {
                sb.Append("Username: " + account.Username + Environment.NewLine);
                sb.Append("Password: " + account.Password + Environment.NewLine);

                if (account.Result == Client.Result.Error)
                {
                    if (!exportErrors)
                    {
                        continue;
                    }

                    sb.Append("Error: " + account.ErrorMessage + Environment.NewLine);
                    sb.Append(hr);
                    continue;
                }

                sb.Append("Summoner Name: " + account.SummonerName + Environment.NewLine);
                sb.Append("Level: " + account.Level + Environment.NewLine);
                sb.Append("RP: " + account.RpBalance + Environment.NewLine);
                sb.Append("IP: " + account.Ipbalance + Environment.NewLine);
                sb.Append("Champions: " + account.Champions + Environment.NewLine);
                sb.Append("Skins: " + account.Skins + Environment.NewLine);
                sb.Append("Rune Pages: " + account.RunePages + Environment.NewLine);
                sb.Append("Email Status: " + account.EmailStatus + Environment.NewLine);
                sb.Append("Rank: " + account.SoloQRank + Environment.NewLine);
                sb.Append("Last Play: " + account.LastPlay + Environment.NewLine);
                sb.Append(hr);
            }

            var sw = new StreamWriter(file);
            sw.WriteLine(sb.ToString());
            sw.Close();
        }
    }
}
