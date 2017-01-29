using Discord;
using Discord.Commands;
using MyAwesomeDiscordBot.Entities;
using System;
using System.Linq;

namespace MyAwesomeDiscordBot.Services
{
    public class TokenService
    {
        public static void AwardTokens(User user, string tokenAmount)
        {
            using(var context = new DiscordBotEntities())
            {
                var userIdAsString = user.Id.ToString();

                int tokens;

                var success = Int32.TryParse(tokenAmount, out tokens);

                if (!success)
                {
                    throw new ApplicationException("Could not parse tokenamount to int");
                }

                var entity = context.tokens.Where(input => input.user_id == userIdAsString).FirstOrDefault();

                if(entity != null)
                {
                    entity.tokens1 += tokens;
                }
                else
                {
                    context.tokens.Add(new tokens
                    {
                        tokens1 = tokens,
                        user_id = userIdAsString,
                        username = user.Name
                    });
                }

                context.SaveChanges();
            }
        }

    }
}
