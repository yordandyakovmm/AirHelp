function changeType(type)
{
    clear();
    if (type == 'delay')
    {
        $('[reason]').show();
    }
}
function changeReason(reason)
{

}

function clear()
{
    $('[delay], [cancel], [denay], [willness]').hide();
    $('[delay], [cancel], [denay], [willness]').find('.selected').removeClass('selected');
    $('[delay], [cancel], [denay], [willness]').find('[type=radio]').val('');
}

