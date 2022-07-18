using Microsoft.AspNetCore.SignalR;

public class ThreadClass
{

    private IClientProxy _lobby;
    private Game _game;

    public ThreadClass(IClientProxy lobby, int rounds, string lobbyId)
    {
        this._lobby = lobby;

        this._game = Game.GetGameById(lobbyId);
        this._game.totalRounds = rounds;
        this._game._wsLobby = lobby;
    }

    public async void InstanceMethod()
    {
        await this._game.startGame();
    }
}