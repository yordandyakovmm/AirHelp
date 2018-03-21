function changeType(type)
{
    $('[delay], [cancel], [denay], [willness]').hide();
    $('[delay], [cancel], [denay], [willness]').find('.selected').removeClass('selected');
    $('[delay], [cancel], [denay], [willness]').find('[type=radio]:checked').each(function(){
        this.checked = false;  
    });
    if (type === '1')
    {
        $('[reason]').show();
    }
    if (type === '2') {
        $('[reason]').show();
    }
    if (type === '3') {
        $('[denay-arival]').show();
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
        $('[delay-delay]').show();
    }
    if ($('[name="Type"]:checked').val() === '2') {
        $('[cancel-announsment]').show();
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
    $('[cancel-delay]').show();
}

function changeHowMuch()
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
    $('[document-security]').show();
}

function changeDocumentSecurity()
{
    $('[willness]').hide();
    $('[willness]').find('.selected').removeClass('selected');
    $('[willness]').find('[type=radio]:checked').each(function () {
        this.checked = false;
    });
    $('.submit').attr('disabled', 'disabled');
    $('[willness]').show();
}

function changeWillness() {
    $('.submit').removeAttr('disabled');
}

function clearCheckClaim() {
    $(' [delay], [cancel], [denay], [willness]').hide();
    $('[type-issue], [delay], [cancel], [denay], [willness]').find('.selected').removeClass('selected');
    $('[type-issue], [delay], [cancel], [denay], [willness]').find('[type=radio]:checked').each(function () {
        this.checked = false;
    });

    $('.submit').attr('disabled', 'disabled');
}