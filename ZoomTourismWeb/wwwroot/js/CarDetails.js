$(document).ready(function () {
    const $thumbnails = $(".thumbnail");
    const $popupContent = $(".main-image img");
    const $prevButton = $("#prev-image");
    const $nextButton = $("#next-image");

    let currentIndex = 0;

    function updateImage(index) {
        const $selectedThumbnail = $($thumbnails[index]);
        const imagePath = $selectedThumbnail.attr("src");

        $popupContent.attr("src", imagePath);
        currentIndex = index;
    }

    // Show the first image when the page loads
    updateImage(currentIndex);

    $thumbnails.click(function () {
        const $selectedImage = $(this);
        const index = $thumbnails.index($selectedImage);
        updateImage(index);
    });

    $prevButton.click(function () {
        currentIndex = (currentIndex - 1 + $thumbnails.length) % $thumbnails.length;
        updateImage(currentIndex);
    });

    $nextButton.click(function () {
        currentIndex = (currentIndex + 1) % $thumbnails.length;
        updateImage(currentIndex);
    });
});
