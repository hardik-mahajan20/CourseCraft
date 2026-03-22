$(document).ready(function () {
  // Get URLs from data attributes (we will set in HTML)
  const getCoursesUrl = $("#myCoursesLink").data("get-url");
  const markCompleteUrl = $("#myCoursesLink").data("complete-url");

  $("#myCoursesLink").click(function () {
    $.ajax({
      url: getCoursesUrl,
      type: "GET",
      success: function (data) {
        console.log(data);

        var coursesList = $("#coursesList");
        coursesList.empty();

        if (data.length > 0) {
          data.forEach(function (course) {
            let buttonText = course.isCompleted
              ? "Mark as Incomplete"
              : "Mark As Complete";

            let buttonClass = course.isCompleted
              ? "btn-warning"
              : "btn-primary";

            let courseItem = `
                            <li class="list-group-item d-flex justify-content-between">
                                ${course.courseName}
                                <button class="btn btn-sm ${buttonClass} toggle-complete" 
                                   data-course-id="${course.courseId}"
                                    data-is-completed="${course.isCompleted}">
                                    ${buttonText}
                                </button>
                            </li>
                        `;
            coursesList.append(courseItem);
          });
        } else {
          coursesList.append(
            '<li class="list-group-item">No courses found.</li>',
          );
        }

        $("#coursesModal").modal("show");
      },
      error: function (error) {
        console.log(error);
        alert("An error occurred while fetching the courses.");
      },
    });
  });

  $(document).on("click", ".toggle-complete", function () {
    var courseId = $(this).data("course-id");

    $.ajax({
      url: markCompleteUrl,
      type: "POST",
      data: { courseId: courseId },
      success: function (response) {
        if (response.success) {
          alert("Course status updated!");
          $("#coursesModal").modal("hide");
        } else {
          alert("Failed to mark course as complete.");
        }
      },
      error: function (error) {
        console.log(error);
        alert("An error occurred while marking the course as complete.");
      },
    });
  });
});
