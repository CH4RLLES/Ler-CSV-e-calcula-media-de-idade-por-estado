using Newtonsoft.Json;
using System;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        CalculeMediaDeIdadePorEstado();
        Console.WriteLine();
        ConverteCsvParaJson();

        Console.ReadKey();

        static void CalculeMediaDeIdadePorEstado()
        {
            var csv = File
             .ReadAllLines(@"c:/input.csv")
             .Select(x => x.Split(','))
             .Select((x, i) => new
             {
                 Cidade = x[1],
                 Idade = Decimal.Parse(x[2])
             })
             .GroupBy(x => x.Cidade)
             .Select(g => new
             {
                 cidade = g.Key,
                 idade = Math.Round(g.Average(c => c.Idade), 2)
             });

            foreach (var item in csv)
            {
                Console.WriteLine(item.cidade+", "+item.idade);
            }
        }

        static void ConverteCsvParaJson()
        {
            var medias = File
             .ReadAllLines(@"c:/input.csv")
             .Select(x => x.Split(','))
             .Select((x, i) => new
             {
                 Cidade = x[1],
                 Idade = Decimal.Parse(x[2])
             })
             .GroupBy(x => x.Cidade)
             .Select(g => new
             {
                 cidade = g.Key,
                 idade = Math.Round(g.Average(c => c.Idade), 2)
             });

            var json = JsonConvert.SerializeObject(new { medias });
            Console.WriteLine(json);
            
        }

        static async Task EnviaJsonViaPost(string json)
        {
            var client = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://zeit-endpoint.brmaeji.now.sh/api/avg", content);

            if (response.Content != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
            }
        }
    }
}