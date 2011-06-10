
function DoSearch() {
    var data = { q: $('#headersearchbox').val(),
        p: 1};
    window.location = '/search?' + $.param(data);
    return true;
}

// Document Ready Function
$(document).ready(function () {

    $('#headersearchlink').click(function () { DoSearch(); return false; });
    $('#headersearchbox').keyup(function (e) {
        if (e.keyCode == 13) {
            DoSearch();
            return true;
        }
        return true;
    });
});                // End Document Ready
        


