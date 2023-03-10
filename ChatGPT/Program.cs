using ChatGPT;
using System.Text;
using System.Text.Json;

while (true)
{
    Console.WriteLine("Digite sua pergunta: ");
    var prompt = Console.ReadLine();

    if (prompt.ToLower() == "sair")
        break;

    await Pergunta(prompt);
}

async Task Pergunta(string prompt)
{
    if (String.IsNullOrWhiteSpace(prompt))
        return;

    string apiKey = "";

    using (var client =new HttpClient())
    {
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);
        var response = await client.PostAsync("https://api.openai.com/v1/completions", 
            new StringContent("{\"model\": \"text-davinci-003\", \"prompt\":\"" + prompt + "\", " +
            "\"temperature\": 1, \"max_tokens\": 1024}", Encoding.UTF8, "application/json"));

        if (response.IsSuccessStatusCode)
        {
            string responseString = await response.Content.ReadAsStringAsync();
            Resposta resposta = JsonSerializer.Deserialize<Resposta>(responseString);

            Console.ForegroundColor = ConsoleColor.DarkRed;

            Array.ForEach(resposta.choices.ToArray(), (item) => Console.WriteLine(item.text));


            Console.ResetColor();
        }
        else
        {
            Console.WriteLine("Ocorreu um erro ao enviar a pergunta.");
        }
    }
}