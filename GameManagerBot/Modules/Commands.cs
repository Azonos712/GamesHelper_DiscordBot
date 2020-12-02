using Discord;
using Discord.Commands;
using Discord.WebSocket;
using GameManagerBot.Models;
using GameManagerBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameManagerBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("Pong");
        }

        [Command("mafia")]
        public async Task Mafia(string lead, string roles = "")
        {
            try
            {
                SocketGuildChannel voiceChannel = Context.Guild?.Channels.First(x => x.Name == "Основной");

                if (voiceChannel == null)
                    throw new Exception("Ошибка. Голосовой канал не обнаружен!");

                Mafia game = new Mafia(roles, voiceChannel.Users.Count);

                var leader = voiceChannel.Users.Single(s => s.Username == lead);

                var result = game.Start();

                SendToAllUsers(voiceChannel, result, leader);

                await ReplyAsync($"Бот распределил роли.\nКоличество человек в голосовом канале: {voiceChannel.Users.Count}." +
                            $"\nВедущий - {lead}!" +
                            $"\nУдачи и веселой игры!");

                return;
            }
            catch (Exception e)
            {
                await ReplyAsync($"Что-то пошло не по плану! {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
        }

        [Command("spy")]
        public async Task Spy()
        {
            try
            {
                SocketGuildChannel voiceChannel = Context.Guild?.Channels.First(x => x.Name == "Основной");

                if (voiceChannel == null)
                    throw new Exception("Ошибка. Голосовой канал не обнаружен!");

                Spy game = new Spy(voiceChannel.Users.Count);

                var result = game.Start();

                SendToAllUsers(voiceChannel, result);

                await ReplyAsync($"Бот вычислил шпиона.\nКоличество человек в голосовом канале: {voiceChannel.Users.Count}." +
                            $"\nУдачи и веселой игры!");

                return;

            }
            catch (Exception e)
            {
                await ReplyAsync($"Что-то пошло не по плану! {e.Message}");
                Console.WriteLine(e.StackTrace);
            }
        }

        private void SendToAllUsers(SocketGuildChannel channel, List<string> data, SocketGuildUser leader = null)
        {
            if ((leader == null && channel.Users.Count != data.Count) || (leader != null && channel.Users.Count - 1 != data.Count))
                throw new Exception("Отмена операции. Количество ролей не совпадало с количеством пользователей!");

            string strUsersRoles = string.Empty;
            int index = 0;
            foreach (var user in channel.Users)
            {
                if (leader == user)
                    continue;

                user.SendMessageAsync($"{data[index]}\nПриятной игры!");
                strUsersRoles += user.Username + " - " + data[index] + "\n";
                index++;
            }

            if (leader != null)
                leader.SendMessageAsync($"{strUsersRoles}\nПриятной игры!");

            FileService.SaveData(strUsersRoles);
        }
    }
}
