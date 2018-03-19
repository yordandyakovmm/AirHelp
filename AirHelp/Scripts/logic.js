function changeType(type)
{
    $('[delay], [cancel], [denay], [willness], [button]').hide();
    $('[delay], [cancel], [denay], [willness], [button]').find('.selected').removeClass('selected');
    $('[delay], [cancel], [denay], [willness], [button]').find('[type=radio]:checked').each(function(){
        this.checked = false;  
    });
    if (type == 'delay')
    {
        $('[reason]').show();
    }
    if (type == 'cancel') {
        $('[reason]').show();
    }
    if (type == 'denay') {
        $('[denay-arival]').show();
    }
}
function changeReason(reason)
{
    $('[cancel-delay], [cancel-announsment], [delay-delay], [button]').hide();
    $('[cancel-delay], [cancel-announsment], [delay-delay], [button]').find('.selected').removeClass('selected');
    $('[cancel-delay], [cancel-announsment], [delay-delay], [button]').find('[type=radio]:checked').each(function () {
        this.checked = false;
    });

    if ($('[name="Type"]:checked').val() == 'delay') {
        $('[delay-delay]').show();
    }
    if ($('[name="Type"]:checked').val() == 'cancel') {
        $('[cancel-announsment]').show();
    }
    

}

function changeDelay(reason)
{
     $('[button]').show();

}

function changeAnnonsment()
{
    $('[cancel-delay], [delay-delay], [button]').hide();
    $('[cancel-delay], [delay-delay], [button]').find('.selected').removeClass('selected');
    $('[cancel-delay], [delay-delay], [button]').find('[type=radio]:checked').each(function () {
        this.checked = false;
    });
    $('[cancel-delay]').show();
}

function changeHowMuch()
{
    $('[button]').show();
}

function changeArival()
{
    $('[document-security], [willness], [button]').hide();
    $('[document-security], [willness] [button]').find('.selected').removeClass('selected');
    $('[document-security], [willness], [button]').find('[type=radio]:checked').each(function () {
        this.checked = false;
    });
    $('[document-security]').show();
}

function changeDocumentSecurity()
{
    $('[willness], [button]').hide();
    $('[willness] [button]').find('.selected').removeClass('selected');
    $('[willness], [button]').find('[type=radio]:checked').each(function () {
        this.checked = false;
    });
    $('[willness]').show();
}

function changeWillness()
{
    $('[button]').show();
}
