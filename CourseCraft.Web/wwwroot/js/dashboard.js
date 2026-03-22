$(document).ready(function () {
  loadCourses();
});

// Load courses
function loadCourses() {
  $.ajax({
    url: "/Dashboard/GetAllCourses",
    type: "GET",
    success: function (data) {
      console.log(data);

      var container = $("#courseContainer");
      container.empty();

      $.each(data, function (i, course) {
        var cardHtml = `
        <div class="col-sm-6">
          <div class="card mb-3">
            <div class="card-body">
              <h5 class="card-title">${course.courseName}</h5>
              <p class="card-text">Course Content : ${course.courseContent}</p>
              <p class="card-text">Course Credits : ${course.courseCredits}</p>
              <p class="card-text">Department : ${course.courseDepartment}</p>
              <button class="btn btn-primary enroll-button" data-course-id="${course.courseId}">
                Enroll in this Course
              </button>
            </div>
          </div>
        </div>`;
        container.append(cardHtml);
      });
    },
    error: function () {
      toastr.error("Failed to load courses.");
    },
  });
}

// Enroll button click (delegated)
$(document).on("click", ".enroll-button", function () {
  const courseId = $(this).data("course-id");
  console.log(courseId);

  $.ajax({
    url: "/Courses/EnrollStudentInCourse",
    type: "POST",
    data: { courseId: courseId },
    success: function (response) {
      toastr.success(response.message);
    },
    error: function () {
      toastr.error("Enrollment failed.");
    },
  });
});
