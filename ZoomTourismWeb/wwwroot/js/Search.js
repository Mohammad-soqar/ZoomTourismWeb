$(document).ready(function () {
  var maxLength = 45;
  $(".show-read-more").each(function () {
    var myStr = $(this).text();
    if ($.trim(myStr).length > maxLength) {
      var newStr = myStr.substring(0, maxLength);
      var removedStr = myStr.substring(maxLength, $.trim(myStr).length);
      $(this).empty().html(newStr);
      $(this).append(
        ' <span href="javascript:void(0);" class="read-more">...</span>'
      );
      $(this).append('<span class="more-text">' + removedStr + "</span>");
    }
  });
  $(".read-more").click(function () {
    $(this).siblings(".more-text").contents().unwrap();
    $(this).remove();
  });
});

const searchInput = document.getElementById("search-input");
const categoryButtons = document.querySelectorAll("#category-buttons button");
const blogList = document.getElementById("blog-list");

function filterBlogItems() {
  const searchTerm = searchInput.value.toLowerCase();
  const selectedCategory = document
    .querySelector("#category-buttons button.active")
    .getAttribute("data-category");
  const blogItems = blogList.querySelectorAll("li");

  blogItems.forEach(function (item) {
    const title = item.querySelector("h3").innerText.toLowerCase();
    const category = item.querySelector("p[hidden]").innerText.toLowerCase();

    const titleMatch = title.indexOf(searchTerm) !== -1;
    const categoryMatch =
      category === selectedCategory || selectedCategory === "all";

    if (titleMatch && categoryMatch) {
      item.style.display = "block";
    } else {
      item.style.display = "none";
    }
  });
}

searchInput.addEventListener("keyup", filterBlogItems);

categoryButtons.forEach(function (button) {
  button.addEventListener("click", function () {
    categoryButtons.forEach(function (btn) {
      btn.classList.remove("active");
    });
    this.classList.add("active");
    filterBlogItems();
  });
});
