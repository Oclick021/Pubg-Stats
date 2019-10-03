using Discord;
using Pubg.Net;
using PubgSDK.Models;
using PubgSDK.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PubgStatsBot.Helpers
{
    public static class EmbedHelper
    {
        public static Embed GetStats(string statsType, SeasonStats stats, Color color)
        {
            var embedBuilder = new EmbedBuilder
            {
                Title = statsType,
                Color = color,
                Url = "https://discord.gg/3FsMG3"
            };

            embedBuilder.AddField(new EmbedFieldBuilder() { Name = Strings.Title, Value = SeasonStats.GetTitles(), IsInline = true });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = Strings.Title, Value = stats.GetListValue(), IsInline = true });

            embedBuilder.WithFooter(new EmbedFooterBuilder() { Text = Strings.Thanks, IconUrl = "http://icons.iconarchive.com/icons/graphicloads/100-flat/256/home-icon.png" });
            return embedBuilder.Build();
        }
        public static Embed GetMatches(string statsType, Player player, Color color)
        {
            var embedBuilder = new EmbedBuilder
            {
                Title = statsType,
                Color = color,
                Url = "https://discord.gg/3FsMG3"
            };
            var playerRep = new PlayerRepository(player);
            playerRep.LoadMatches().Wait();
            var matchList = player.Matches.Select(x => x.Match).OrderByDescending(o => o.CreatedAt).Take(10).ToArray();
            for (int i = 0; i < matchList.Length; i += 2)
            {

                embedBuilder.AddField(new EmbedFieldBuilder()
                {
                    Name = Strings.Title,
                    Value = Match.GetTitles(),
                    IsInline = true
                });

                embedBuilder.AddField(new EmbedFieldBuilder()
                {
                    Name = $"{matchList[i].GameMode} | {matchList[i].GetMapName()} | {matchList[i].GetParticipantsMatchStats(player.Name).WinPlace} {GetMVPStar(matchList[i], player.Name)}",
                    Value = matchList[i].GetParticipantsMatchStatsString(player.Name),
                    IsInline = true
                });

                if (i + 1 <= matchList.Length)
                {
                    embedBuilder.AddField(new EmbedFieldBuilder()
                    {
                        Name = $"{matchList[i + 1].GameMode} | {matchList[i + 1].GetMapName()} | {matchList[i + 1].GetParticipantsMatchStats(player.Name).WinPlace} {GetMVPStar(matchList[i], player.Name)}",
                        Value = matchList[i + 1].GetParticipantsMatchStatsString(player.Name),
                        IsInline = true
                    });
                }

            }

            embedBuilder.WithFooter(new EmbedFooterBuilder()
            {
                Text = Strings.Thanks,
                IconUrl = "http://icons.iconarchive.com/icons/graphicloads/100-flat/256/home-icon.png"
            });
            return embedBuilder.Build();
        }

        public static Embed GetCompare(string StatsType, IEnumerable<Player> players, Color color)
        {
            var embedBuilder = new EmbedBuilder
            {
                Title = StatsType,
                Color = color,
                Url = "https://discord.gg/3FsMG3"
            };

            //Solos
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "Solos", Value = SeasonStats.GetTitles(), IsInline = true });
            foreach (var player in players)
            {
                embedBuilder.AddField(new EmbedFieldBuilder() { Name = player.Name, Value = player.SoloStats.GetListValue(), IsInline = true });
            }

            //Dues
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "Duos", Value = SeasonStats.GetTitles(), IsInline = true });
            foreach (var player in players)
            {
                embedBuilder.AddField(new EmbedFieldBuilder() { Name = player.Name, Value = player.DuoStats.GetListValue(), IsInline = true });
            }

            //Squad
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "Squads", Value = SeasonStats.GetTitles(), IsInline = true });
            foreach (var player in players)
            {
                embedBuilder.AddField(new EmbedFieldBuilder() { Name = player.Name, Value = player.SquadStats.GetListValue(), IsInline = true });
            }

            embedBuilder.WithFooter(new EmbedFooterBuilder() { Text = "با تشکر", IconUrl = "http://icons.iconarchive.com/icons/graphicloads/100-flat/256/home-icon.png" });
            return embedBuilder.Build();
        }
        public static Embed GetHelp()
        {
            var embedBuilder = new EmbedBuilder
            {
                Title = Strings.Help,
                Color = new Color(255, 0, 0),
                Description = Strings.HelpDescription,
                ThumbnailUrl = "https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/f4/f41f5b7bfcbb7f4b6fc6a79fdc978638222ffc2f_full.jpg",
                Url = "https://discord.gg/3FsMG3"
            };

            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "!help", Value = Strings.Help });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "!invite", Value = Strings.InviteDesc });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "!stats [Your pubg username]", Value = Strings.PlayersStats });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "!solo [Your pubg username]", Value = Strings.SoloStats });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "!duo [Your pubg username]", Value = Strings.DuoStats });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "!squad [Your pubg username] ", Value = Strings.SquadStats });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "!compare [Player1],[Player2]", Value = Strings.Compare });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "!recents [Your pubg username]", Value = Strings.RecentsStats });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "Watch service", Value = Strings.Watch });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "!addWatch  [Your pubg username] ", Value = Strings.AddWatchDesc });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "!myWatch ", Value = Strings.MyWatchDesc });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "!removeWatch [Your pubg username]", Value = Strings.RemoveWatch });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "⭐", Value = Strings.TeamMVPIconDesc });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "✨", Value = Strings.MatchMVPIconDesc });

            embedBuilder.WithFooter(new EmbedFooterBuilder() { Text = "با تشکر", IconUrl = "http://icons.iconarchive.com/icons/graphicloads/100-flat/256/home-icon.png" });
            return embedBuilder.Build();
        }


        public static Embed GetParticipantsStats(IEnumerable<Participant> participants, Match match)
        {
            var embedBuilder = new EmbedBuilder
            {
                Url = "https://discord.gg/3FsMG3"
            };
            bool isFirst = true;
            var participantsArray = participants.OrderByDescending(m => m.Stats.DamageDealt).ToArray();
            for (int i = 0; i < participantsArray.Length; i += 2)
            {
                int winplace = match.GetParticipantsMatchStats(participantsArray[i].Stats.Name).WinPlace;

                if (winplace == 1)
                {
                    embedBuilder.Color = Color.Gold;
                }
                else
                {
                    embedBuilder.Color = Color.Blue;
                }


                embedBuilder.AddField(new EmbedFieldBuilder() { Name = $"{match.GameMode} | {match.GetMapName()} | {winplace} ", Value = Match.GetTitles(), IsInline = true });
                if (isFirst && participants.Count() > 1)
                {
                    string star = "⭐";
                    if (match.GetMatchMVP() == participantsArray[i])
                    {
                        star += GetMVPStar(match, participantsArray[i].Stats.Name);
                    }
                    isFirst = false;
                    embedBuilder.AddField(new EmbedFieldBuilder() { Name = $"{participantsArray[i].Stats.Name} {star}", Value = match.GetParticipantsMatchStatsString(participantsArray[i].Stats.Name), IsInline = true });
                }
                else
                {
                    embedBuilder.AddField(new EmbedFieldBuilder() { Name = $"{participantsArray[i].Stats.Name}", Value = match.GetParticipantsMatchStatsString(participantsArray[i].Stats.Name), IsInline = true });
                }
                if (i + 1 <= participantsArray.Length - 1)
                {
                    embedBuilder.AddField(new EmbedFieldBuilder() { Name = $"{participantsArray[i + 1].Stats.Name}", Value = match.GetParticipantsMatchStatsString(participantsArray[i + 1].Stats.Name), IsInline = true });
                }

            }
            if (participantsArray.Length == 3)
            {
                embedBuilder.AddField(new EmbedFieldBuilder() { Name = "3 man squad", Value = "Who needs 4 players?", IsInline = true }); ;
            }
            return embedBuilder.Build();
        }

        static string GetMVPStar(Match match, string playerName)
        {
            if (match.GetMatchMVP().Stats.Name == playerName)
            {
                return "✨";
            }
            return "";
        }

        //public static Embed GetWatchMatch(List<Player> players, string matchID)
        //{





        //    //var embedBuilder = new EmbedBuilder
        //    //{
        //    //    Color = color,
        //    //    Url = "https://discord.gg/3FsMG3"
        //    //};
        //    //player.Matches

        //    //var matchList = player.Matches.Select(x => x.Match).OrderByDescending(o => o.CreatedAt).Take(10).ToArray();
        //    //for (int i = 0; i < matchList.Length; i += 2)
        //    //{
        //    //    embedBuilder.AddField(new EmbedFieldBuilder() { Name = Strings.Title, Value = Match.GetTitles(), IsInline = true });
        //    //    embedBuilder.AddField(new EmbedFieldBuilder() { Name = $"{matchList[i].GameMode} | {matchList[i].GetMapName()} | {matchList[i].GetParticipantsMatchStats(player.Name).WinPlace} ", Value = matchList[i].GetParticipantsMatchStatsString(player.Name), IsInline = true });
        //    //    if (i + 1 <= matchList.Length)
        //    //    {
        //    //        embedBuilder.AddField(new EmbedFieldBuilder() { Name = $"{matchList[i + 1].GameMode} | {matchList[i + 1].GetMapName()} | {matchList[i + 1].GetParticipantsMatchStats(player.Name).WinPlace} ", Value = matchList[i + 1].GetParticipantsMatchStatsString(player.Name), IsInline = true });
        //    //    }

        //    //}


        //    //embedBuilder.WithFooter(new EmbedFooterBuilder() { Text = Strings.Thanks, IconUrl = "http://icons.iconarchive.com/icons/graphicloads/100-flat/256/home-icon.png" });
        //    //return embedBuilder.Build();
        //}

    }
}
