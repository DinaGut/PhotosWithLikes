$(() => {

    $("#like-button").on("click", function () {
        let id = $("#image-id").val()
        $.post("/home/updatelikes", { id }, function () {
            $("#like-button").prop("disabled", true)
        })
    })

    setInterval(() => {
        let id = $("#image-id").val()
        $.get('/home/getlikes', { id }, likes => {
            $("#likes-count").text(likes);
        })
    }, 1000);
})
