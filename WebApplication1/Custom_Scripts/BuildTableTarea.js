$(document).ready(function () {

    $.ajax({
        url: '/Tarea/BuildToDoTable',
        success: function (result) {
            $('#tableDiv').html(result);
        }
    });

});