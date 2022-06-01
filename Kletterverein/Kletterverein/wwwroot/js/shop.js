$("#btnToggle").click(() => {
    $("#cartList").toggle(1200);
});

$("#cartList").mouseenter(() => {
    $(".productDescription").css("display", "inline");
});

$("#cartList").mouseleave(() => {
    $(".productDescription").css("display", "none");
});



