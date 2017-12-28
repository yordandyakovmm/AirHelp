$(document).ready(function () {

    

    $('input[type=radio]').change(function () {

        if ($(this).is('#rYes:checked')) {
            $('#connectionAirPorts').show();
            $('#rYes').parent().addClass('selected');
            $('#rNo').parent().removeClass('selected');
        }
        else {
            $('#connectionAirPorts').hide();
            $('#rYes').parent().removeClass('selected');
            $('#rNo').parent().addClass('selected');
        }
    });

    function addCbAiports(addButton) {
        var $this = $(addButton);
        var $template = $this.parent().parent().find('.form-box-connection').first();
        $template.find('input').val('');
        $template.find('input').removeClass('success').removeClass('error');
        $this.insertBefore($template);
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
    }
}

function menuItemClick(_this) {
    var $parent = $(_this).parent();
    $parent.parent().find('input').val($(_this).text());
    $parent.hide();
}

function ddKeyUp(_this ,e) {
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
    else if ($this.val().length >= 2) {
        $dropDown.find('li').removeClass('selected');
        $dropDown.find('li').first().addClass('selected');
        $dropDown.show();
    }
    else {
        $dropDown.hide();
    }
}