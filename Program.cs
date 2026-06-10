using MafAgent.Tools;
using Microsoft.Extensions.AI;
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
               """, tools:
    [
        AIFunctionFactory.Create(WeatherTool.GetWeather)
    ]);

var session = await agent.CreateSessionAsync();

while (true)
{
    Console.WriteLine("Faça uma pergunta:");
    var prompt = Console.ReadLine();

    await foreach (var token in agent.RunStreamingAsync(prompt ?? string.Empty, session))
    {
        Console.Write(token);
    }

    Console.WriteLine();
    Console.WriteLine("---");
    Console.WriteLine();
}