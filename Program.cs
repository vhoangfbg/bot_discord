using System;
using System.Threading.Tasks;
using DotNetEnv;
using Discord;
using Discord.WebSocket;

class Program
{
    private static DiscordSocketClient? _client;
    private static string TOKEN;
    static Program(){
    Env.Load();
    TOKEN = Env.GetString("Token") ?? "";
    }
    static void Main() => new Program().RunBotAsync().GetAwaiter().GetResult();

    public async Task RunBotAsync()
    {
        _client = new DiscordSocketClient(
            new DiscordSocketConfig{
            GatewayIntents = GatewayIntents.GuildMessages | GatewayIntents.MessageContent | GatewayIntents.Guilds
            }
        );
        _client.Log += Log;
        _client.MessageReceived += MessageReceived;

        await _client.LoginAsync(TokenType.Bot, TOKEN);
        await _client.StartAsync();
        await Task.Delay(-1);
    }

    private async Task MessageReceived(SocketMessage message)
{
    if (message.Author.IsBot) return;

    // Cú pháp: !spam [số lần] [tin nhắn]
    if (message.Content.StartsWith("!spam "))
    {
        var parts = message.Content.Split(' ', 3);
        if (parts.Length < 3 || !int.TryParse(parts[1], out int repeat) || repeat <= 0)
        {
            await message.Channel.SendMessageAsync("Sai cú pháp! Dùng: `!spam <số lần> <nội dung>`");
            return;
        }

        string content = parts[2];
        for (int i = 1; i <= repeat; i++)
        {
            await message.Channel.SendMessageAsync($"{content} ({i}/{repeat})");
            await Task.Delay(500); // Chống spam quá nhanh
        }
    }
}


    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}

