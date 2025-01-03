using dotenv.net;
using OpenAI.Chat;

DotEnv.Load();

const string EXIT_COMMAND = "/bye";
const int TOKEN_LIMIT = 128000;

const string SYSTEM_PROMPT = @"Você é um assistente prestativo, criativo, inteligente e muito insistente.
    Seu papel é vender um plano de internet para um cliente.
    Use todos os possíveis argumentos para convercer o cliente a comprar o plano.
    Não deixe que ele termine a conversa sem você atingir seu objetivo.
    Se ele se negar a comprar, ofereça melhores condições e vantagens mas nunca desista.";

var apiKey = Environment.GetEnvironmentVariable("GROQ_API_KEY") ?? "";
var modelName = Environment.GetEnvironmentVariable("MODEL_NAME") ?? "";
var apiEndpoint = Environment.GetEnvironmentVariable("API_ENDPOINT") ?? "";

if (string.IsNullOrEmpty(apiKey))
{
    Console.WriteLine("API key is missing. Please set the GROQ_API_KEY environment variable.");
    return;
}

ChatClient client = new(
    model: modelName,
    credential: new System.ClientModel.ApiKeyCredential(apiKey),
    options: new OpenAI.OpenAIClientOptions
    {
        Endpoint = new Uri(apiEndpoint)
    });

Console.WriteLine("Welcome to the chat!");
Console.WriteLine($"Type {EXIT_COMMAND} to exit.");

var messageHistory = new List<ChatMessage>
{
    new SystemChatMessage(SYSTEM_PROMPT)
};

while (true)
{
    var prompt = ReadPrompt();
    if (string.Compare(prompt, EXIT_COMMAND, StringComparison.OrdinalIgnoreCase) == 0)
        break;

    messageHistory.Add(new UserChatMessage(prompt));
    TrimMessageHistory(messageHistory, TOKEN_LIMIT);
    var reply = await SendMessageAsync(client, messageHistory);
    Console.WriteLine($"[ASSISTANT]: {reply}");
    messageHistory.Add(new AssistantChatMessage(reply));
}

string ReadPrompt()
{
    Console.Write("> ");
    return Console.ReadLine() ?? string.Empty;
}

async Task<string> SendMessageAsync(ChatClient client, List<ChatMessage> messageHistory)
{
    try
    {
        ChatCompletion completion = await client.CompleteChatAsync(messageHistory);
        return completion.Content[0].Text;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        return string.Empty;
    }
}

void TrimMessageHistory(List<ChatMessage> messageHistory, int tokenLimit)
{
    int tokenCount = 0;
    for (int i = messageHistory.Count - 1; i > 0; i--) // Start from the end but skip the first message
    {
        tokenCount += CountTokens(messageHistory[i].Content[0].Text);
        if (tokenCount > tokenLimit)
        {
            messageHistory.RemoveAt(i);
        }
    }
}

int CountTokens(string content)
{
    // Simple token count approximation: 1 token per 4 characters
    return content.Length / 4;
}
