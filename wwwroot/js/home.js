(function () {
  [...document.querySelectorAll(".icon-container")].forEach((container) => {
    container.addEventListener("click", (evt) => handleClick(evt));
  });

  document.querySelector(".button").addEventListener("click", (evt) => {
    if (evt.currentTarget.classList.contains("blocked")) return;
    console.log("Selected mode: " + window.selectedMode);
    window.location.href = "/game";
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
