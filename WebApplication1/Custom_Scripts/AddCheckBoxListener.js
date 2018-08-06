$(document).ready(function () {

    $('.ActiveCheck').change(function () {
        var self = $(this);
        var id = self.attr('id');
        var value = self.prop('checked');

        $.ajax({
            url: '/Tarea/AJAXEdit',
            data: {
                id: id,
                value: value
            },
            type: 'POST',
            success: function (result) {
                $('#tableDiv').html(result);                

                if ('@ViewBag.successMessage' != "") {
                    $('#divSuccessMessage').show("slow");                    
                }
                else {
                    $('#divSuccessMessage').hide("slow");                    
                }
            }
        });                
    });
})