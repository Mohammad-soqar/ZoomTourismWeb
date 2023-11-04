function sortTable(column, dataType) {
    var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;
    table = document.getElementById("studentTable");
    switching = true;
    dir = "asc";

    // Get the current sorting direction from the data attribute
    var currentDirection = table.querySelector("th:nth-child(" + (column + 1) + ")").getAttribute("data-sort-direction");

    if (currentDirection === "asc") {
        dir = "desc";
    }

    // Reset sorting direction for all columns
    var headers = table.getElementsByTagName("th");
    for (i = 0; i < headers.length; i++) {
        headers[i].setAttribute("data-sort-direction", "asc");
    }

    // Set the new sorting direction
    table.querySelector("th:nth-child(" + (column + 1) + ")").setAttribute("data-sort-direction", dir);

    while (switching) {
        switching = false;
        rows = table.getElementsByTagName("tr");
        for (i = 1; i < (rows.length - 1); i++) {
            shouldSwitch = false;
            x = rows[i].getElementsByTagName("td")[column];
            y = rows[i + 1].getElementsByTagName("td")[column];

            if (dataType === 'date') {
                x = new Date(parseDate(x.textContent));
                y = new Date(parseDate(y.textContent));
            } else if (dataType === 'text') {
                x = x.textContent.toLowerCase();
                y = y.textContent.toLowerCase();
            }

            if (dir === "asc") {
                if (x > y) {
                    shouldSwitch = true;
                    break;
                }
            } else if (dir === "desc") {
                if (x < y) {
                    shouldSwitch = true;
                    break;
                }
            }
        }
        if (shouldSwitch) {
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
            switchcount++;
        }
    }

    // Reset arrow rotation for all arrows
    var arrows = document.querySelectorAll(".sort");
    arrows.forEach(function (arrow) {
        arrow.style.transform = "";
    });

    // Rotate the current arrow based on sorting direction
    var arrow = table.querySelector("th:nth-child(" + (column + 1) + ") .sort");
    if (dir === "desc") {
        arrow.style.transform = "rotate(180deg)";
    }
}

function parseDate(dateString) {
    var parts = dateString.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}





//search
document.getElementById("searchInput").addEventListener("input", function () {
    var input, filter, table, tr, td, i, j, txtValue;
    input = document.getElementById("searchInput");
    filter = input.value.toLowerCase();
    table = document.getElementById("studentTable");
    tr = table.getElementsByTagName("tr");
    for (i = 1; i < tr.length; i++) {
        var found = false; // Flag to track if the row matches the search criteria
        td = tr[i].getElementsByTagName("td");
        for (j = 0; j < td.length; j++) {
            txtValue = td[j].textContent || td[j].innerText;
            if (txtValue.toLowerCase().indexOf(filter) > -1) {
                found = true; // The row contains a match
                break;
            }
        }
        if (found) {
            tr[i].style.display = "";
        } else {
            tr[i].style.display = "none";
        }
    }
});







$(document).ready(function () {
    function updateProgress(value) {
        var progressBar = $('.progress-circle');
        var progressNumber = $('.progress-number');

        var angle = value * 72; // Each number takes 36 degrees

        progressBar.css({
            'background-image': 'conic-gradient(#007bff 0 ' + angle + 'deg, #f2f2f2 ' + angle + 'deg 360deg)'
        });

        progressNumber.text(value);
    }

    // You can call this function with the value from your backend
    var valueFromBackend = 3.4; // Example value from backend
    updateProgress(valueFromBackend);
});








jQuery(document).ready(function () {

    var el;
    var options;
    var canvas;
    var span;
    var ctx;
    var radius;

    var createCanvasVariable = function (id) {  // get canvas
        el = document.getElementById(id);
    };

    var createAllVariables = function () {
        options = {
            percent: el.getAttribute('data-percent') || 25,
            size: el.getAttribute('data-size') || 165,
            lineWidth: el.getAttribute('data-line') || 15,
            rotate: el.getAttribute('data-rotate') || 0,
            color: el.getAttribute('data-color')
        };

        canvas = document.createElement('canvas');
        span = document.createElement('span');
        span.textContent = options.percent + '%';

        if (typeof (G_vmlCanvasManager) !== 'undefined') {
            G_vmlCanvasManager.initElement(canvas);
        }

        ctx = canvas.getContext('2d');
        canvas.width = canvas.height = options.size;

        el.appendChild(span);
        el.appendChild(canvas);

        ctx.translate(options.size / 2, options.size / 2); // change center
        ctx.rotate((-1 / 2 + options.rotate / 180) * Math.PI); // rotate -90 deg

        radius = (options.size - options.lineWidth) / 2;
    };


    var drawCircle = function (color, lineWidth, percent) {
        percent = Math.min(Math.max(0, percent || 1), 1);
        ctx.beginPath();
        ctx.arc(0, 0, radius, 0, Math.PI * 2 * percent, false);
        ctx.strokeStyle = color;
        ctx.lineCap = 'square'; // butt, round or square
        ctx.lineWidth = lineWidth;
        ctx.stroke();
    };

    var drawNewGraph = function (id) {
        el = document.getElementById(id);
        createAllVariables();
        drawCircle('#efefef', options.lineWidth, 100 / 100);
        drawCircle(options.color, options.lineWidth, options.percent / 100);


    };
    drawNewGraph('graph1');
    drawNewGraph('graph2');
    drawNewGraph('graph3');
    drawNewGraph('graph4');


});