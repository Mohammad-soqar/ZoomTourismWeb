
//for changing the flag next to the language 
$(document).ready(function () {
    $('#languageSelector').change(function () {
        var selectedValue = $(this).val();
        var flagContainer = $('.FlagContainer');

        if (selectedValue === 'en') {
            flagContainer.attr('src', '/Images/icons/britainFlag.png');
        } else if (selectedValue === 'AR') {
            flagContainer.attr('src', '/Images/icons/SaudiArabiaFlag.png');
        }
    });
});

//notification box on click

$(document).click(function (e) {
    var notificationContainer = $(".notificationContiner");
    if (!notificationContainer.is(e.target) && notificationContainer.has(e.target).length === 0) {
        notificationContainer.hide();
    }
});

$("#NotificationButton").click(function (e) {
    e.stopPropagation();
    if ($(".notificationContiner").css("display") === "none") {
        $(".notificationContiner").show();
    } else if ($(".notificationContiner").is(":visible")) {
        $(".notificationContiner").hide();
    }
});
//language-dropdow box on click
$(document).ready(function () {
    var accountMenu = $(".account-menu");

    $(document).click(function (e) {
        if (!accountMenu.is(e.target) && accountMenu.has(e.target).length === 0) {
            accountMenu.hide();
        }
    });

    $(".account-dropdown").click(function (e) {
        e.stopPropagation();
        if (accountMenu.css("display") === "none") {
            accountMenu.show();
        } else if (accountMenu.is(":visible")) {
            accountMenu.hide();
        }
    });
});


$('.show-sidebar-button').click(function () {
    $('.sidebar').toggle();
});

