using Discord;
using Discord.Commands;
using MyAwesomeDiscordBot.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyAwesomeDiscordBot
{
    public class DiscordBot
    {
        DiscordClient client;
        CommandService commands;
        CommandEventArgs adminPanelArgs;
        Form AdminPanel;

        public DiscordBot()
        {

            client = new DiscordClient(input =>
            {
                input.LogLevel = LogSeverity.Info;
                input.LogHandler = Log;
            });

            client.UsingCommands(input =>
            {
                input.PrefixChar = '!';
                input.AllowMentionPrefix = true;
            });

            commands = client.GetService<CommandService>();

            commands.CreateCommand("Hello").Do(async (e) =>
            {
                await e.Channel.SendMessage("World!");
            });

            commands.CreateCommand("announce").Parameter("message", ParameterType.Multiple).Do(async (e) =>
            {
                await DoAnnouncement(e);
            });

            commands.CreateCommand("award").Parameter("name", ParameterType.Multiple).Do(async (e) =>
            {
                await AwardUserTokens(e);
            });

            commands.CreateCommand("adminpanel").Do((e) =>
            {
                AdminPanel = new AdminPanel(client, e);

                var thread = new Thread(OpenAdminPanel);

                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();

            });

            commands.CreateCommand("mod").Parameter("name", ParameterType.Required).Do(async (e) =>
            {
                var username = e.Args[0];

                var user = e.Channel.Users.Where(input => input.Name == username).FirstOrDefault();

                var roles = e.Server.Roles.Where(input => input.Name == "Moderator").FirstOrDefault();

                await user.AddRoles(roles);
            });

            commands.CreateCommand("quote").Parameter("name", ParameterType.Multiple).Do(async (e) =>
            {
                await DoQuoteCommmand(e);
            });


            client.UserJoined += async (s, e) =>
            {
                var channel = e.Server.FindChannels("general", ChannelType.Text).FirstOrDefault();

                var user = e.User;

                await channel.SendTTSMessage(string.Format("{0} has joined the channel!", user.Name));
            };

            client.UserLeft += async (s, e) =>
            {
                var channel = e.Server.FindChannels("general", ChannelType.Text).FirstOrDefault();

                var user = e.User;

                await channel.SendTTSMessage(string.Format("{0} has left the channel!", user.Name));
            };

            client.ExecuteAndWait(async () =>
            {
                await client.Connect("MjQ3MzY3Mzg1ODYwODAwNTEy.CwoKWA.UC9xT2O6Ft6OFjYSbDPWXB5VtXM", TokenType.Bot);
            });
        }

        private async Task DoQuoteCommmand(CommandEventArgs e)
        {

            if (!File.Exists("quotes.txt"))
            {
                File.Create("quotes.txt");
            }

            var name = e.Args[0];

            if (e.Args[1].All(char.IsDigit))
            {

                var line = File.ReadAllLines("quotes.txt").Where(input => input.StartsWith(name) && input.Substring(name.Length + 1) == e.Args[1]).FirstOrDefault();

                if (line != null)
                {
                    await e.Channel.SendMessage(line);
                }

                else
                {

                    await e.Channel.SendMessage(string.Format("Could not find quote {0} with the name of {1}", e.Args[1], name));
                }
            }
            else
            {
                var line = "";

                var linesForCurrentUser = File.ReadAllLines("quotes.txt").Where(input => input.StartsWith(name));

                var highestCount = 0;

                foreach (var lineForUser in linesForCurrentUser)
                {
                    if (Convert.ToInt32(line.Substring(name.Length, line.IndexOf(' '))) > highestCount)
                    {
                        highestCount = Convert.ToInt32(line.Substring(name.Length, line.IndexOf(' ')));
                    }
                }

                highestCount++;

                for (int i = 0; i < e.Args.Length; i++)
                {
                    if (i == 1)
                    {

                    }

                    line += e.Args[i] + " ";
                }

                var file = new StreamWriter("quotes.txt");

                file.WriteLine(line);

                file.Close();
            }
        }


        private void OpenAdminPanel()
        {
            Application.Run(AdminPanel);
        }

        private async Task AwardUserTokens(CommandEventArgs e)
        {
            var username = e.Args[0];

            var user = e.Server.FindUsers(username).FirstOrDefault();

            var userRoles = e.User.Roles;

            if (userRoles.Any(input => input.Name.ToUpper() == "MODERATOR"))
            {
                if (user != null)
                {
                    var tokenAmount = e.Args[1];

                    try
                    {
                        TokenService.AwardTokens(user, tokenAmount);

                        await e.Channel.SendMessage(string.Format("{0} has been awarded with {1} tokens", user.Name, tokenAmount));
                    }
                    catch (Exception ex)
                    {
                        await e.Channel.SendMessage("Error occured, please contact developer");
                    }
                }
                else
                {
                    await e.Channel.SendMessage(string.Format("Could not find user with username {0}!", username));
                }
            }
            else
            {
                await e.User.SendMessage("You do not have sufficient permissions for this command!");
            }

        }

        private async Task DoAnnouncement(CommandEventArgs e)
        {
            var channel = e.Server.FindChannels(e.Args[0], ChannelType.Text).FirstOrDefault();

            var message = ConstructMessage(e, channel != null);

            if (channel != null)
            {
                await channel.SendMessage(message);
            }
            else
            {
                await e.Channel.SendMessage(message);
            }
        }

        private string ConstructMessage(CommandEventArgs e, bool firstArgIsChannel)
        {
            string message = "";

            var name = e.User.Nickname != null ? e.User.Nickname : e.User.Name;

            var startIndex = firstArgIsChannel ? 1 : 0;

            for (int i = startIndex; i < e.Args.Length; i++)
            {
                message += e.Args[i].ToString() + " ";
            }

            var result = name + " says: " + message;

            return result;
        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
