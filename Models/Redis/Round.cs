using lolguesser.Models;
using Microsoft.AspNetCore.SignalR;

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

        await _wsLobby.SendAsync("PrepareLoadScreen");
        Thread.Sleep(2 * 1000);
        await _wsLobby.SendAsync("CountDown", 3);
        Thread.Sleep(1000);
        await _wsLobby.SendAsync("CountDown", 2);
        Thread.Sleep(1000);
        await _wsLobby.SendAsync("CountDown", 1);
        Thread.Sleep(1000);
        await _wsLobby.SendAsync("EnterRound", this.result[0], this.result[1]);

        var remainingTime = 60;
        while(remainingTime > 0){
            await _wsLobby.SendAsync("RoundTime", remainingTime);
            Thread.Sleep(1000);
            remainingTime--;

            if(remainingTime == 30){
                if(this.result[1] == "Passive")
                    _wsLobby.SendAsync("RoundTip", "Letter", this.result[1]);
                else{
                    char letterTip = this.result[1][this.result[1].Length - 1];
                    _wsLobby.SendAsync("RoundTip", "Letter", letterTip);
                }
            }

            else if(remainingTime == 15){
                _wsLobby.SendAsync("RoundTip", "Icon", this.result[2]);
            }
        }

        return true;
    }
}