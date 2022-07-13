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

    //Obtener todos los campeones y devolver el id de uno aleatorio
    private async Task<ChampionResponse> getChamps()
    {
        string version = await getLatestVersion();
        HttpResponseMessage response = await client.GetAsync($"https://ddragon.leagueoflegends.com/cdn/{version}/data/es_ES/champion.json");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ChampionResponse>();
    }

    private async Task<Champion> getRandomChamp()
    {
        ChampionResponse champs = await getChamps();
        Random random = new Random();
        int index = random.Next(champs.data.Count);
        string randomKey = champs.data.Keys.ElementAt(index);
        Champion randomChampion = champs.data[randomKey];
        return randomChampion;
    }


    //Teniendo el id de un personaje aleatorio, devolver el nombre de una habilidad o su pasiva aleatoria
    public async Task<string> getChampionSpell()
    {
        try
        {
            string version = await getLatestVersion();
            Champion randomChamp = await getRandomChamp();

            HttpResponseMessage response = await client.GetAsync($"https://ddragon.leagueoflegends.com/cdn/{version}/data/es_ES/champion/{randomChamp.id}.json");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadFromJsonAsync<ChampionExtended>();

            var posibleValues = new List<string>();
            posibleValues.Add(responseBody.data[randomChamp.id].passive.name);
            foreach (var spell in responseBody.data[randomChamp.id].spells)
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

    public async Task<List<Champion>> getChampions(){
        ChampionResponse champs = await getChamps();
        return champs.data.Values.ToList<Champion>();
    }
}
