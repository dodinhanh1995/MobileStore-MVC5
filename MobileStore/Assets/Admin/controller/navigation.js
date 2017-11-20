$(document).ready(function () {
    $('#filtering').keyup(function () {
        var li = $('#side-menu li:not(:first-child)'),
        len = li.length,
        i = 0,
        filter = slug($(this).val());

        while (i < len) {
            var value = slug($(li[i]).text());
            if (value.indexOf(filter) !== -1) {
                $(li[i]).show();
            } else {
                $(li[i]).hide();
            }
            i++;
        }
    });
});

