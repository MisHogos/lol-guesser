using Microsoft.AspNetCore.SignalR;

public class ThreadClass
{

    private IClientProxy _lobby;
    private Game _game;

    public ThreadClass(IClientProxy lobby, string lobbyId)
    {
        this._lobby = lobby;
        this._game = new Game(1, lobby, lobbyId);
    }

    public async void InstanceMethod()
    {
        await this._game.startGame();
    }
}