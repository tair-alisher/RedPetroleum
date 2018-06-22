function updateEmployeeTable() {
    var departmentId = $("#departmentsDropdown").val();
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: "/Reports/GetEmployeesByDepartment",
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

function downloadReport(reportType) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    if ($("#departmentsDropdown").length > 0) {
        var departmentId = $("#departmentsDropdown").val();
    } else {
        var departmentId = "*";
    }
    
    window.location.href = "/Reports/ExportToExcel?departmentId=" + departmentId + "&reportType=" + reportType;
}
