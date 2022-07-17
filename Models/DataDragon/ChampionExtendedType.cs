
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

    public SpellImage? image { get; set; }

}

public class championExtendedPassive
{
    public string name { get; set; }
    public string description { get; set; }

    public SpellImage image { get; set; }
}

public class SpellImage
{
        public string full { get; set; }
        public string sprite { get; set; }

        public string group { get; set; }

        public string GetUrl(string version){
            var spellGroup = "spell";
            if(group == "passive") spellGroup = "passive";
            return $"https://ddragon.leagueoflegends.com/cdn/{version}/img/{spellGroup}/{full}";
        }

}