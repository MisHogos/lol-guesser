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

    public Game(int totalRounds, IClientProxy wsLobby, string lobbyId)
    {
        this.lobbyId = lobbyId;
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