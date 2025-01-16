using System.ClientModel;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var apiKey = config["GroqApiKey"] ?? "";
var modelName = config["ModelName"];
var baseUrl = config["BaseUrl"] ?? "";

const string SYSTEM_PROMPT =
    """
    Você é um assistente prestativo, criativo, inteligente e muito insistente.
        Seu papel é vender um plano de internet para um cliente.
        Use todos os possíveis argumentos para convercer o cliente a comprar o plano.
        Não deixe que ele termine a conversa sem você atingir seu objetivo.
        Se ele se negar a comprar, ofereça melhores condições e vantagens mas nunca desista.
    """;

var chatClient = new ChatClient(
    modelName,
    new ApiKeyCredential(apiKey),
    new OpenAIClientOptions { Endpoint = new Uri(baseUrl) });

var chatHistory = new List<ChatMessage> { new SystemChatMessage(SYSTEM_PROMPT) };

Console.WriteLine("Bem vindo ao chat. Digite 'exit' para terminar a conversa.");

while (true)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("> ");

    var prompt = Console.ReadLine();

    if (prompt == "exit")
    {
        Console.WriteLine("bye!");
        break;
    }

    chatHistory.Add(new UserChatMessage(prompt));

    AsyncCollectionResult<StreamingChatCompletionUpdate> response = chatClient.CompleteChatStreamingAsync(chatHistory);

    string text = "";
    Console.ForegroundColor = ConsoleColor.White;
    
    await foreach (var item in response)
    {
        if (item.ContentUpdate.Count > 0)
        {
            Console.Write(item.ContentUpdate[0].Text);
            text += item.ContentUpdate[0].Text;
        }
    }
    Console.WriteLine();

    chatHistory.Add(new AssistantChatMessage(text));
}