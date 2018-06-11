function validateUserForm() {
    var result = true;
    $('input:visible[validate]').each(function (el) {
        if ($(this).parent().parent().not('.success').length > 0) {
            $(this).parent().parent().addClass('error');
            result = false;
        }
    });

    if ($('#Adress').val().length <= 10)
    {
        $('#Adress').parent().parent()
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


function validateEditUserForm() {
    debugger;
    var result = true;
    $('input:visible[validate]').each(function (el) {
        if ($(this).parent().parent().not('.success').length > 0) {
            $(this).parent().parent().addClass('error');
            result = false;
        }
    });

    if ($('#Adress').val().length <= 10) {
        $('#Adress').parent().parent()
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

function clearUserEditForm() {
    $('input:visible[validate]').each(function (el) {
        $(this).parent().parent().removeClass('error').addClass('success');
     });
    
}
