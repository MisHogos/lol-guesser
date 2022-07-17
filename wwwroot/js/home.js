(function () {
  [...document.querySelectorAll(".icon-container")].forEach((container) => {
    container.addEventListener("click", (evt) => handleClick(evt));
  });

  let started = false;

  document.querySelector(".button").addEventListener("click", async (evt) => {
    if (evt.currentTarget.classList.contains("blocked")) return;

    if (!started) {
      started = true;
      await window.connection.start();
      await window.connection.invoke("CreateLobby");
      if (window.selectedMode === "solo") {
        console.log("Assigned to lobby " + window.lobbyId);
        await window.connection.invoke("StartGame", window.lobbyId);
        console.log("Starting game");
      } else {
        alert("Not yet implemented...");
      }
    }
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
