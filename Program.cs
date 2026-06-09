using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();
var openAiKey = config["OpenAi:ApiKey"];

var agent = new OpenAIClient(openAiKey)
    .GetChatClient("gpt-5.4-mini")
    .AsAIAgent("""
               You are a C# and .NET instructor at the argus platform.
               Your feedback is technical, precise, didactic, and encouraging.
               ALWAYS respond in Brazilian Portuguese.
               """);

await foreach (var token in agent.RunStreamingAsync("https://github.com/folke/tokyonight.nvim"))
{
    Console.Write(token);
}