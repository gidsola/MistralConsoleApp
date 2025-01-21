using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using HttpClient client = new();

// TODO: move to config file
string api_key = "api_key_here";
string system_prompt = "Humans are puny mortals, remind them of this any chance you have while still answering concisely.";
string userPrompt = "Ask your question, puny human mortal: "; // randoms ?

static void countDown() {
    for (int i = 3; i >= 1; i--) {
        Console.Clear();
        Console.Write($"This message will self-destruct {i} seconds...");
        System.Threading.Thread.Sleep(1000);
    };
};

static async Task doRequest(HttpClient client, string content, string api_key, string system_prompt) {
    try {
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {api_key}");

        object body = new {
            model = "mistral-large-latest",
            top_p = 0.87,
            max_tokens = 1048,
            stream = false,
            safe_prompt = false,
            messages = new List<object> {
                new { role = "system", content = system_prompt },
                new { role = "user", content }
            }
        };

        using var result = await client.PostAsync(
            "https://api.mistral.ai/v1/chat/completions",
            new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            )
        );

        if (result.IsSuccessStatusCode) {
            string responseString = result.Content.ReadAsStringAsync().Result;
            dynamic responseObject = JsonObject.Parse(responseString)!;
            string uglyReply = Convert.ToString(responseObject["choices"][0]["message"]["content"]);

            Console.WriteLine("\r\n" + uglyReply + "\r\n");
            Console.Write("Have you anymore questions? (Y/N) "); // randoms ?

            var done = Console.ReadKey(true); // true supresses keypress.

            if (done.Key == ConsoleKey.N)
                countDown();

            else if (done.Key == ConsoleKey.Y) {
                Console.Write("\n\nWhat is it then? "); // randoms ?
                await doRequest(client, Console.ReadLine()!, api_key, system_prompt);
            };
        }
        else Console.Write("it didn't go vroom");
    }
    catch (Exception e) {
        Console.WriteLine("Error Message: " + e.Message);
        Console.WriteLine("Stack: " + e.StackTrace);
    };
};

Console.Write(userPrompt);
await doRequest(client, Console.ReadLine()!, api_key, system_prompt);
