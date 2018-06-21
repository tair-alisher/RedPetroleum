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

function addTask() {
    $("#emptyTaskList").remove();

    var taskListDiv = $("#taskList");

    if ($("#submitTask").length > 0) {
        var warningMessage = `
    <div class="alert alert-warning  alert-dismissible" role="alert">
        <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <strong>Сохраните задачу</strong>
    </div>
`;
        taskListDiv.prepend(warningMessage);
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
        taskListDiv.append(generatedHtml);
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

    $.ajax({
        url: "/TaskLists/CreateTask",
        type: "POST",
        data: {
            __RequestVerificationToken: token,
            "employeeId": employeeId,
            "taskName": taskName,
            "taskDuration": taskDuration
        },
        cache: false,
        success: function (createdTask) {
            removeGeneratedHtml();
            $("#taskList").append(createdTask);
        },
        error: function (XMLHttpRequest) {
            console.log(XMLHttpRequest);
        }
    });
    return false;
}

function saveOnEnter() {
    $("#taskList").keypress(function (e) {
        if (e.keyCode == 13 && $("#submitTask").length > 0) {
            submitTask();
        }
    })
}