function openNav() {
    document.getElementById("mySidenav").style.width = "250px";
    document.getElementById("mainContent").style.marginRight = "250px";
    document.getElementById("openNews").style.display = "none";
}

function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
    document.getElementById("mainContent").style.marginRight = "0px";
    document.getElementById("openNews").style.display = "inline";
}