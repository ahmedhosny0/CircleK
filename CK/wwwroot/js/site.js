// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//$(document).ready(function () {
//    $('#myTable').DataTable({
//        "scrollY": "450px",
//        "scrollCollapse": true,
//        "paging": true
//    });
//});
        // Check if the ViewBag.StartStopwatch is set to true
if (ViewBag.StartStopwatch != null && (bool)ViewBag.StartStopwatch)
{
    // Start the stopwatch when the page loads
    var startTime = performance.now();

    // Add an event listener to track when the Excel download is complete
    document.getElementById('exportButton').addEventListener('click', function () {
        // Start the stopwatch when the export button is clicked
        startTime = performance.now();

        // Trigger the export action
        $.ajax({
            url: '@Url.Action("ExportToExcel", "Sales")',
            type: 'POST',
            success: function (data) {
                // Create an invisible image element to trigger the download
                var img = new Image();
                img.src = 'data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,' + data;

                // Calculate the elapsed time when the image is loaded
                img.onload = function () {
                    var endTime = performance.now();
                    var elapsedTime = endTime - startTime;

                    // Display the elapsed time in the stopwatch div
                    document.getElementById('stopwatch').innerText = 'Time taken for export: ' + elapsedTime + ' milliseconds';
                };
            }
        });
    });
}

