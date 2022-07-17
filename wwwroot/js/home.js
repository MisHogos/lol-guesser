(function () {
  [...document.querySelectorAll(".icon-container")].forEach((container) => {
    container.addEventListener("click", (evt) => handleClick(evt));
  });

  let clicks = 0;
  let started = false;

  document.querySelector(".button").addEventListener("click", async (evt) => {
    if (evt.currentTarget.classList.contains("blocked")) return;
    console.log("Selected mode: " + window.selectedMode);

    if (!started) {
      started = true;
      await window.connection.start();
    }
    await window.connection.invoke(clicks % 2 === 0 ? "JoinLobby" : "StartGame", "user");
    clicks++;
  });

  window.connection = new signalR.HubConnectionBuilder().withUrl("/ws/game").build();
  window.connection.on("ReceiveMessage" || "EndGame", (msg) => {
    alert(msg);
  });
})();

function handleClick(evt) {
  document.querySelector(".button").classList.remove("blocked");
  window.selectedMode = evt.currentTarget.dataset.mode;

  const screen = document.querySelector(".screen-container");
  screen.classList.remove("solo", "pvp");
  screen.classList.add(window.selectedMode);

  const currentSelectedElement = document.querySelector(".icon-container .icon.selected");
  if (currentSelectedElement && currentSelectedElement !== evt.currentTarget)
    currentSelectedElement.classList.remove("selected");

  evt.currentTarget.querySelector(".icon").classList.add("selected");
}
