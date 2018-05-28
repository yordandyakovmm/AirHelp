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


function validatePassword() {
    var result = true;
    $('input:visible[validate]').each(function (el) {
        if ($(this).parent().parent().not('.success').length > 0) {
            $(this).parent().parent().addClass('error');
            result = false;
        }
    });

   if ($('#Password').val().length < 8) {
        $('#Password').parent().parent()
            .removeClass('success')
            .addClass('error');
        result = false;
    }
    if ($('#Confirm-password').val() != $('#Password').val()) {
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

function validateEmailForm() {

    $('input:visible[validate]').each(function (el) {
        if ($(this).parent().parent().not('.success').length > 0) {
            $(this).parent().parent().addClass('error');
            result = false;
        }
    });

    var result = true;
    var data = $('#Email').val();
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    result = re.test(data);
    if (!result)
    {
        $('#Email').parent().parent().find('.sub-error').text('невалиден имейл');
        $('#Email').parent().parent()
           .removeClass('success')
           .addClass('error');

    }

    return result;
}
