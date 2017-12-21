$(document).ready(function () {

    $('[dropdown]').blur(function () {
        var $this = $(this);
        var $dropDown = $this.parent().find('.lSuggestoin');
        if(($this.parent().find('li:hover').length) == 0)
        {
           $dropDown.hide();
        }
    });

    $('.lSuggestoin li').click(function () {
        var $parent = $(this).parent();
        $parent.parent().find('input').val($(this).text());
        $parent.hide();
    });

    $('[dropdown]').keyup(function (e) {
        if (e.which == 37 || e.which == 39)
        {
            return;
        }
        var $this = $(this);
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
        else if ($(this).val().length >= 2) {
            $dropDown.find('li').removeClass('selected');
            $dropDown.find('li').first().addClass('selected');
            $dropDown.show();
        }
        else {
            $dropDown.hide();
        }
    });
});
