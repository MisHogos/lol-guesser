using Microsoft.AspNetCore.SignalR;

namespace lolguesser.Hubs
{

    public class GameHub : Hub
    {

        //Join lobby
        public async Task JoinLobby(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
            var game = Game.CreateOrGetGame(group);
            game.AddPlayer(Context.ConnectionId);
        }

        public async Task CreateLobby(){
            var newLobbyId = Guid.NewGuid().ToString();

            await JoinLobby(newLobbyId);
            await Clients.Caller.SendAsync("LobbyId", newLobbyId);
        }

        //Start game
        public void StartGame(string group)
        {
            var lobby = Clients.Group(group);

            ThreadClass thread = new ThreadClass(lobby, 1, group);

            Thread InstanceCaller = new Thread(new ThreadStart(thread.InstanceMethod));
            InstanceCaller.Start();
            InstanceCaller.Join();
        }

        public void SubmitChampion(string championId, string lobbyId){
            Game.SetPlayerPick(lobbyId, Context.ConnectionId, championId);
        } 

    }

}