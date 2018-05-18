
$('input, textarea').focus(function () {
    $(this).parent().removeClass('error');
});

function fixSize() {
    var heigth = $(window).height() - 240;
    $('.form-content').height(heigth);
}

function validate() {
    var result = true;
    

    if ($("input[name='Email']").length > 0) {
        if ($("input[name='Email']").val().length < 3)
        {
            $("input[name='Email']").parent().addClass('error');
            result = false;
        }
        if ($("input[name='Password']").val().length < 3) {
            $("input[name='Password']").parent().addClass('error');
            result = false;
        }
    }

    if ($("input[name='password']").length > 0) {
        if ($("input[name='password']").val().length < 7 )
        {
            $("input[name='password']").parent().addClass('error');
            $('.autentication-error').text('Паролата е трърде кратка. Паролата трябва да е поне 8 символа');
            result = false;
        }
        else if ($("input[name='password']").val() != $("input[name='password1']").val()) {
            $("input[name='password1']").parent().addClass('error');
            $('.autentication-error').text('Паролите не съвпадат');
            result = false;
        }
    }

    if (!result)
    {
        $('.autentication-error').show();
    }

    return result;
}



function validateContact() {
    var result = true;

    var email = $("input[name='Email']").val();
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    result = re.test(email);
    if (!result) {
        $("input[name='Email']").parent().addClass('error');
    }

    if ($("[name='AdditionalInfo']").val().length < 20) {
        $("[name='AdditionalInfo']").parent().addClass('error');
        result = false;
    }
    
    return result;
}

function clearForm()
{
    $('input').parent().removeClass('error');
    $('.autentication-error').hide();
}
