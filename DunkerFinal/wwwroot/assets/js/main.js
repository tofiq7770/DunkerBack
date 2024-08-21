  
function openSearch() {
    document.getElementById("searchOverlay").style.display = "block";
    document.body.classList.add("no-scroll"); 
}

function closeSearch() {
    document.getElementById("searchOverlay").style.display = "none";
    document.body.classList.remove("no-scroll");  
}

function checkEnter(event) {
    if (event.key === "Enter") {
        performSearch();
    }
}

function performSearch() {
    var searchQuery = document.getElementById("searchInput").value;
    alert("Search performed for: " + searchQuery);
    closeSearch(); // Optionally close the search after performing it
}


 