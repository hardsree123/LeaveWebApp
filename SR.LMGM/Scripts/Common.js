
$(document).ready(function () {
    $(".rejLeave").click(function () {
        var TeamDetailPostBackURL = '/Home/Reject';
        var $buttonClicked = $(this);
        var id = $buttonClicked.attr('data-id');
        var options = { "backdrop": "static", keyboard: true };
        $('#popupcontent').html('');
        $.ajax({
            type: "GET",
            url: TeamDetailPostBackURL,
            contentType: "application/json; charset=utf-8",
            data: { "reqId": id },
            datatype: "json",
            success: function (data) {
                $('#popupcontent').html(data);
                $('#myModal').modal(options);
                $('#myModal').modal('show');
            },
            error: function () {
                alert("Dynamic content load failed.");
            }
        });
    });
    $("#closbtn").click(function () {
        $('#popupcontent').html('');
        $('#myModal').modal('hide');
    });
    $("#Leavetype").change(function () {
        if (this.value == 200001) {
            document.getElementById("medcert").style.display = "block";
        }
        else {
            document.getElementById("medcert").style.display = "none";
        }
    });
 
});