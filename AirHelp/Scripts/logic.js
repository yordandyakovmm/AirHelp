function changeType(type)
{
    $('[delay], [cancel], [denay], [willness]').hide();
    $('[delay], [cancel], [denay], [willness]').find('.selected').removeClass('selected');
    $('[delay], [cancel], [denay], [willness]').find('[type=radio]:checked').each(function(){
        this.checked = false;  
    });
    if (type === '1')
    {
       $('[reason]').show(1000);
    }
    if (type === '2') {
        $('[reason]').show(1000);
    }
    if (type === '3') {
        $('[denay-arival]').show(1000);
    }
    $('.submit').attr('disabled');
}
function changeReason(reason)
{
    $('[cancel-delay], [cancel-announsment], [delay-delay]').hide();
    $('[cancel-delay], [cancel-announsment], [delay-delay]').find('.selected').removeClass('selected');
    $('[cancel-delay], [cancel-announsment], [delay-delay]').find('[type=radio]:checked').each(function () {
        this.checked = false;
    });

    if ($('[name="Type"]:checked').val() === '1') {
        $('[delay-delay]').show(1000);
        $('html, body').animate({
            scrollTop: $("[delay-delay]").offset().top
        }, 1000);
    }
    if ($('[name="Type"]:checked').val() === '2') {
        $('[cancel-announsment]').show(1000);
        $('html, body').animate({
            scrollTop: $("[cancel-announsment]").offset().top
        }, 1000);
    }
    $('.submit').attr('disabled', 'disabled');

}

function changeDelay(reason)
{
    $('.submit').removeAttr('disabled');

}

function changeAnnonsment()
{
    $('[cancel-delay], [delay-delay]').hide();
    $('[cancel-delay], [delay-delay]').find('.selected').removeClass('selected');
    $('[cancel-delay], [delay-delay]').find('[type=radio]:checked').each(function () {
        this.checked = false;
    });
    $('.submit').attr('disabled', 'disabled');
    $('[cancel-delay]').show(1000);
    $('html, body').animate({
        scrollTop: $("[cancel-delay]").offset().top
    }, 1000);
}

function changeCancelOverbokingDelay()
{
    $('.submit').removeAttr('disabled');
}

function changeArival()
{
    $('[document-security], [willness]').hide();
    $('[document-security], [willness]').find('.selected').removeClass('selected');
    $('[document-security], [willness]').find('[type=radio]:checked').each(function () {
        this.checked = false;
    });
    $('.submit').attr('disabled', 'disabled');
    $('[document-security]').show(1000);
    $('html, body').animate({
        scrollTop: $("[document-security]").offset().top
    }, 1000);
}

function changeDocumentSecurity()
{
    $('[willness]').hide(1000);
    $('[willness]').find('.selected').removeClass('selected');
    $('[willness]').find('[type=radio]:checked').each(function () {
        this.checked = false;
    });
    $('.submit').attr('disabled', 'disabled');
    $('[willness]').show(1000);
    $('html, body').animate({
        scrollTop: $("[willness]").offset().top
    }, 1000);
}

function changeWillness() {
    $('[cancel-delay], [delay-delay]').hide();
    $('[cancel-delay], [delay-delay]').find('.selected').removeClass('selected');
    $('[cancel-delay], [delay-delay]').find('[type=radio]:checked').each(function () {
        this.checked = false;
    });
    $('.submit').attr('disabled', 'disabled');
    $('[cancel-delay]').show(1000);
    $('html, body').animate({
        scrollTop: $("[cancel-delay]").offset().top
    }, 1000);
}

function clearCheckClaim() {
    $(' [delay], [cancel], [denay], [willness]').hide();
    $('[type-issue], [delay], [cancel], [denay], [willness]').find('.selected').removeClass('selected');
    $('[type-issue], [delay], [cancel], [denay], [willness]').find('[type=radio]:checked').each(function () {
        this.checked = false;
    });

    $('.submit').attr('disabled', 'disabled');
}