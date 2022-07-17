using lolguesser.Models;
using Microsoft.AspNetCore.SignalR;

public class Game
{
    private string lobbyId { get; set; }
    private int actualRound { get; set; }

    private int totalRounds { get; set; }

    public List<Round> rounds { get; set; }

    private Player[] players { get; set; }

    private Boolean isFinished { get; set; }

    private TimeSpan createdAt { get; set; }

    private TimeSpan updatedAt { get; set; }

    private IClientProxy _wsLobby { get; set; }


    public Game(int totalRounds, IClientProxy wsLobby)
    {
        this.lobbyId = Guid.NewGuid().ToString();
        this.actualRound = 1;
        this.totalRounds = totalRounds;
        this.createdAt = DateTime.Now.TimeOfDay;
        this.isFinished = false;
        this.rounds = new List<Round>();
        this._wsLobby = wsLobby;
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


    public async Task<bool> createRound()
    {
        if (this.actualRound <= this.totalRounds)
        {
            this.rounds.Add(new Round(this.actualRound, this._wsLobby));
            await this.rounds.ElementAt(this.actualRound - 1).startRound();
            this.actualRound++;
            return true;
        }
        return false;
    }

}

public class Round
{
    public int roundNumber { get; set; }
    public List<string> tips { get; set; }

    public string[] result { get; set; }

    private IClientProxy _wsLobby;

    private DataDragonModel _apiModel;
    public Round(int roundNumber, IClientProxy wsLobby)
    {
        this.roundNumber = roundNumber;
        _apiModel = new DataDragonModel();
        this._wsLobby = wsLobby;
    }

    public async Task<bool> startRound()
    {
        this.result = await _apiModel.getChampionSpell();
        await _wsLobby.SendAsync("ReceiveMessage", this.result[0]);
        Thread.Sleep(30 * 1000);
        char letterTip = this.result[1][this.result[1].Length - 1];
        await _wsLobby.SendAsync("ReceiveMessage", letterTip);
        Thread.Sleep(30 * 1000);

        return true;
    }
}

public class Player
{
    public string playerId { get; set; }

}