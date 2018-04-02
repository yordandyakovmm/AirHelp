$(document).ready(function () {

    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });

    //nitialize width of position absolute elements
    //$('.lSuggestoin').each(function (el) {
    //    $(this).width($(this).parent().find('input').width() + 50);
    //});
    //$(window).resize(function () {
    //    $('.lSuggestoin').each(function (el) {
    //        $(this).width($(this).parent().find('input').width() + 50);
    //    });
    //});

    $.datepicker.setDefaults($.datepicker.regional['bg']);

    $('#Date').datepicker({ dateFormat: 'dd.mm.yy' });
    $('#Date').datepicker($.datepicker.regional['bg']);

    $('input[type=text], input[type=password]').change(onChageInput);

    

    $('input[type=radio]').change(function () {
        $(this).parent().parent().removeClass('error');
        $(this).parent().parent().find('label').removeClass('selected');
        $(this).parent().addClass('selected');
        if ($(this).is('#rYes, #rNo')) {
            if ($(this).is('#rYes:checked')) {
                $('#connectionAirPorts').show();

            }
            else {
                $('#connectionAirPorts').hide();
            }
        }

        if ($(this).is('[name="Type"]'))
        {
            changeType($(this).val());
        }
        if ($(this).is('[name="Reason"]')) {
            changeReason($(this).val());
        }
        if ($(this).is('[name="DelayDelay"]')) {
            changeDelay($(this).val());
        }
        if ($(this).is('[name="CancelAnnonsment"]')) {
            changeAnnonsment($(this).val());
        }
        if ($(this).is('[name="CancelOverbokingDelay"]')) {
            changeCancelOverbokingDelay($(this).val());
        }
        if ($(this).is('[name="Arival"]')) {
            changeArival($(this).val());
        }
        if ($(this).is('[name="DocumentSecurity"]')) {
            changeDocumentSecurity($(this).val());
        }
        if ($(this).is('[name="Willness"]')) {
            changeWillness($(this).val());
        }
    });

    // convace 


    initSigniture();

});





var el;
var conv;
var ctx;
var isDrawing;
var couunt = 0;
var type = 0;
var x = 0;
var y = 0;




function initSigniture() {

    el = document.getElementById('signiture-div');
    if (!el)
    {
        return;
    }

    conv = document.getElementById('signiture');
    ctx = conv.getContext('2d');
    ctx.strokeStyle = 0;
    ctx.lineWidth = 3;
    ctx.lineJoin = 'round';
    ctx.strokeStyle = '#000000';
    ctx.shadowColor = '#000000';
    ctx.shadowBlur = 0.6;
    isDrawing = false;;
    couunt = 0;

    el.onmousedown = function (e) {
        isDrawing = true;
        x = e.offsetX;
        y = e.offsetY;
        ctx.moveTo(e.offsetX, e.offsetX);
    };
    
    el.onmousemove = function (e) {
        if (isDrawing) {
            couunt++;
            var distance = (x - e.offsetX) ** 2 + (y - e.offsetY) ** 2;
            setContext(ctx, distance, x, y)
            ctx.lineTo(e.offsetX, e.offsetY);
            x = e.offsetX;
            y = e.offsetY;
            ctx.stroke();
        }
        if (couunt > 80) {
            $('.form-box-signiture').removeClass('error').addClass('success');
            saveSigiture();
        }
    };
    el.mouseout = function (e) {
        isDrawing = false;
        
    };

    el.mousein = function (e) {
        debugger;
        //console.log('in');
    };

    el.onmouseup = function () {
        isDrawing = false;
        //console.log('up');
        saveSigiture();
    };
}

function saveSigiture() {
    var dataURL = conv.toDataURL('image/png');
    $('[name=SignitureImage]').val(dataURL);
}

function clearSignature() {
    $('.form-box-signiture').removeClass('success').removeClass('error');
    var html = $('.form-box-signiture > div.convas-holder').html();
    $('.form-box-signiture > div.convas-holder').html('');
    $('.form-box-signiture > div.convas-holder').html(html);
    initSigniture();

}

function addCbAiports(addButton) {
    var $this = $(addButton);
    var template = $('#template').html();
    //var $template = $(template);
    $this.parent().before(template);
}

function cbBlur(_this) {
    var $this = $(_this);
    var $dropDown = $this.parent().find('.lSuggestoin');
    if (($this.parent().find('li:hover').length) == 0) {
        $dropDown.hide();
        $this.removeClass('remove-shadow')
    }
}

function menuItemClick(_this) {
    var $parent = $(_this).parent();
    $parent.parent().find('input').val($(_this).text()).removeClass('remove-shadow');
    $parent.parent().find('input').data('data', $(_this).data('data'));
    $(_this).parent().parent().parent().addClass('success');
    $parent.hide();
}


function onChageInput() {

    $('.error-row').hide();

    var data = $(this).val();
    var result = false;

    if (this.id == 'Date') {
        try {
            data = $.datepicker.parseDate('dd.mm.yy', data);
            result = true;
        }
        catch (ex) {
            result = false
        }
    }
    else if (this.id == 'Email') {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        result = re.test(data);
    }
    else if (this.id == 'Tel') {
        result = data.length >= 8;
    }
    else if (this.id == 'FlightNumber') {
        var text = data.replace(' ', '').replace('-', '');
        if (text.length < 3) {
            result = false;
        }
        else {
            var re = /(^[a-zA-Z]){1}([a-zA-Z]|[0-9]){1}([0-9]*$)/;
            result = re.test(text);
            $(this).parent().parent().find('.sub-error').text('Невалиден номер');
        }

    }
    else if (this.id == 'Password') {
        result = data.length >= 8;
    }
    else if (this.id == 'Confirm-password') {
        result = data == $('#Password').val();
    }
    else if (this.id == 'BookCode') {
        var re = /([a-z]|[A-Z]|[0-9]){6}/;
        result = re.test(data);
    }
    else if (this.id == 'TikedNumber') {
        var text = data.replace(' ', '').replace('-', '');
        var re = /([0-9]){12}/;
        result = re.test(text);
    }
    else {
        result = data.length >= 3;
    }

    if (result && $(this).not('[dropdown]').length > 0) {
        $(this).parent().parent().removeClass('error');
        $(this).parent().parent().addClass('success');
    }

}

function ddKeyUp(_this, e) {
    if (e.which == 37 || e.which == 39) {
        return;
    }
    var $this = $(_this);
    $this.parent().parent().removeClass('error');
    $this.parent().parent().removeClass('success');
    var $dropDown = $this.parent().find('.lSuggestoin');
    if (e.which == 13) {
        var $selected = $dropDown.find('li.selected');
        if ($selected.length > 0) {
            $this.parent().find('input').val($selected.text());
            $this.parent().find('input').data('data', $selected.data('data'));
            $this.parent().parent().addClass('success');

        }
        $this.removeClass('remove-shadow');
        $dropDown.hide();
        e.stopPropagation();
        return;
    }
        // up
    else if (e.which == 38) {
        var $selected = $dropDown.find('li.selected');
        if (($selected).length == 0) {
            $dropDown.find('li[last]').addClass('selected');
        }
        else if ($selected.attr('index') != '1') {
            var index = parseInt($selected.attr('index')) - 1
            $selected.removeClass('selected');
            $dropDown.find('[index="' + index + '"]').addClass('selected');
        }
    }
        // down
    else if (e.which == 40) {
        var $selected = $dropDown.find('li.selected');
        if (($selected).length == 0) {
            $dropDown.find('li').first().addClass('selected');
        }
        else if ($selected.not('[last]').length > 0) {
            var index = parseInt($selected.attr('index')) + 1
            $selected.removeClass('selected');
            $dropDown.find('[index="' + index + '"]').addClass('selected');
        }
    }
    else if ($this.val().length >= 3) {
        $this.addClass('remove-shadow');
        $this.parent().parent().removeClass('error');


        var isAirtport = $this.not('[name="AirCompany"]').length == 1;

        var url = (isAirtport ? '/api/airports?text=' : '/api/airline?text=') + $this.val();
        $.get(url, function (data) {
            data = JSON.parse(data);
            if (data.status == 1) {
                $dropDown.html('');
                for (i = 0; i < data.airports.length; i++) {
                    var li = '<li index="' + (i + 1) + '" onclick="menuItemClick(this)" ' + (i == data.airports.length - 1 ? 'last' : '') + '>' +
                        (data.airports[i].name || data.airports[i][i].city) + ' (' + data.airports[i].iata + ')' + '</li>';
                    var $li = $(li).data('data', data.airports[i]);
                    $dropDown.append($li);
                }
                $dropDown.find('li').removeClass('selected');
                $dropDown.find('li').first().addClass('selected');
                $dropDown.show();
            }
        });
        return;
        //}
        //else {
        //    // Air Company
        //    // add filter
        //    $dropDown.find('li').removeClass('selected');
        //    $dropDown.find('li').first().addClass('selected');
        //    $dropDown.show();
        //}

    }
    else {
        $dropDown.hide();
        $this.removeClass('remove-shadow');
    }
}

function uploadClick(obj) {
    $(obj).parent().find('input[type="file"]').click();
}

function uploadChange(obj) {
    var fileName = obj.files[0].name.split('.')[0];
    var $parent = $(obj).parent();
    //if (fileName.length > 30) {
    //    fileName = fileName.substring(0, 30) + '...';
    //}
    if (obj.files.length == 0)
    {
        fileName = ""
    }
    else if (obj.files.length == 1)
    {
        fileName = "1 избран файл";
    }
    else if (obj.files.length >= 1)
    {
        fileName = obj.files.length + " избрани файла";
    }
    $parent.find('label').text(fileName);
    $parent.find('button').addClass('success');
    if ($('[name="upload"]').length > 0)
    {
        $('[name="upload"]').click();
    }
}



function validateCommon()
{
    var result = true;
    $('input:visible[validate]').each(function (el) {
        if ($(this).parent().parent().not('.success').length > 0) {
            $(this).parent().parent().removeClass('success');
            $(this).parent().parent().addClass('error');
            result = false;
        }
    });
    return result;
}

function validate() {
    
    var result = validateCommon();

    if ($("input[name='Reason']").length > 0) {
        var howMuch = $("input[name='Reason']:checked").val();
        if (!howMuch) {
            result = false;
            $("input[name='Reason']").parent().parent().addClass('error');
        }
        else {
            $("input[name='Reason']").parent().parent().removeClass('error');
        }
    }

    if ($("input[name='Delay']").length > 0) {
        var howMuch = $("input[name='Delay']:checked").val();
        if (!howMuch) {
            result = false;
            $("input[name='Delay']").parent().parent().addClass('error');
        }
        else {
            $("input[name='Delay']").parent().parent().removeClass('error');
        }
    }

    if ($("input[name='HowMuch']").length > 0) {
        var HowMuch = $("input[name='HowMuch']:checked").val();
        if (!HowMuch) {
            result = false;
            $("input[name='HowMuch']").parent().parent().addClass('error');
        }
        else {
            $("input[name='HowMuch']").parent().parent().removeClass('error');
        }
    }

    if ($("input[name='Annonsment']").length > 0) {
        var howMuch = $("input[name='Annonsment']:checked").val();
        if (!howMuch) {
            result = false;
            $("input[name='Annonsment']").parent().parent().addClass('error');
        }
        else {
            $("input[name='Annonsment']").parent().parent().removeClass('error');
        }
    }

    if ($("input[name='annonsment']").length > 0) {
        var howMuch = $("input[name='annonsment']:checked").val();
        if (!howMuch) {
            result = false;
            $("input[name='Arival']").parent().parent().addClass('error');
        }
        else {
            $("input[name='Arival']").parent().parent().removeClass('error');
        }
    }

    if ($("input[name='DocumentSecurity']").length > 0) {
        var howMuch = $("input[name='DocumentSecurity']:checked").val();
        if (!howMuch) {
            result = false;
            $("input[name='DocumentSecurity']").parent().parent().addClass('error');
        }
        else {
            $("input[name='DocumentSecurity']").parent().parent().removeClass('error');
        }
    }

    if ($("input[name='Willness']").length > 0) {
        var howMuch = $("input[name='Willness']:checked").val();
        if (!howMuch) {
            result = false;
            $("input[name='Willness']").parent().parent().addClass('error');
        }
        else {
            $("input[name='Willness']").parent().parent().removeClass('error');
        }
    }

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
    if (result) {
        var json = {};
        json.airports = [];
        json.airports.push($('[name="DepartureAirport"]').data('data'));
        $('[name=ConnectionAirports]:visible').each(function (index) {
            var data = $($('[name=ConnectionAirports]')[index]).data('data');
            json.airports.push(data);
        });
        json.airports.push($('[name="DestinationAirports"]').data('data'));
        json.airline = $('[name="AirCompany"]').data('data');
        $('[name="json"]').val(JSON.stringify(json));
        console.log(JSON.stringify(json));
    }
    saveSigiture();
    return result;
}



function clearForm() {
    $('.success').removeClass('success');
    $('.error').removeClass('error');
    $('.selected').removeClass('selected');
    $('.form-box-upload > div > label').text(' -- ');
    $('#connectionAirPorts').hide();
    $('#connectionAirPorts .form-box-connection:not(:first)').remove();
    clearSignature();
}