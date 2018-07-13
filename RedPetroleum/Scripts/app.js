//This function for show and update DataTable ReportByDepartment on View
function updateDepartmentTable() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var dropDown = document.getElementById("departmentsDropdown");
    var departmentId = $("#departmentsDropdown").val();

    var dateValue = $("#taskDate").val();
    var dateFormat = new Date(dateValue);
    var options = { month: 'long', year: 'numeric' };
    //Check if DropDown is empty disabled button. 
    if (departmentId === "") {
        $("#downloadBtn").prop('disabled', true);
        $("#tableContent").empty();
        $("#DepartmentName").text(dropDown.options[dropDown.selectedIndex].text);
        $("#dt").text("ОЦЕНКА ЭФФЕКТИВНОСТИ ПЕРСОНАЛА - " + dateFormat.toLocaleDateString("ru-RU", options).charAt(0).toUpperCase() + dateFormat.toLocaleString("ru-RU", options).slice(1));

    } else {
        $.ajax({
            url: "/Reports/PartialReportByDepartment",
            type: "POST",
            data: {
                __RequestVerificationToken: token,
                "departmentId": departmentId,
                "dateValue": dateValue
            },
            cache: false,
            success: function (result) {
                $("#dt").text("ОЦЕНКА ЭФФЕКТИВНОСТИ ПЕРСОНАЛА - " + dateFormat.toLocaleDateString("ru-RU", options).charAt(0).toUpperCase() + dateFormat.toLocaleString("ru-RU", options).slice(1));
                //If DropDown not empty written the select item on head of Table
                $("#DepartmentName").text(dropDown.options[dropDown.selectedIndex].text);
                var tableContent = $("#tableContent");
                tableContent.html(result);
                $("#downloadBtn").prop('disabled', false);
                //Check If DataTable is Empty disabled button
                if (result.length === "2") {
                    $("#downloadBtn").prop('disabled', true);
                }
            },
            error: function (XMLHttpRequest) {
                console.log(XMLHttpRequest);
            }
        });
    }
    return false;
}
//This function for show and update DataTable ReportByCompany on View
function updateCompanyTable() {
    var options = { month: 'long', year: 'numeric' };
    var dateValue = $("#taskDate").val();
    var dateFormat = new Date(dateValue);
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: "/Reports/PartialReportByCompany",
        type: "POST",
        data: {
            __RequestVerificationToken: token,
            "dateValue": dateValue
        },
        cache: false,
        success: function (result) {
            $("#dt").text("ОЦЕНКА ЭФФЕКТИВНОСТИ ПЕРСОНАЛА - " + dateFormat.toLocaleDateString("ru-RU", options).charAt(0).toUpperCase() + dateFormat.toLocaleString("ru-RU", options).slice(1));
            var tableContent = $("#tableContent");
            tableContent.html(result);
        },
        error: function (XMLHttpRequest) {
            console.log(XMLHttpRequest);
        }
    });
    return false;
}
//This function for show and update DataTable ReportByDepartmentAverageMarkTable on View
function updateDepartmentAverageMarkTable() {
    var options = { month: 'long', year: 'numeric' };
    var token = $('input[name="__RequestVerificationToken"]').val();
    var dateValue = $("#taskDate").val().split(' ');
    var startDate = new Date(dateValue[0]);
    var endDate = new Date(dateValue[2]);

    if (startDate.getMonth() == endDate.getMonth()) {
        var dateParams = [dateValue[0], dateValue[0]];
        var onSuccessRes = function () {
            $("#dt").text("Отчет средних показателей по отделам - " + startDate.toLocaleDateString("ru-RU", options).charAt(0).toUpperCase() + startDate.toLocaleString("ru-RU", options).slice(1));
        }
    } else {
        var dateParams = [dateValue[0], dateValue[2]];
        var onSuccessRes = function () {
            $("#dt").text("Отчет средних показателей по отделам - " + startDate.toLocaleDateString("ru-RU", options).charAt(0).toUpperCase() + startDate.toLocaleString("ru-RU", options).slice(1)
                + " - " + endDate.toLocaleDateString("ru-RU", options).charAt(0).toUpperCase() + endDate.toLocaleString("ru-RU", options).slice(1));
        }
    }

    $.ajax({
        url: "/Reports/PartialReportByDepartmentAverageMark",
        type: "POST",
        data: {

            __RequestVerificationToken: token,
            "dateValue": dateParams
        },
        cache: false,
        success: function (result) {
            onSuccessRes();
            var tableContent = $("#tableContent");
            tableContent.html(result);
        },
        error: function (XMLHttpRequest) {
            console.log(XMLHttpRequest);
        }
    });    
    return false;
}
//This function for show and update DataTable ReportByConsolidated on View
function updateConsolidatedTable() {
    var options = { month: 'long', year: 'numeric' };
    var dateValue = $("#taskDate").val();
    var dateFormat = new Date(dateValue);
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: "/Reports/PartialReportByConsolidated",
        type: "POST",
        data: {
            __RequestVerificationToken: token,
            "dateValue": dateValue
        },
        cache: false,
        success: function (result) {
            $("#dt").text("Консолидированный анализ по оценке эффективности - " + dateFormat.toLocaleDateString("ru-RU", options).charAt(0).toUpperCase() + dateFormat.toLocaleString("ru-RU", options).slice(1));
            var tableContent = $("#tableContent");
            tableContent.html(result);
        },
        error: function (XMLHttpRequest) {
            console.log(XMLHttpRequest);
        }
    });
    return false;
}
//This function for show and update DataTable ReportByInstructionsDGon View
function updateInstructionsDGTable() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var dropDown = document.getElementById("departmentsDropdown");
    var departmentId = $("#departmentsDropdown").val();

    var dateValue = $("#taskDate").val();
    var dateFormat = new Date(dateValue);

    var options = { month: 'long', year: 'numeric' };
    //Check if DropDown is empty disabled button. 
    if (departmentId === "") {
        $("#downloadBtn").prop('disabled', true);
        $("#tableContent").empty();
        $("#DepartmentName").text(dropDown.options[dropDown.selectedIndex].text);
        $("#dt").text("Оценка выполнения поручений Генерального Директора по отчету о проделанной работе - " + dateFormat.toLocaleDateString("ru-RU", options).charAt(0).toUpperCase() + dateFormat.toLocaleString("ru-RU", options).slice(1));

    } else {
        $.ajax({
            url: "/Reports/PartialReportByInstructionsDG",
            type: "POST",
            data: {
                __RequestVerificationToken: token,
                "departmentId": departmentId,
                "dateValue": dateValue
            },
            cache: false,
            success: function (result) {
                $("#dt").text("Оценка выполнения поручений Генерального Директора по отчету о проделанной работе - " + dateFormat.toLocaleDateString("ru-RU", options).charAt(0).toUpperCase() + dateFormat.toLocaleString("ru-RU", options).slice(1));
                //If DropDown not empty written the select item on head of Table
                $("#DepartmentName").text(dropDown.options[dropDown.selectedIndex].text);
                var tableContent = $("#tableContent");
                tableContent.html(result);
                $("#downloadBtn").prop('disabled', false);
                //Check If DataTable is Empty disabled button
                if (result.length === "2") {
                    $("#downloadBtn").prop('disabled', true);
                }
            },
            error: function (XMLHttpRequest) {
                console.log(XMLHttpRequest);
            }
        });
    }
    return false;
}

function downloadReport(reportType) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var dateValue = $("#taskDate").val();
    var departmentId;
    var parentId;
    if ($("#departmentsDropdown").length > 0) {
        departmentId = $("#departmentsDropdown").val();
    } else {
        departmentId = "*";
    }
    window.location.href = "/Reports/ExportToExcel?departmentId=" + departmentId + "&reportType=" + reportType + "&dateValue=" + dateValue + "&parentId=" + parentId;
}

function downloadReportByDepartmentAverageMark(reportType) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var dateValue = $("#taskDate").val().split(" ");
    var departmentId;
    var parentId;
    if ($("#departmentsDropdown").length > 0) {
        departmentId = $("#departmentsDropdown").val();
    } else {
        departmentId = "*";
    }
    window.location.href = "/Reports/ExportToExcelDepartmentAverageMark?departmentId=" + departmentId + "&reportType=" + reportType + "&dateValue=" + [dateValue[0], dateValue[2]] + "&parentId=" + parentId;
}


function addTask(forDepartment = null, inputHtml = null) {
    $("#emptyTaskList").remove();

    var createArea = $("#createArea");

    if ($("#removeGeneratedHtml").length > 0) {
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
        if (inputHtml == null) {
            var generatedHtml = `
    <div class="generatedHtml form-group row" id="generatedHtml">
        <div class="col-md-5">
            <div class="row">
                <label class="control-label col-md-4" for="TaskName">Задача</label>
                <div class="col-md-8">
                    <input type="text" class="form-control text-box single-line" id="TaskName" name="TaskName" required />
                </div>
            </div>
        </div>
        <div class="col-md-5">
            <div class="row">
                <label class="control-label col-md-4" for="TaskDuration">Продолжительность</label>
                <div class="col-md-8">
                    <input type="text" class="form-control text-box single-line" id="TaskDuration" name="TaskDuration" required />
                </div>
            </div>
        </div>
        <div class="col-md-2">
            <button type="button" class="btn btn-success" id="submitTask" onclick="submitTask(${forDepartment})" title="Сохранить"><span class="oi oi-check" title="Сохранить" aria-hidden="true"></span></button>
            <button type="button" class="btn btn-danger" id="removeGeneratedHtml" onclick="removeGeneratedHtml()"><span class="oi oi-x" title="Удалить" aria-hidden="true"></span></button>
        </div>
    </div>
`;
        } else {
            var generatedHtml = inputHtml;
        }
        
        createArea.append(generatedHtml);
    }
}

function removeGeneratedHtml() {
    $("#generatedHtml").remove();
    $(".alert-warning").remove();
}

function submitTask(forDepartment = null) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var employeeId = $("#employeesDropdown").val();
    var taskName = $("#TaskName").val();
    var taskDuration = $("#TaskDuration").val();
    var taskDate = $("#taskDate").val();
    var createArea = $("#createArea");

    var warningMessageOnEmptyFields = `
    <div class="alert alert-warning alert-dismissible fade show" role="alert">
      <button type="button" class="close" data-dismiss="alert" aria-label="Close">
        <span aria-hidden="true">&times;</span>
      </button>
      Все поля для задачи обязательны к заполнению!
    </div>
`;
    if (taskName == "" || taskDuration == "") {
        createArea.prepend(warningMessageOnEmptyFields);
        return false;
    }
    var sendData = {
        __RequestVerificationToken: token,
        "taskName": taskName,
        "taskDuration": taskDuration,
        "taskDate": taskDate
    };

    if (forDepartment != null) {
        var departmentId = $("#departmentsDropdown").val();
        var targetUrl = "/TaskLists/CreateDepartmentTaskPost";
        sendData["departmentId"] = departmentId;
    }
    else {
        var employeeId = $("#employeesDropdown").val();
        var targetUrl = "/TaskLists/CreateTask";
        sendData["employeeId"] = employeeId;
    }

    $.ajax({
        url: targetUrl,
        type: "POST",
        data: sendData,
        cache: false,
        success: function (createdTask) {
            removeGeneratedHtml();
            $("#taskList").append(createdTask);
            addTask(forDepartment);
            $("#TaskName").focus();
        },
        error: function (XMLHttpRequest) {
            console.log(XMLHttpRequest);
        }
    });
    return false;
}

function submitEditTask(forDepartment = null) {
    var token = $('input[name="__RequestVerificationToken"]').val();

    var taskId = $("#generatedHtml").attr("data-taskid");
    var taskName = $("#TaskName").val();
    var taskDuration = $("#TaskDuration").val();
    var taskDate = $("#taskDate").val();
    var createArea = $("#createArea");

    var warningMessageOnEmptyFields = `
    <div class="alert alert-warning alert-dismissible fade show" role="alert">
      <button type="button" class="close" data-dismiss="alert" aria-label="Close">
        <span aria-hidden="true">&times;</span>
      </button>
      Все поля для задачи обязательны к заполнению!
    </div>
`;
    if (taskName == "" || taskDuration == "") {
        createArea.prepend(warningMessageOnEmptyFields);
        return false;
    }
    var sendData = {
        __RequestVerificationToken: token,
        "TaskListId": taskId,
        "TaskName": taskName,
        "TaskDuration": taskDuration,
        "TaskDate": taskDate,
        "CommentEmployees": ""
    };

    if (forDepartment != null) {
        var departmentId = $("#departmentsDropdown").val();
        var targetUrl = "/TaskLists/EditDepartmentTask";
        sendData["DepartmentId"] = departmentId;
    }
    else {
        var employeeId = $("#employeesDropdown").val();
        var targetUrl = "/TaskLists/Edit";
        sendData["EmployeeId"] = employeeId;
    }

    $.ajax({
        url: targetUrl,
        type: "POST",
        data: sendData,
        cache: false,
        success: function (createdTask) {
            var taskRow = $("#" + taskId);

            taskRow.find(".taskName").text(taskName);
            taskRow.find(".taskDuration").text(taskDuration);
            taskRow.find(".taskDate").text(taskDate);
            if ($("#employeesDropdown").length > 0) {
                var performerName = $("#employeesDropdown option:selected").text();                
            } else if ($("#departmentsDropdown").length > 0) {
                var performerName = $("#departmentsDropdown option:selected").text();
            }
            taskRow.find(".taskPerformer").text(performerName);

            removeGeneratedHtml();
        },
        error: function (XMLHttpRequest) {
            console.log(XMLHttpRequest);
        }
    });
    return false;
}

function saveOnEnter(forDepartment = null) {
    $("#createArea").keypress(function (e) {
        if (e.keyCode === 13) {
            if ($("#submitTask").length > 0) {
                submitTask(forDepartment);
            } else if ($("#editTask").length > 0) {
                submitEditTask(forDepartment);
            }
        }
    });
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
            $(`#${taskId}`).remove();
        },
        error: function (XMLHttpRequest) {
            console.log(XMLHttpRequest);
        }
    });
    return false;
}

function editTask(forDepartment = null, taskId) {
    var token = $('input[name="__RequestVerificationToken"]').val();

    var taskRow = $("#" + taskId);
    var taskName = taskRow.find(".taskName").text();
    var taskDuration = taskRow.find(".taskDuration").text();

    var generatedHtml = `
    <div class="generatedHtml form-group row" id="generatedHtml" data-taskid="${taskId}">
        <div class="col-md-5">
            <div class="row">
                <label class="control-label col-md-4" for="TaskName">Задача</label>
                <div class="col-md-8">
                    <input type="text" class="form-control text-box single-line" id="TaskName" name="TaskName" value="${taskName}" required />
                </div>
            </div>
        </div>
        <div class="col-md-5">
            <div class="row">
                <label class="control-label col-md-4" for="TaskDuration">Продолжительность</label>
                <div class="col-md-8">
                    <input type="text" class="form-control text-box single-line" id="TaskDuration" name="TaskDuration" value="${taskDuration}" required />
                </div>
            </div>
        </div>
        <div class="col-md-2">
            <button type="button" class="btn btn-success" id="editTask" onclick="submitEditTask(${forDepartment})" title="Сохранить"><span class="oi oi-check" title="Сохранить" aria-hidden="true"></span></button>
            <button type="button" class="btn btn-danger" id="removeGeneratedHtml" onclick="removeGeneratedHtml()"><span class="oi oi-x" title="Удалить" aria-hidden="true"></span></button>
        </div>
    </div>
`;

    addTask(forDepartment, generatedHtml);
}

function rate(id) {
    var skillMark = $("#" + id).find(".SkillMark").val();
    var effectivenessMark = $("#" + id).find(".EffectivenessMark").val();
    var disciplineMark = $("#" + id).find(".DisciplineMark").val();
    var timelinessMark = $("#" + id).find(".TimelinessMark").val();
    var averageMark = $("#" + id).find(".AverageMark");

    $.ajax({
        url: "/TaskLists/RateTask",
        type: "POST",
        data: {
            "taskId": id,
            "skillMark": skillMark,
            "effectivenessMark": effectivenessMark,
            "disciplineMark": disciplineMark,
            "timelinessMark": timelinessMark
        },
        cache: false,
        success: function (average) {
            averageMark.val(average);
        },
        error: function (XMLHttpRequest) {
            console.log(XMLHttpRequest);
        }
    });
    return false;
}

function rateDepartmentTask(taskId) {
    var averageMark = $("#" + taskId).find(".AverageMark").val();

    $.ajax({
        url: "/TaskLists/RateDepartmentTask",
        type: "POST",
        data: {
            "taskId": taskId,
            "averageMark": averageMark
        },
        cache: false,
        success: function (message) {
            $("#successMessage").text(message);
            $('#onSuccessModal').modal({
                show: true
            });
        },
        error: function (XMLHttpRequest) {
            console.log(XMLHttpRequest);
        }
    });
    return false;
}

function taskComment(taskId, forDepartment = false) {
    var taskRow = $("#comment_" + taskId);
    var saveBtnTemplate = `
    <button type="button" class="btn btn-success" onclick="submitComment('${taskId}')" title="Сохранить"><span class="oi oi-check" title="Сохранить" aria-hidden="true"></span></button>
`;

    if (forDepartment) {
        taskRow = $("#" + taskId);
        saveBtnTemplate = `
    <button type="button" class="btn btn-primary" onclick="submitComment('${taskId}', true)" title="Сохранить"><span class="oi oi-check" title="Сохранить" aria-hidden="true"></span></button>
`;
    }
    var commentField = taskRow.find(".comment-field");
    var oldCommentFieldText = commentField.text().trim();

    var submitButton = taskRow.find(".submit-button");

    var commentBtnTemplate = submitButton.html();

    var commentFieldTemplate = `
    <div class="row">
        <div class="col-md-12">
            <input type="text" name="comment" class="form-control comment-input" value="${oldCommentFieldText}" autofocus>
        </div>
    </div>
`;
    
    submitButton.html(saveBtnTemplate);
    commentField.html(commentFieldTemplate);

    taskRow.find('.comment-input').focus();
}

function submitComment(taskId, forDepartment = false) {
    var taskRow = $("#comment_" + taskId);
    var commentBtnTemplate = `
    <button type="button" class="btn btn-primary" onclick="taskComment('${taskId}')"><i class="fa fa-comment" aria-hidden="true" title="Комментировать" data-toggle="tooltip" data-placement="top"></i></button>
`;

    if (forDepartment) {
        taskRow = $("#" + taskId);
        var commentBtnTemplate = `
    <button type="button" class="btn btn-primary" onclick="taskComment('${taskId}', true)">Комментарий</button>
`;
    }

    var commentMessage = taskRow.find(".comment-input").val();
    

    $.ajax({
        url: "/TaskLists/CommentTask",
        type: "POST",
        data: {
            "taskId": taskId,
            "comment": commentMessage
        },
        cache: false,
        success: function (average) {
            taskRow.find(".comment-field").html("<b>" + commentMessage + "</b>");
            taskRow.find(".submit-button").html(commentBtnTemplate);
        },
        error: function (XMLHttpRequest) {
            console.log(XMLHttpRequest);
        }
    });
    return false;
}

function submitCommentOnEnter(forDepartment = false) {
    if (forDepartment) {
        $(".comment-field").keypress(function (e) {
            if (e.keyCode === 13) {
                submitComment($(this).attr("data-taskid"), true);
            }
        });
    } else {
        $(".task-row").keypress(function (e) {
            if (e.keyCode === 13) {
                submitComment(this.id.split('_')[1]);
            }
        });
    }
}

function submitMarkOnEnter() {
    $(".mark-field").keypress(function (e) {
        if (e.keyCode === 13) {
            rateDepartmentTask($(this).parent().attr('id'));
        }
    });
}

function updateAddTaskUrl(forDepartment) {
    var selectedDate = $("#taskDate").val();
    

    if (forDepartment) {
        $("#addTaskBtn")
            .attr("href", "/TaskLists/CreateDepartmentTask?taskDate=" + selectedDate);
    } else {
        $("#addTaskBtn")
            .attr("href", "/TaskLists/Create?taskDate=" + selectedDate);
    }
}