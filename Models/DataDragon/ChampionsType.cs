public class ChampionRequest
{
    public string? type { get; set; }
    public string? version { get; set; }
    public string? format { get; set; }

}

public class ChampionResponse
{
    public string? type { get; set; }
    public string? version { get; set; }
    public string? format { get; set; }
    public Dictionary<string, Champion>? data { get; set; }
}

public class Champion
{
    public string? version { get; set; }
    public string? id { get; set; }
    public string? key { get; set; }
    public string? name { get; set; }
    public string? title { get; set; }


    public string? blurb { get; set; }
    public string[]? tags { get; set; }
    public string? partype { get; set; }
}