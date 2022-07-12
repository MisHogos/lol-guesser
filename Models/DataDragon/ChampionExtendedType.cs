
public class ChampionExtended
{
    public string? type { get; set; }
    public string? version { get; set; }

    public Dictionary<string, ChampionExtendedData> data { get; set; }

}

public class ChampionExtendedData
{
    public championExtendedSpells[]? spells { get; set; }

    public championExtendedPassive? passive { get; set; }
}

public class championExtendedSpells
{
    public string? id { get; set; }
    public string? name { get; set; }


}

public class championExtendedPassive
{
    public string name { get; set; }
    public string description { get; set; }
}

