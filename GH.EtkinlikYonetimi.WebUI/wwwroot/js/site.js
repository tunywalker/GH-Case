var loadingScreen = document.querySelector(".loadingScreen");

window.addEventListener('load', function () {
    setTimeout(function () {
        loadingScreen.style.display = 'none';
    }, 500); 
});
$(document).ready(function () {
    $(".container").fadeIn(6000); 

});

