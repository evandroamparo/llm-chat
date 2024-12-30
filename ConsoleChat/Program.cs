using dotenv.net;
using OpenAI.Chat;

DotEnv.Load();

var apiKey = Environment.GetEnvironmentVariable("GROQ_API_KEY") ?? "";

ChatClient client = new(
    model: "llama-3.3-70b-versatile", 
    credential: new System.ClientModel.ApiKeyCredential(apiKey),
    options: new OpenAI.OpenAIClientOptions
    {
        Endpoint = new Uri("https://api.groq.com/openai/v1")
    });

ChatCompletion completion = client.CompleteChat("tell me a joke");

Console.WriteLine($"[ASSISTANT]: {completion.Content[0].Text}");