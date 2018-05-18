function validateUserForm() {
    var result = true;
    $('input:visible[validate]').each(function (el) {
        if ($(this).parent().parent().not('.success').length > 0) {
            $(this).parent().parent().addClass('error');
            result = false;
        }
    });

    if ($('#oldPassword').val().length < 3) {
        $('#oldPassword').parent().parent()
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
    if (!result) {
        $('html, body').animate({
            scrollTop: $(".error").first().offset().top
        }, 1000);
    }
    return result;
}



