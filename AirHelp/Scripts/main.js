$(document).ready(function () {

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

    $('#iDate').datepicker({ dateFormat: 'dd.mm.yy' });
    $('#iDate').datepicker($.datepicker.regional['bg']);



    function addCbAiports(addButton) {
        var $this = $(addButton);
        var $template = $this.parent().parent().find('.form-box-connection').first();
        $template.find('input').val('');
        $template.find('input').removeClass('success').removeClass('error');
        $this.insertBefore($template);
    }


    // convace 
    var el = document.getElementById('signiture');
    var ctx = el.getContext('2d');
    ctx.strokeStyle = 0;
    ctx.lineWidth = 5;
    var isDrawing;

    el.onmousedown = function (e) {
        isDrawing = true;
        ctx.lineWidth = 5;
        ctx.lineJoin = ctx.lineCap = 'round';
        ctx.moveTo(e.layerX, e.layerY);
    };
    el.onmousemove = function (e) {
        if (isDrawing) {
            ctx.lineTo(e.layerX, e.layerY);
            ctx.stroke();
        }
    };
    el.mouseout = function (e) {
        console.log('out');
    };

    el.mousein = function (e) {
        console.log('in');
    };

    el.onmouseup = function () {
        isDrawing = false;
        saveSigiture();

    };

    function saveSigiture() {
        document.getElementById("signiture-img").style.border = "2px solid";
        var dataURL = el.toDataURL();
        document.getElementById("signiture-img").src = dataURL;
        document.getElementById("signiture-img").style.display = "inline";
    }


});


function addCbAiports(addButton) {
    var $this = $(addButton);
    var template = $('#sCbTemplate').html();
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
    $parent.hide();
}

function ddKeyUp(_this, e) {
    if (e.which == 37 || e.which == 39) {
        return;
    }
    var $this = $(_this);
    var $dropDown = $this.parent().find('.lSuggestoin');
    if (e.which == 13) {
        var $selected = $dropDown.find('li.selected');
        if ($selected.length > 0) {
            $this.parent().find('input').val($selected.text());
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
            var url = 'https://www.save70.com/components/autocompleteJson.php?type=airport&term=' + $this.val();
            $.get(url, function (data) {
                debugger;
                for (i = 0; i < data.length; i++) {
                    var li = '<li index="' + i + '" onclick="menuItemClick(this)" ' + (i == data.length - 1 ? 'last' : '') + '>' + data[i].name + '</li>';
                    $dropDown.append(li);
                }
                $dropDown.find('li').removeClass('selected');
                $dropDown.find('li').first().addClass('selected');
                $dropDown.show();
            });
            //return;
        }
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
    if (fileName.length > 20) {
        fileName = fileName.substring(0, 20) + '...';
    }
    $parent.find('label').text(fileName);
    $parent.find('button').addClass('success');
}

function radioChange(obj) {

    $(obj).parent().parent().find('label').removeClass('selected');
    $(obj).parent().addClass('selected');

    if ($(obj).is('#rYes:checked')) {
        $('#connectionAirPorts').show();

    }
    else {
        $('#connectionAirPorts').hide();
    }
}