﻿$(document).ready(function () {

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
        if (result) {
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

        if ($(this).is('#rYes:checked')) {
            $('#connectionAirPorts').show();

        }
        else {
            $('#connectionAirPorts').hide();
        }
    });


    initSigniture();

});


// convace 
var el = document.getElementById('signiture');
var ctx = el.getContext('2d');
ctx.strokeStyle = 0;
ctx.lineWidth = 5;
var isDrawing;
var couunt = 0;

function initSigniture() {

    el.onmousedown = function (e) {
        isDrawing = true;
        ctx.lineWidth = 3;
        ctx.lineJoin = ctx.lineCap = 'round';
        ctx.moveTo(e.layerX, e.layerY);
    };
    el.onmousemove = function (e) {
        if (isDrawing) {
            couunt++;
            ctx.lineTo(e.layerX, e.layerY);
            ctx.stroke();
        }
        if (couunt > 60) {
            $('.form-box-signiture').removeClass('error').addClass('success');
        }
    };
    el.mouseout = function (e) {
        isDrawing = false;
        console.log('out');
    };

    el.mousein = function (e) {
        debugger;
        console.log('in');
    };

    el.onmouseup = function () {
        isDrawing = false;
        //saveSigiture();
    };
}

function saveSigiture() {
    document.getElementById("signiture-img").style.border = "2px solid";
    var dataURL = el.toDataURL();
    document.getElementById("signiture-img").src = dataURL;
    document.getElementById("signiture-img").style.display = "inline";
}

function clearSignature() {
    $('.form-box-signiture').removeClass('success').removeClass('error');
    var html = $('.form-box-signiture > div.convas-holder').html();
    $('.form-box-signiture > div.convas-holder').html('');
    $('.form-box-signiture > div.convas-holder').html(html);
    el = document.getElementById('signiture');
    ctx = el.getContext('2d');
    ctx.strokeStyle = 0;
    ctx.lineWidth = 5;
    isDrawing;
    couunt = 0;
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

            $this.parent().parent().addClass('success');

        }
        $this.removeClass('remove-shadow');
        $dropDown.hide();
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
        else if ($selected.not('[last]')) {
            var index = parseInt($selected.attr('index')) + 1
            $selected.removeClass('selected');
            $dropDown.find('[index="' + index + '"]').addClass('selected');
        }
    }
    else if ($this.val().length >= 3) {
        $this.addClass('remove-shadow');
        $this.parent().parent().removeClass('error');

        if ($this.val().length == 3) {
            if ($this.not('[name="AirCompany"]').length == 1) {
                var url = '/api/airports?id=' + $this.val();
                $.get(url, function (data) {
                    data = JSON.parse(data);
                    console.log(data);
                    if (data.status == 'success') {
                        $dropDown.html('');
                        for (i = 0; i < data.results.length; i++) {
                            var li = '<li index="' + (i+1) + '" onclick="menuItemClick(this)" ' + (i == data.results.length - 1 ? 'last' : '') + '>' + data.results[i].airportName + ' (' + data.results[i].threeCode + ')'+ '</li>';
                            $dropDown.append(li);
                        }
                        $dropDown.find('li').removeClass('selected');
                        $dropDown.find('li').first().addClass('selected');
                        $dropDown.show();
                    }
                });
                return;
            }
            else
            {
                // Air Company
                // add filter
                $dropDown.find('li').removeClass('selected');
                $dropDown.find('li').first().addClass('selected');
                $dropDown.show();
            }
        }
        // should remive next 3 rows
        $dropDown.find('li').removeClass('selected');
        $dropDown.find('li').first().addClass('selected');
        $dropDown.show();

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
    $('input:visible').each(function (el) {
        if ($(this).parent().parent().not('.success').length > 0) {
            $(this).parent().parent().removeClass('success');
            $(this).parent().parent().addClass('error');
            result = false;
        }
    });

    if (true) {
        var howMuch = $("input[name='reason']:checked").val();
        if (!howMuch) {
            result = false;
            $("input[name='reason']").parent().parent().addClass('error');
        }
        else {
            $("input[name='reason']").parent().parent().removeClass('error');
        }
    }

    if (true) {
        var howMuch = $("input[name='delay']:checked").val();
        if (!howMuch) {
            result = false;
            $("input[name='delay']").parent().parent().addClass('error');
        }
        else {
            $("input[name='delay']").parent().parent().removeClass('error');
        }
    }

    if (true) {
        var howMuch = $("input[name='howMuch']:checked").val();
        if (!howMuch) {
            result = false;
            $("input[name='howMuch']").parent().parent().addClass('error');
        }
        else {
            $("input[name='howMuch']").parent().parent().removeClass('error');
        }
    }

    if (true) {
        var howMuch = $("input[name='annonsment']:checked").val();
        if (!howMuch) {
            result = false;
            $("input[name='annonsment']").parent().parent().addClass('error');
        }
        else {
            $("input[name='annonsment']").parent().parent().removeClass('error');
        }
    }

    if (true) {
        var howMuch = $("input[name='annonsment']:checked").val();
        if (!howMuch) {
            result = false;
            $("input[name='arival']").parent().parent().addClass('error');
        }
        else {
            $("input[name='arival']").parent().parent().removeClass('error');
        }
    }

    if (true) {
        var howMuch = $("input[name='documentSecurity']:checked").val();
        if (!howMuch) {
            result = false;
            $("input[name='documentSecurity']").parent().parent().addClass('error');
        }
        else {
            $("input[name='documentSecurity']").parent().parent().removeClass('error');
        }
    }

    if (true) {
        var howMuch = $("input[name='willness']:checked").val();
        if (!howMuch) {
            result = false;
            $("input[name='willness']").parent().parent().addClass('error');
        }
        else {
            $("input[name='willness']").parent().parent().removeClass('error');
        }
    }

    if (true) {
        var howMuch = $("input[name='confirm']:checked").val();
        if (!howMuch) {
            result = false;
            $("input[name='confirm']").parent().parent().addClass('error');
        }
        else {
            $("input[name='confirm']").parent().parent().removeClass('error');
        }
    }
    if ($('.form-box-signiture').is('.success') == 0) {
        result = false;
        $('.form-box-signiture').addClass('error');
    }
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