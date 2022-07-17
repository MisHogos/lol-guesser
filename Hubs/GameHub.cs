using Microsoft.AspNetCore.SignalR;

namespace lolguesser.Hubs
{

    public class GameHub : Hub
    {

        //Join lobby
        public async Task JoinLobby(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

        //Start game
        public async Task StartGame(string group)
        {

            var lobby = Clients.Group(group);

            //Cramos un hilo
            ThreadClass thread = new ThreadClass(lobby);

            Thread InstanceCaller = new Thread(new ThreadStart(thread.InstanceMethod));
            InstanceCaller.Start();
            InstanceCaller.Join();
        }

        public async Task Ping(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", "Pong");
        }

    }

}