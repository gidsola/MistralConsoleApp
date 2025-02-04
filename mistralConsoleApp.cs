
using MistralConsoleApp.MistralConfig;
using MistralConsoleApp.MistralRequest;

MistralChatConfig config = new();
MistralChat mistralChat = new();

Console.Write(config.UserPrompt);
await mistralChat.ChatCompletion(Console.ReadLine()!);
