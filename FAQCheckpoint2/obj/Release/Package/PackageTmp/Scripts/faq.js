$(function () {

    $("body").on("click", ".accordion a", function (event) {
        
        this.classList.toggle('active');
        this.nextElementSibling.classList.toggle('active');
        return false;
    });
});