namespace lolguesser.Models;

public class DataDragonModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    static readonly HttpClient client = new HttpClient();

    //Ultima version de DDRAGON
    private async Task<string> getLatestVersion()
    {
        HttpResponseMessage response = await client.GetAsync("https://ddragon.leagueoflegends.com/api/versions.json");
        response.EnsureSuccessStatusCode();
        var value = response.Content;
        var responseBody = await response.Content.ReadFromJsonAsync<string[]>();

        return responseBody[0].ToString();
    }
    private async Task<string> getChamps()
    {
        string version = await getLatestVersion();
        HttpResponseMessage response = await client.GetAsync($"https://ddragon.leagueoflegends.com/cdn/{version}/data/es_ES/champion.json");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadFromJsonAsync<ChampionResponse>();

        var responseString = await response.Content.ReadAsStringAsync();

        Random random = new Random();
        int index = random.Next(responseBody.data.Count);

        string randomKey = responseBody.data.Keys.ElementAt(index);
        Champion randomChampion = responseBody.data[randomKey];
        return randomChampion.id;
    }

    public async Task<string> getChampionSpell()
    {
        try
        {
            string version = await getLatestVersion();
            string championId = await getChamps();

            HttpResponseMessage response = await client.GetAsync($"https://ddragon.leagueoflegends.com/cdn/{version}/data/es_ES/champion/{championId}.json");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadFromJsonAsync<ChampionExtended>();

            var posibleValues = new List<string>();
            posibleValues.Add(responseBody.data[championId].passive.name);
            foreach (var spell in responseBody.data[championId].spells)
            {
                posibleValues.Add(spell.name);
            }

            Random random = new Random();
            int index = random.Next(posibleValues.Count);
            return posibleValues[index];
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            return "";
        }
    }
}
