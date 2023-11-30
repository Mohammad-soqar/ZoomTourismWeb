
//for changing the flag next to the language 
$(document).ready(function () {
    $('#language-select').change(function () {
        var selectedValue = $(this).val();
        var flagContainer = $('.FlagContainer');

        if (selectedValue === 'en') {
            flagContainer.attr('src', '~/assets/icons/britainFlag.png');
        } else if (selectedValue === 'AR') {
            flagContainer.attr('src', '~/assets/icons/SaudiArabiaFlag.png');
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

//showing sidebar on click
$('.show-sidebar-button').click(function () {
    $('.sidebar').toggle();
});

$('.close-button').click(function () {
    $('.sidebar').toggle();
});

