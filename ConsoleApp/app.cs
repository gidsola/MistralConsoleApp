using System.Text;
using Newtonsoft.Json;
using MistralAppConfig;
//using MistralMessageHistory;


namespace MistralConsoleApp {

    internal class MistralChat {

        readonly HttpClient client = new();
        readonly MistralConfig config = new();

        internal static void CountDown() {
            for (int i = 3; i >= 1; i--) {
                Console.Clear();
                Console.Write($"This message will self-destruct {i} seconds...");
                System.Threading.Thread.Sleep(1000);
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

                    Console.WriteLine("\r\n" + response + "\r\n");
                    Console.Write("Have you anymore questions? (Y/N) "); // randoms ?

                    var done = Console.ReadKey(true); // true supresses keypress.

                    if (done.Key == ConsoleKey.N)
                        CountDown();

                    else if (done.Key == ConsoleKey.Y) {
                        Console.Write("\n\nWhat is it then? "); // randoms ?
                        await MakeModelRequest(Console.ReadLine()!);
                    };
                }
                else Console.Write("it didn't go vroom");
            }
            catch (Exception e) {
                Console.WriteLine("Error Message: " + e.Message);
                Console.WriteLine("Stack: " + e.StackTrace);
            };
        }
    }
};

