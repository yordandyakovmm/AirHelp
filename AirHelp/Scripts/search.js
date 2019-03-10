$("#searchText").keyup(function () {
    debouncedSearch();
});

var debouncedSearch = debounce(search, 300);

function search() {

    var searchText = $('#searchText').val();
    var searchAll = $('#searchAll').val();
    var searchFull = $('#searchFull').val();

    if (searchText.length == 1) {
        return;
    }

    var url = "/search?searchText=" + searchText;
    $.get(url, function (data) {
        $('#searchResult').html(data);
    });
}

function deleteClaim(id, text) {
    var searchText = $('#searchText').val();
    var searchAll = $('#searchAll').val();
    var searchFull = $('#searchFull').val();
    if (confirm("Потвърждавате ли изтриването на " + text)){
        var url = "/deleteClaim?id="+id + "&searchText=" + searchText;
        $.get(url, function (data) {
            $('#searchResult').html(data);
        });
    }
}

function debounce(fn, delay) {
    var timer = null;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            fn.apply(context, args);
        }, delay);
    };
}

               