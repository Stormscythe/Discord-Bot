using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyAwesomeDiscordBot
{
    public partial class AdminPanel : Form
    {
        private CommandEventArgs e;
        private DiscordClient client;

        public AdminPanel(DiscordClient client, CommandEventArgs e)
        {
            this.client = client;
            this.e = e;

            InitializeComponent();
        }

        private void kickButton_Click(object sender, EventArgs e)
        {
            var usernameToKick = kickTextBox.Text.ToUpper();

            var userToKick = this.e.Channel.Users.Where(input => input.Name.ToUpper() == usernameToKick).FirstOrDefault();

            userToKick.Kick();
        }

        private void kickTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
