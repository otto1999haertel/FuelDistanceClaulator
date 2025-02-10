document.addEventListener("DOMContentLoaded", function () {
    const toggleButton = document.getElementById("darkModeToggle");
    const body = document.body;
    const title = document.querySelector("title");
    const table = document.getElementById("fuelTable");

    // Dark Mode Status aus dem Local Storage abrufen
    if (localStorage.getItem("darkMode") === "enabled") {
        body.classList.add("dark-mode");
        toggleButton.textContent = "☀️ Mode";
        localStorage.setItem("darkMode", "enabled");
        toggleButton.classList.remove("btn-outline-dark");
        toggleButton.classList.add("btn-outline-light");
    }

    if(table){
        if (body.classList.contains("dark-mode")){
            table.classList.add("table-dark");
        }
        else{
            table.classList.remove("table-dark");
        }
    }

    toggleButton.addEventListener("click", function () {
        body.classList.toggle("dark-mode");

        // Zustand speichern und Button-Text ändern
        if (body.classList.contains("dark-mode")) {
            localStorage.setItem("darkMode", "enabled");
            toggleButton.textContent = "☀️ Mode";
            toggleButton.classList.remove("btn-outline-dark");
            toggleButton.classList.add("btn-outline-light");
            table.classList.add("table-dark");
        } else {
            localStorage.setItem("darkMode", "disabled");
            toggleButton.textContent = "🌙 Mode";
            toggleButton.classList.remove("btn-outline-light");
            toggleButton.classList.add("btn-outline-dark");
            table.classList.remove("table-dark");
        }
    });
    


});