function updateEmployeeTable() {
    var departmentId = $("#departmentsDropdown").val();
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: "/Home/GetEmployeesByDepartment",
        type: "POST",
        data: {
            __RequestVerificationToken: token,
            "departmentId": departmentId
        },
        cache: false,
        success: function (result) {
            var tableContent = $("#tableContent");
            tableContent.html(result);
        },
        error: function (XMLHttpRequest) {
            console.log(XMLHttpRequest);
        }
    });
    return false;
}

function downloadReport() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var departmentId = $("#departmentsDropdown").val();
    window.location.href = "/Home/ExportToExcel?departmentId=" + departmentId;
}