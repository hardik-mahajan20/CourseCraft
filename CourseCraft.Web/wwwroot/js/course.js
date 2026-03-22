$(document).ready(function () {
  let currentPage = 1;
  let pageSize = $("#pageSizeDropdown").val();
  let searchQuery = $("#searchInput").val();

  loadCourses(currentPage, pageSize, searchQuery);

  function loadCourses(page, size, search) {
    $.ajax({
      url: "/Courses/GetCoursesTable",
      type: "GET",
      data: {
        page: page,
        pageSize: size,
        search: search,
      },
      success: function (data) {
        $("#courseContainer").html(data);
        searchQuery = "";
        updatePaginationControls();
        rebindPaginationEvents();
      },
    });
  }

  function updatePaginationControls() {
    let totalPages = parseInt($("#totalPages").val());
    $("#prevPageBtn").prop("disabled", currentPage <= 1);
    $("#nextPageBtn").prop("disabled", currentPage >= totalPages);
  }

  function rebindPaginationEvents() {
    let totalPages = parseInt($("#totalPages").val());

    $("#prevPageBtn")
      .off("click")
      .on("click", function () {
        if (currentPage > 1) {
          currentPage--;
          loadCourses(currentPage, pageSize);
        }
      });

    $("#nextPageBtn")
      .off("click")
      .on("click", function () {
        if (currentPage < totalPages) {
          currentPage++;
          loadCourses(currentPage, pageSize);
        }
      });

    $("#pageSizeDropdown")
      .off("change")
      .on("change", function () {
        pageSize = $(this).val();
        currentPage = 1;
        loadCourses(currentPage, pageSize);
      });

    $(document).on("input", "#searchInput", function () {
      let search = $(this).val().trim();
      searchQuery = search;
      loadCourses(currentPage, pageSize, searchQuery);
      searchQuery = "";
    });
  }

  $("#openAddCourseModal").click(function () {
    $("#addCourseModal").modal("show");
  });

  $("#addCourseForm").submit(function (e) {
    e.preventDefault();

    $(".field-validation-error").text("");

    var form = $(this);
    var formData = form.serialize();

    $.ajax({
      type: "POST",
      url: form.attr("action"),
      data: formData,
      success: function (response) {
        if (response.success) {
          toastr.success("Course added successfully!");
          $("#addCourseForm")[0].reset();

          $(".text-danger").text("");

          $("#addCourseModal").modal("hide");
          loadCourses(currentPage, pageSize);
        } else if (response.errors) {
          $.each(response.errors, function (key, messages) {
            $(`[data-valmsg-for="${key}"]`).text(messages[0]);
          });
        } else {
          toastr.error(response.message || "Something went wrong.");
        }
      },
      error: function () {
        toastr.error("Something went wrong!");
      },
    });
  });

  $("#addCourseModal").on("hidden.bs.modal", function () {
    $("#addCourseForm")[0].reset();

    $(".text-danger").text("");
  });

  $("#editCourseModal").on("hidden.bs.modal", function () {
    $("#editCourseForm")[0].reset();

    $(".text-danger").text("");
  });

  // Edit Course Modal Open
  $(document).on("click", ".edit-course", function (e) {
    e.preventDefault();
    var id = $(this).data("courseId");

    $.ajax({
      url: "/Courses/GetCourseById/" + id,
      type: "GET",
      success: function (response) {
        if (response.success) {
          console.log(response);

          $("#editCourseId").val(response.courseId);
          $("#editCourseName").val(response.courseName);
          $("#editCourseContent").val(response.courseContent);
          $("#editCourseCredits").val(response.courseCredits);
          $("#editCourseDepartment").val(response.courseDepartment);

          $("#editCourseModal").modal("show");
        } else {
          toastr.error(response.message || "Failed to load  course details.");
        }
      },
      error: function () {
        toastr.error("Error loading course details.");
      },
    });
  });

  // Save Changes in Edit Course Modal
  $("#saveChangesBtn").click(function (e) {
    e.preventDefault();

    var formData = $("#editCourseForm").serialize();

    $.ajax({
      type: "POST",
      url: "/Courses/UpdateCourse",
      data: formData,
      success: function (response) {
        if (response.success) {
          toastr.success("Course updated successfully!");
          $("#editCourseModal").modal("hide");
          loadCourses(currentPage, pageSize);
        } else if (response.errors) {
          $.each(response.errors, function (key, messages) {
            $(`[data-valmsg-for="${key}"]`).text(messages[0]);
          });
        } else {
          toastr.error("Error updating course.", "Error");
        }
      },
      error: function (response) {
        if (response.status === 400) {
          $(".text-danger").text("");

          var errors = response.responseJSON.errors;
          $.each(errors, function (key, value) {
            $("#" + key + "Error").text(value[0]);
          });
        } else {
          toastr.error("Error updating tax.");
        }
      },
    });
  });

  // Delete Course
  $(document).on("click", ".delete-course", function () {
    var id = $(this).data("courseId");
    $("#deleteCourseId").val(id);
    $("#deleteCourseModal").modal("show");
  });
  $("#deleteCourseForm").submit(function (e) {
    e.preventDefault();

    var id = $("#deleteCourseId").val();

    $.ajax({
      type: "POST",
      url: "/Courses/SoftDeleteCourse",
      data: { id: id },
      dataType: "json",
      success: function (response) {
        if (response.success) {
          toastr.success("Course deleted successfully!");
          $("#deleteCourseModal").modal("hide");
          loadCourses(currentPage, pageSize);
        } else {
          toastr.error("Error deleting Course.");
        }
      },
      error: function () {
        toastr.error("Error deleting Course.");
      },
    });
  });

  $(document).on("change", ".form-check-input-quick", function () {
    var checkbox = $(this);
    var courseId = checkbox.data("id");
    var isChecked = checkbox.is(":checked");
    var toggleType = checkbox.data("type");
    var previousValue = !isChecked;

    $.ajax({
      url: "/Courses/ToggleCourseField",
      type: "POST",
      data: { courseId: courseId, isChecked: isChecked, field: toggleType },
      success: function (response) {
        if (response.success) {
          let customMessage = "";

          switch (toggleType) {
            case "IsEnabled":
              customMessage = isChecked
                ? "Course has been opened successfully."
                : "Course has been close successfully.";
              break;
            default:
              customMessage = "Field updated successfully.";
          }
          toastr.success(customMessage);
        } else {
          toastr.error(
            response.message || "Something went wrong while updating.",
          );
          setTimeout(() => {
            checkbox.prop("checked", previousValue);
          }, 500);
        }
      },
      error: function (xhr) {
        if (xhr.status === 403) {
          toastr.error(
            "Access Denied: You don't have permission to perform this action.",
          );
        } else {
          toastr.error("An error occurred while updating.");
        }
        // Revert with a delay
        setTimeout(() => {
          checkbox.prop("checked", previousValue);
        }, 500);
      },
    });
  });
});
