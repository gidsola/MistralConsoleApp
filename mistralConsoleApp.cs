
using MistralConfig;
using MistralRequest;

MistralChatConfig config = new();
MistralChat mistralChat = new();

Console.Write(config.UserPrompt);
await mistralChat.ChatCompletion(Console.ReadLine()!);
