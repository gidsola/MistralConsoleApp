using MistralConsoleApp;
using MistralAppConfig;

MistralConfig config = new();
MistralChat mistralChat = new();

Console.Write(config.UserPrompt);
await mistralChat.MakeModelRequest(Console.ReadLine()!);

