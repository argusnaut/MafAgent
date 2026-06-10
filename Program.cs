using System.Text;
using MafAgent.Tools;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();
var openAiKey = config["OpenAi:ApiKey"];

var instructorAgent = new OpenAIClient(openAiKey)
    .GetChatClient("gpt-5.4-mini")
    .AsAIAgent("""
               You are a C# and .NET instructor at the argus platform.
               Your feedback is technical, precise, didactic, and encouraging.
               ALWAYS respond in Brazilian Portuguese.
               """, tools:
    [
        AIFunctionFactory.Create(WeatherTool.GetWeather)
    ]);

var translationAgent = new OpenAIClient(openAiKey)
    .GetChatClient("gpt-5.4-mini")
    .AsAIAgent("""
               Você é um agente especialista em traduzir textos de português para inglês.
               Sempre que lhe passarem um texto, traduza para o inglês.
               """);

var session = await instructorAgent.CreateSessionAsync();

while (true)
{
    Console.WriteLine("Faça uma pergunta:");
    var prompt = Console.ReadLine();

    if (string.IsNullOrEmpty(prompt)) continue;

    var fullResponse = new StringBuilder();

    await foreach (var token in instructorAgent.RunStreamingAsync(prompt, session))
    {
        Console.Write(token);
        fullResponse.Append(token);
    }

    var responseText = fullResponse.ToString().Trim();

    Console.WriteLine();
    Console.WriteLine("---");

    if (!string.IsNullOrEmpty(responseText))
    {
        await foreach (var translatedToken in translationAgent.RunStreamingAsync(responseText))
        {
            Console.Write(translatedToken);
        }

        Console.WriteLine();
        Console.WriteLine("---");
    }

    Console.WriteLine();
}