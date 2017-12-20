$(document).ready(function () {
    debugger;

    $('[dropdown]').blur(function () {
        var $this = $(this);
        var $dropDown = $this.parent().find('.lSuggestoin');
        $dropDown.hide();
    });

    $('[dropdown]').keyup(function (e) {
        var $this = $(this);
        var $dropDown = $this.parent().find('.lSuggestoin');
        if (e.which == 13) {
            var $selected = $dropDown.first('li.selected');
            if ($selected.length > 1) {
                $this.val($selected.text());
            }
        }
        // up
        else if (e.which == 38) {
            var $selected = $dropDown.first('li.selected');
            if (($selected).length == 0) {
                $dropDown.find('li').last().addClass('selected');
            }
            else if ($selected.prev('li').length > 1) {
                $selected.removeClass('selected');
                $selected.prev('li').addClass('selected');
            }
        }
        // down
        else if (e.which == 40) {
            var $selected = $dropDown.first('li.selected');
            if (($selected).length == 0) {
                $dropDown.find('li').first().addClass('selected');
            }
            else if ($selected.next('li').length > 1) {
                $selected.removeClass('selected');
                $selected.next('li').addClass('selected');
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
