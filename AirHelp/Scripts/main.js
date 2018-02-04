$(document).ready(function () {



    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });


    //nitialize width of position absolute elements
    $('.lSuggestoin').each(function (el) {
        $(this).width($(this).parent().find('input').width() + 50);
    });
    $(window).resize(function () {
        $('.lSuggestoin').each(function (el) {
            $(this).width($(this).parent().find('input').width() + 50);
        });
    });

    $.datepicker.setDefaults($.datepicker.regional['bg']);

    $('#Date').datepicker({ dateFormat: 'dd.mm.yy' });
    $('#Date').datepicker($.datepicker.regional['bg']);
    $('input[type=text]').change(function () {

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
        else {
            result = data.length >= 3;
        }
        if (result && $(this).not('[dropdown]').length > 0) {
            $(this).parent().parent().removeClass('error');
            $(this).parent().parent().addClass('success');
        }
        else {
            $(this).parent().parent().removeClass('success');
        }
    });

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
    });

    // convace 


    initSigniture();

});

var el;
var conv;
var ctx;
var isDrawing;
var couunt = 0;


function initSigniture() {

    el = document.getElementById('signiture-div');
    conv = document.getElementById('signiture');
    ctx = conv.getContext('2d');
    ctx.strokeStyle = 0;
    ctx.lineWidth = 5;
    isDrawing = false;;
    couunt = 0;

    el.onmousedown = function (e) {
        isDrawing = true;
        ctx.lineWidth = 3;
        ctx.lineJoin = ctx.lineCap = 'round';
        ctx.moveTo(e.offsetX, e.offsetY);
        //console.log('down', e.offsetX, e.offsetY);

    };
    el.onmousemove = function (e) {
        if (isDrawing) {
            couunt++;
            ctx.lineTo(e.offsetX, e.offsetY);
            ctx.stroke();
            //console.log('move', e.offsetX, e.offsetY);
        }
        if (couunt > 60) {
            $('.form-box-signiture').removeClass('error').addClass('success');
            saveSigiture();
        }
    };
    el.mouseout = function (e) {
        isDrawing = false;
        //console.log('out');
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
    else if ($this.val().length >= 2) {
        $this.addClass('remove-shadow');
        $this.parent().parent().removeClass('error');


        var isAirtport = $this.not('[name="AirCompany"]').length == 1;

        var url = (isAirtport ? '/api/airports?id=' : '/api/airline?id=') + $this.val();
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
    if (fileName.length > 30) {
        fileName = fileName.substring(0, 30) + '...';
    }
    $parent.find('label').text(fileName);
    $parent.find('button').addClass('success');
}


function validate() {
    var result = true;
    $('input:visible[validate]').each(function (el) {
        if ($(this).parent().parent().not('.success').length > 0) {
            $(this).parent().parent().removeClass('success');
            $(this).parent().parent().addClass('error');
            result = false;
        }
    });

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
        debugger;
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