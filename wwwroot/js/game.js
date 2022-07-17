(function () {
  updateChampionList();
  document.getElementById("champ-search").addEventListener("input", (evt) => {
    updateChampionList(evt.target.value);
  });
})();

function updateChampionList(search) {
  const championList = document.getElementById("champ-list");
  const toAddChamps = !!search
    ? window.champions.filter((champion) =>
        champion.name.toLowerCase().includes(search.toLowerCase())
      )
    : window.champions;

  const imgs = toAddChamps.map(
    (champ) =>
      `<div data-id='${champ.id}' class='champ-img-container ${
        window.selectedChampion === champ.id ? "selected-champ" : ""
      }'>
        <img 
          onClick='setSelectedChampion(\`${champ.id}\`)' 
          title="${champ.name}"
          class='champ-img'
          src="${champ.img}" 
        />
      </div>`
  );
  championList.innerHTML = imgs.join("");
}

function setSelectedChampion(championId) {
  if (window.selectedChampion) {
    const currentSelectedChampion = document.querySelector(
      `*[data-id='${window.selectedChampion}']`
    );
    currentSelectedChampion.classList.remove("selected-champ");
  }

  window.selectedChampion = championId;
  const currentSelectedChampion = document.querySelector(`*[data-id='${championId}']`);
  currentSelectedChampion.classList.add("selected-champ");
}
