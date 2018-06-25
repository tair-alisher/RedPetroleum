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


function addTask() {
    $("#emptyTaskList").remove();

    var createArea = $("#createArea");

    if ($("#submitTask").length > 0) {
        var warningMessage = `
    <div class="alert alert-warning  alert-dismissible" role="alert">
        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <strong>Сохраните задачу</strong>
    </div>
`;
        createArea.prepend(warningMessage);
        $("#submitTask").focus();
    }
    else {
        var generatedHtml = `
    <div class="generatedHtml form-group" id="generatedHtml">
        <div class="col-md-5">
            <div class="row">
                <label class="control-label col-md-4" for="TaskName">Задача</label>
                <div class="col-md-8">
                    <input type="text" class="form-control text-box single-line" id="TaskName" name="TaskName">
                </div>
            </div>
        </div>
        <div class="col-md-5">
            <div class="row">
                <label class="control-label col-md-4" for="TaskDuration">Продолжительность</label>
                <div class="col-md-8">
                    <input type="text" class="form-control text-box single-line" id="TaskDuration" name="TaskDuration">
                </div>
            </div>
        </div>
        <div class="col-md-2">
            <button type="button" class="btn btn-success" id="submitTask" onclick="submitTask()" title="Сохранить"><span class="glyphicon glyphicon-ok" aria-hidden="true"></span></button>
            <button type="button" class="btn btn-danger" id="removeGeneratedHtml" onclick="removeGeneratedHtml()" title="Удалить"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></button>
        </div>
    </div>
`;
        createArea.append(generatedHtml);
    }
}

function removeGeneratedHtml() {
    $("#generatedHtml").remove();
}

function submitTask() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var employeeId = $("#employeesDropdown").val();
    var taskName = $("#TaskName").val();
    var taskDuration = $("#TaskDuration").val();
    var taskDate = $("#taskDate").val();

    $.ajax({
        url: "/TaskLists/CreateTask",
        type: "POST",
        data: {
            __RequestVerificationToken: token,
            "employeeId": employeeId,
            "taskName": taskName,
            "taskDuration": taskDuration,
            "taskDate": taskDate
        },
        cache: false,
        success: function (createdTask) {
            removeGeneratedHtml();
            $("#taskList").append(createdTask);
            addTask();
            $("#TaskName").focus();
        },
        error: function (XMLHttpRequest) {
            console.log(XMLHttpRequest);
        }
    });
    return false;
}

function saveOnEnter() {
    $("#createArea").keypress(function (e) {
        if (e.keyCode == 13 && $("#submitTask").length > 0) {
            submitTask();
        }
    })
}

function removeTask(taskId) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: "/TaskLists/DeleteTask",
        type: "POST",
        data: {
            __RequestVerificationToken: token,
            "taskId": taskId
        },
        cache: false,
        success: function () {
            $(`#${taskId}`).remove()
        },
        error: function (XMLHTtpRequest) {
            console.log(XMLHttpRequest);
        }
    });
    return false;
}
