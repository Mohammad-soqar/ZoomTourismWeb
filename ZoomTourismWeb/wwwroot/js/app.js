$(document).ready(function () {
    $(".hamburger-menu").click(function () {
        $(".nav-list").toggleClass("active");
        $(".nav-buttons").toggleClass("active");
    });
});

$(document).ready(function () {
    const $carousel = $(".carousel");
    const $prevBtn = $(".prev");
    const $nextBtn = $(".next");

    let currentIndex = 0;
    const $items = $(".carousel-item");
    const numItems = $items.length;

    function calculateVisibleItems() {
        if ($(window).width() < 768) {
            return 1; // Display 1 item on mobile
        } else {
            return 3; // Display 3 items on desktop
        }
    }

    function showItems() {
        const visibleItems = calculateVisibleItems();
        $items.each(function (index, item) {
            if (index >= currentIndex && index < currentIndex + visibleItems) {
                $(item).css("display", "block");
            } else {
                $(item).css("display", "none");
            }
        });
    }

    $prevBtn.on("click", function () {
        currentIndex = Math.max(currentIndex - calculateVisibleItems(), 0);
        showItems();
    });

    $nextBtn.on("click", function () {
        currentIndex = Math.min(
            currentIndex + calculateVisibleItems(),
            numItems - calculateVisibleItems()
        );
        showItems();
    });

    // Initially show the items based on screen width
    showItems();

    // Update the displayed items when the window is resized
    $(window).resize(function () {
        showItems();
    });
});

$(document).ready(function () {
    const $viewport = $(".carousel-viewport");
    const $cardContainers = $(".review-card-container");
    const cardContainerWidth = $cardContainers.first().outerWidth(); // Get the width of one card container
    let currentIndex = 0;

    $(".next-btn").on("click", function () {
        currentIndex = (currentIndex + 1) % $cardContainers.length;
        const translateX = -currentIndex * cardContainerWidth;
        $viewport.css("transform", `translateX(${translateX}px)`);
    });
});
