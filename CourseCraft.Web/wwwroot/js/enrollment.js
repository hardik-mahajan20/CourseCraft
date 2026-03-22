$(document).ready(function () {
  $.ajax({
    url: "/Enrollments/GetEnrollments",
    type: "GET",
    success: function (data) {
      console.log(data);
      if (data.length > 0) {
        let tableBody = $("#enrollmentTable tbody");
        tableBody.empty();
        data.forEach(function (enrollment) {
          let row = `<tr>
                        <td>${enrollment.studentId}</td>
                        <td>${enrollment.studentName}</td>
                        <td>${enrollment.courseId}</td>
                        <td>${enrollment.courseName}</td>
                        <td>${enrollment.isCompleted ? "Yes" : "No"}</td>
                    </tr>`;
          tableBody.append(row);
        });
      } else {
        toastr.error("No enrollments found.");
      }
    },
    error: function (error) {
      toastr.error("An error occurred while fetching enrollments.");
    },
  });
});
