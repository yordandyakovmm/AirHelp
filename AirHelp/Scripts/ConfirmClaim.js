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
    if (!($('.form-box-signiture').is('.success'))) {
        result = false;
        $('.form-box-signiture').removeClass('success').addClass('error');
    } 

    return result;
}



