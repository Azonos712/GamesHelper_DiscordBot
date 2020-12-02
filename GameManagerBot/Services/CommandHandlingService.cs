using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace GameManagerBot.Services
{
    class CommandHandlingService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private IServiceProvider _services;

        public CommandHandlingService(IServiceProvider services)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>(); ;
            _commands = services.GetRequiredService<CommandService>();
            _services = services;

            _commands.CommandExecuted += CommandExecutedAsync;
            _discord.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ignore system messages and messages from bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;
            if (message.Author.IsBot) return;

            // This value holds the offset where the prefix ends
            var argPos = 0;
            // Perform prefix check. You may want to replace this with
            if (!message.HasCharPrefix('!', ref argPos)) return;
            // for a more traditional command format like !help.
            //if (!message.HasMentionPrefix(_discord.CurrentUser, ref argPos)) return;

            var context = new SocketCommandContext(_discord, message);
            // Perform the execution of the command. In this method,
            // the command service will perform precondition and parsing check
            // then execute the command if one is matched.
            await _commands.ExecuteAsync(context, argPos, _services);
            // Note that normally a result will be returned by this format, but here
            // we will handle the result in CommandExecutedAsync,
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // command is unspecified when there was a search failure (command not found); we don't care about these errors
            if (!command.IsSpecified)
                return;

            // the command was successful, we don't care about this result, unless we want to log that a command succeeded.
            if (result.IsSuccess)
                return;

            // the command failed, let's notify the user that something happened.
            await context.Channel.SendMessageAsync($"Ошибка: {result}");
        }
    }
}
