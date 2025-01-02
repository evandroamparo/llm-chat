using dotenv.net;
using OpenAI.Chat;

DotEnv.Load();

const string EXIT_COMMAND = "/bye";

var apiKey = Environment.GetEnvironmentVariable("GROQ_API_KEY") ?? "";

ChatClient client = new(
    model: "llama-3.3-70b-versatile",
    credential: new System.ClientModel.ApiKeyCredential(apiKey),
    options: new OpenAI.OpenAIClientOptions
    {
        Endpoint = new Uri("https://api.groq.com/openai/v1")
    });

var exit = false;
do
{
    Console.Write("> ");
    var prompt = Console.ReadLine();
    exit = string.Compare(prompt, EXIT_COMMAND, StringComparison.OrdinalIgnoreCase) == 0;

    if (exit)
        continue;

    ChatCompletion completion = await client.CompleteChatAsync(prompt);

    Console.WriteLine($"[ASSISTANT]: {completion.Content[0].Text}");
} while (!exit);

