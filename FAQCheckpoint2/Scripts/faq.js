$(function () {

    $("body").on("click", ".accordion a", function () {
        this.classList.toggle('active');
        this.nextElementSibling.classList.toggle('active');
    });
});