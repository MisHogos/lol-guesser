using lolguesser.Models;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

[Serializable]
public class Game
{
    public string lobbyId { get; set; }
    public int actualRound { get; set; }

    public int totalRounds { get; set; }

    public List<Round> rounds { get; set; }

    public List<Player> players { get; set; }

    public Boolean isFinished { get; set; }

    public TimeSpan createdAt { get; set; }

    public TimeSpan updatedAt { get; set; }

    public IClientProxy _wsLobby { get; set; }

    public Game(string lobbyId)
    {
        this.lobbyId = lobbyId;
        this.totalRounds = totalRounds;
        this.createdAt = DateTime.Now.TimeOfDay;
        this.actualRound = 1;
        this.isFinished = false;
        this.rounds = new List<Round>();
        this.players = new List<Player>();
    }

    public async Task<bool> startGame()
    {

        while (!this.isFinished)
        {
            await this.createRound();
            this.isFinished = true;
        }
        await _wsLobby.SendAsync("EndGame", "Partida terminada");
        return true;
    }

    public void SaveGame(){
        var newGame = JsonSerializer.Serialize(this);
        RedisInstance.GetRedisDatabase().StringSet($"game:${this.lobbyId}", newGame);
    }

    public void AddPlayer(string playerId){
        this.players.Add(new Player(playerId));
        this.SaveGame();
    }

    public void RemovePlayer(string playerId){
        this.players = this.players.Where(player => player.playerId != playerId).ToList();
        this.SaveGame();
    }

    public List<Player> GetPlayers(){
        return players;
    }

    public async Task<bool> createRound()
    {
        if (this.actualRound <= this.totalRounds)
        {
            this.rounds.Add(new Round(this.actualRound, this.lobbyId, this._wsLobby));
            await this.rounds.ElementAt(this.actualRound - 1).startRound();
            this.actualRound++;
            return true;
        }
        return false;
    }

    public static void SetPlayerPick(string lobbyId, string playerId, string championId){
        RedisInstance.GetRedisDatabase().StringSet($"game:{lobbyId}:${playerId}", championId);
    }

    public List<string> GetPicks(){
        return RedisInstance
                .GetRedisServer()
                .Keys(RedisInstance.GetRedisDatabase().Database, $"game:{lobbyId}:*")
                .Select(key => (string)RedisInstance.GetRedisDatabase().StringGet(key))
                .Where(val => val != null)
                .ToList<string>();
    }

    public List<string[]> GetPicksWithPlayer(){
        return RedisInstance
                .GetRedisServer()
                .Keys(RedisInstance.GetRedisDatabase().Database, $"game:{lobbyId}:*")
                .Select(key => { 
                    return new string[]{ key.ToString().Split(":").Last(), (string)RedisInstance.GetRedisDatabase().StringGet(key) };
                })
                .ToList<string[]>();
    }

    public static Game? GetGameById(string lobbyId){
        var serializedGame = RedisInstance.GetRedisDatabase().StringGet($"game:${lobbyId}");
        if(serializedGame.IsNullOrEmpty) return null;
        return JsonSerializer.Deserialize<Game>(serializedGame);
    }

    public static Game CreateOrGetGame(string lobbyId){
        var serializedGame = GetGameById(lobbyId);
        if(serializedGame != null) return serializedGame;

        var game = new Game(lobbyId);
        game.SaveGame();
        return game;
    }

}