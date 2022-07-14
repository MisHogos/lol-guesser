using Microsoft.AspNetCore.SignalR;

namespace lolguesser.Hubs {

  public class GameHub : Hub {

    public async Task Marco(string user, string message){
      await Clients.All.SendAsync("ReceiveMessage", "Polo");
    }

    public async Task Ping(string user, string message){
      await Clients.All.SendAsync("ReceiveMessage", "Pong");
    }

  }

}