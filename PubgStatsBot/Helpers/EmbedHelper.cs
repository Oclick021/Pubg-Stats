using Discord;
using Pubg.Net;
using PubgSDK.Models;
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

            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "Title", Value = SeasonStats.GetTitles(), IsInline = true });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "Title", Value = stats.GetListValue(), IsInline = true });

            embedBuilder.WithFooter(new EmbedFooterBuilder() { Text = "با تشکر", IconUrl = "http://icons.iconarchive.com/icons/graphicloads/100-flat/256/home-icon.png" });
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

            var matchList = player.Matches.Select(x => x.Match).OrderByDescending(o => o.CreatedAt).Take(6).ToArray();
            for (int i = 0; i < matchList.Length; i += 2)
            {
                embedBuilder.AddField(new EmbedFieldBuilder() { Name = "Title", Value = Match.GetTitles(), IsInline = true });
                embedBuilder.AddField(new EmbedFieldBuilder() { Name = $"{matchList[i].GameMode} | {matchList[i].MapName} | {matchList[i].GetParticipantsMatchStats(player.Name).WinPlace} ", Value = matchList[i].GetParticipantsMatchStats(player.Name), IsInline = true });
                if (i + 1 <= matchList.Length)
                {
                    embedBuilder.AddField(new EmbedFieldBuilder() { Name = $"{matchList[i + 1].GameMode} | {matchList[i + 1].MapName} | {matchList[i + 1].GetParticipantsMatchStats(player.Name).WinPlace} ", Value = matchList[i + 1].GetParticipantsMatchStats(player.Name), IsInline = true });
                }

            }


            embedBuilder.WithFooter(new EmbedFooterBuilder() { Text = "با تشکر", IconUrl = "http://icons.iconarchive.com/icons/graphicloads/100-flat/256/home-icon.png" });
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
                Title = "راهنما",
                Color = new Color(255, 0, 0),
                Description = "جهت استفاده از این بات میتوانید از دستورهای زیر استفاده کنید",
                ThumbnailUrl = "https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/f4/f41f5b7bfcbb7f4b6fc6a79fdc978638222ffc2f_full.jpg",
                Url = "https://discord.gg/3FsMG3"
            };

            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "!help", Value = "درخواست راهنما" });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "!stats [Your pubg username]", Value = "درخواست آمار بازیکن" });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "!compare [Player1],[Player2]", Value = "مقایسه آمار 2 بازیکن" });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "!recents [Your pubg username] | disabled", Value = "درخواست مشخصات 10 بازی اخیر بازیکن" });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "!addWatch [Player1],[Player2],[Player3],[Player4] | disabled", Value = "درخواست گذارش ترکیبی نوشتاری" });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "!myWatches | disabled", Value = "نمایش گذارش های ثبت شده" });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "!removeWatch [Watch ID] | disabled", Value = "درخواست گذارش ترکیبی نوشتاری" });
            embedBuilder.AddField(new EmbedFieldBuilder() { Name = "!lang EN|FA | disabled", Value = "تغییر زبان" });

            embedBuilder.WithFooter(new EmbedFooterBuilder() { Text = "با تشکر", IconUrl = "http://icons.iconarchive.com/icons/graphicloads/100-flat/256/home-icon.png" });
            return embedBuilder.Build();
        }


    }
}
