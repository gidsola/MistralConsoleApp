using System.Text;
using Newtonsoft.Json;
using MistralConfig;
//using MistralMessageHistory;


/*
 * TODO:
 * This all needs to be cleaned up. 
 * extract the texts surrounding the requester (MakeModelRequest)
 * and start adding other "requesters"
 */

namespace MistralRequest {

    internal class MistralChat {

        readonly HttpClient client = new();
        readonly MistralChatConfig config = new();

        internal static void CountDown() { // TODO
            for (int i = 3; i >= 1; i--) {
                Console.Clear();
                Console.Write($"This message will self-destruct {i} seconds...");
                Thread.Sleep(1000);
            };
        }

        public async Task MakeModelRequest(string content) {
            try {

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.ApiKey}");

                dynamic body = new {
                    model = config.Model,
                    top_p = config.Top_p,
                    max_tokens = config.Max_tokens,
                    stream = config.Stream,
                    safe_prompt = config.Safe_prompt,
                    messages = new List<dynamic> {
                        new { role = "system", content = config.SystemPrompt },
                        new { role = "user", content }
                    }
                };

                using var result = await client.PostAsync(
                    config.Endpoint,
                    new StringContent(
                        JsonConvert.SerializeObject(body),
                        Encoding.UTF8,
                        "application/json"
                    )
                );

                if (result.IsSuccessStatusCode) {
                    string responseString = result.Content.ReadAsStringAsync().Result;
                    dynamic responseObject = JsonConvert.DeserializeObject(responseString)!;
                    string response = responseObject.choices[0].message.content;

                    Console.WriteLine("\r\n" + response + "\r\n"); // TODO
                    Console.Write("Have you anymore questions? (Y/N) "); // TODO

                    var done = Console.ReadKey(true); // something happened here, unexpected keys are exiting process..

                    if (done.Key == ConsoleKey.N) // TODO
                        CountDown();

                    else if (done.Key == ConsoleKey.Y) { // TODO
                        Console.Write("\n\nWhat is it then? "); 
                        await MakeModelRequest(Console.ReadLine()!);
                    };
                }
                else throw new Exception("it didn't go vroom"); // change to reflect info from details
            }
            catch (Exception e) { // get specific; details from request when error
                Console.WriteLine("error in requester: " + e);
            };
        }
    };

    internal class MistralVision { }; // TODO
};
