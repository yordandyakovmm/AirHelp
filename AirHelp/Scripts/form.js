$(document).ready(function () {
    $('input').change(function () {
        $(this).parent().removeClass('error');
        $('.autentication-error').hide();
    });
});

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


function clearForm()
{
    $('input').parent().removeClass('error');
    $('.autentication-error').hide();
}
