
function onBegin() {
    $("img[data-loading]").show();
}

function onSuccess() {
    $("img[data-loading]").hide();
    $.ajax({
        type: "get",
        url: "/Blog/GetComments",
        data: { id: $("#Id").val() },
        success: function (data) {
            console.log(data)
            $("#comments").html(data);
            $("#Name").val("");
            $("#Email").val("");
            $("#Message").val("");
            toastr.success("Your comment has been posted successfully");
        }
    });
}

function onFailure() {
    $("img[data-loading]").hide();
    toastr.error("Failed to retrieve the comments");
}
