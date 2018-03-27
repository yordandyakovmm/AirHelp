function validateUserForm() {
    var result = true;
    $('input:visible[validate]').each(function (el) {
        if ($(this).parent().parent().not('.success').length > 0) {
            $(this).parent().parent().addClass('error');
            result = false;
        }
    });

    if ($('#Password').val().length < 8)
    {
        $('#Password').parent().parent()
            .removeClass('success')
            .addClass('error');
        result = false;
    }
    if ($('#Password').val().length < 8) {
        $('#Password').parent().parent()
            .removeClass('success')
            .addClass('error');
        result = false;
    }
    if ($('#Confirm-password').val() != $('#Password').val())  {
        $('#Confirm-password').parent().parent()
            .removeClass('success')
            .addClass('error');
        result = false;
    }

    return result;
}



