(function () {
  window.connection = new signalR.HubConnectionBuilder().withUrl("/ws/game").build();

  window.connection.on("LobbyId", (id) => {
    handleNewLobby(id);
  });

  window.connection.on("PrepareLoadScreen", () => {
    document.getElementById("select-gamemode-screen").style.display = "none";
    document.getElementById("countdown-page").style.display = "flex";
  });

  window.connection.on("CountDown", (count) => {
    document.getElementById("countdown").innerHTML = count;
  });

  window.connection.on("EnterRound", (summonerSpellName, summonerSpellId) => {
    document.getElementById("countdown-page").style.display = "none";
    document.getElementById("game").style.display = "flex";

    document.getElementById("spell-name").innerHTML = summonerSpellName;
  });

  window.connection.on("RoundTime", (time) => {
    document.getElementById("remaining-time").innerHTML = time;
  });

  window.connection.on("RoundTip", (tipKind, tip) => {
    if (tipKind === "Letter") {
      document.getElementById("spell-type-container").style.display = "flex";
      document.getElementById("spell-type").innerHTML = tip.toUpperCase();
    } else if (tipKind === "Icon") {
      document.getElementById("spell-icon-container").style.display = "flex";
      document.getElementById("spell-icon").src = tip;
    }
  });
})();

function handleNewLobby(lobbyId) {
  window.lobbyId = lobbyId;
}
