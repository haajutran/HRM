
$(document).ready(function () {
    var uri = "";
    $(document).ready(function () {
        var url = window.location.pathname + "";
        for (i = 1; i < url.length; i++) {
            if (url.charAt(i) == '/') {
                break;
            }
            uri += url.charAt(i);
        }
        if (uri == "") {
            $('.Employees').addClass('active');
        }
        else {
            $('.' + uri).addClass('active');
        }

    });
})