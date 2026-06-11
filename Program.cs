using System.Text.Json;
using System.Text.Json.Serialization;
using MafAgent.Tools;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();
var openAiKey = config["OpenAI:ApiKey"];

var instructorAgent = new OpenAIClient(openAiKey)
    .GetChatClient("gpt-5.4-mini")
    .AsAIAgent(new ChatClientAgentOptions
    {
        ChatOptions = new ChatOptions
        {
            Instructions = """
                           You are a C# and .NET instructor at the argus platform.
                           Your feedback is technical, precise, didactic, and encouraging.
                           ALWAYS respond in Brazilian Portuguese.
                           """,
            Tools =
            [
                AIFunctionFactory.Create(WeatherTool.GetWeather)
            ]
        },
        Name = "InstructorAgent",
        ChatHistoryProvider = new LocalFileChatHistoryProvider()
    });

var session = await instructorAgent.CreateSessionAsync();

while (true)
{
    Console.WriteLine("Faça uma pergunta:");
    var prompt = Console.ReadLine();

    if (string.IsNullOrEmpty(prompt)) continue;

    await foreach (var token in instructorAgent.RunStreamingAsync(prompt, session))
    {
        Console.Write(token);
    }

    Console.WriteLine();
    Console.WriteLine("---");
    Console.WriteLine();
}

public class LocalFileChatHistoryProvider : ChatHistoryProvider
{
    private readonly ProviderSessionState<State> _sessionState;
    private readonly string _path;

    public LocalFileChatHistoryProvider(string path = "maf-history.json")
    {
        _path = path;
        _sessionState = new ProviderSessionState<State>(
            stateInitializer: _ => LoadFromFile(),
            stateKey: GetType().Name
        );
    }

    protected override ValueTask<IEnumerable<ChatMessage>> ProvideChatHistoryAsync(InvokingContext context,
        CancellationToken cancellationToken = new CancellationToken())
        => new(_sessionState.GetOrInitializeState(context.Session).Messages);

    protected override ValueTask StoreChatHistoryAsync(InvokedContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var state = _sessionState.GetOrInitializeState(context.Session);

        var allNewMessages = context.RequestMessages.Concat(context.ResponseMessages ?? []);
        state.Messages.AddRange(allNewMessages);

        _sessionState.SaveState(context.Session, state);

        SaveToFile(state);

        return default;
    }

    private State LoadFromFile()
    {
        if (!File.Exists(_path))
            return new State();

        var json = File.ReadAllText(_path);
        return JsonSerializer.Deserialize<State>(json) ?? new State();
    }

    private void SaveToFile(State state)
    {
        var json = JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_path, json);
    }
}

public sealed class State
{
    [JsonPropertyName("messages")] public List<ChatMessage> Messages { get; set; } = [];
}