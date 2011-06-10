$(document).ready(function () {
    $.ajax({
        url: "./css/Theme.css",
        dataType: "text",
        success: function (data) {
            var fullTheme = data;
            fullTheme = fullTheme.replace(/{{assets}}/g, 'assets/');            
            $('#themecss').html(fullTheme);
        }
    });
});
