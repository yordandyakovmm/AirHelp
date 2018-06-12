$('[name=SignViaEmail][type=radio]').change(function () {
    $(this).parent().parent().find('label').removeClass('selected');
    $(this).parent().addClass('selected');
    if ($(this).val() == 'yes') {
        $('[signiture]').hide();
        $('[email]').show();
    }
    else
    {
        $('[signiture]').show();
        $('[email]').hide();
    }
});


function validateConfirmClaim() {
    var result = true;
    $('input:visible[validate]').each(function (el) {
        if ($(this).parent().parent().not('.success').length > 0) {
            $(this).parent().parent().addClass('error');
            result = false;
        }
    });
    
    if ($("input[name='confirm']").length) {
        var howMuch = $("input[name='confirm']:checked").val();
        if (!howMuch) {
            result = false;
            $("input[name='confirm']").parent().parent().addClass('error');
        }
        else {
            $("input[name='confirm']").parent().parent().removeClass('error');
        }
    }
    debugger;
    if ($('[name="SignViaEmail"]:checked').val() == 'no') {
        if (!($('.form-box-signiture').is('.success')))  {
            result = false;
            $('.form-box-signiture').removeClass('success').addClass('error');
        }
    }
    if (!result) {
        $('html, body').animate({
            scrollTop: $(".error").first().offset().top
        }, 1000);
    }
    return result;
}


function onchangeInput(_this) {
    var data = $(_this).val();
    if (data.length < 3) {
        $(_this).parent().parent().removeClass('success');
    }
    else
    {
        $(_this).parent().parent().addClass('success');
    }
}

function addPassager() {
    var $this = $(".bbAdd");
    var template = $('#template').html();
    $this.parent().before(template);
}
