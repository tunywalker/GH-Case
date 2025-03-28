$(window).on('load', function () {
    setTimeout(function () {
        $('.loadingScreen').fadeOut(500);
    }, 500);
});

$(document).ready(function () {
    $(".container").hide().fadeIn(2000);
});
