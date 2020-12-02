$(document).ready(function ()
{

    $('#NepaliDate').nepaliDatePicker({
        ndpEnglishInput: 'Date',
        npdMonth: true,
        npdYear: true,
        npdYearCount: -25
    });
    
    $('#Date').change(function () {
        $('#NepaliDate').val(AD2BS($('#Date').val()));
    });

    

    $("#StudentsDailyAttandanceSearch").off().on('click', function (e)
    {
        if (!$('form#formStudentsDailyAttandanceEntry').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {
            $("#StudentsDailyAttadanceList").data("kendoGrid").dataSource.read();
            // $("#marksEntrySearch").hide();
        }

    })
  
    $('#StudentsDailyAttadanceList').off().on('click', '.chkbx', function ()
    {
       
        var checked = $(this).is(':checked');
        var grid = $('#StudentsDailyAttadanceList').data().kendoGrid;
        var dataItem = grid.dataItem($(this).closest('tr'));
        row = $(this).closest("tr");
        checkedIds[dataItem.SN] = checked;
        dataItem.set('IsAttend', checked);

        var view = $("#StudentsDailyAttadanceList").data("kendoGrid").dataSource.view();
        for (var i = 0; i < view.length; i++)
        {

            if (checkedIds[view[i].SN]) {
                grid.tbody.find("tr[data-uid='" + view[i].uid + "']")
                .addClass("k-alt k-state-selected")
                .find(".chkbx")
                .attr("checked", "checked");
            }
            else {
                grid.tbody.find("tr[data-uid='" + view[i].uid + "']")
                row.removeClass("k-alt k-state-selected");
            }
        }
    })
       
    $("#FacultyID").change(function () {

        var faculty = $("#FacultyID").val();
        var classes = $("#ClassID").val();
        GetSectionBasedOnClassAndFaculty(classes, faculty);

        // GetSectionBasedOnClass(classs);

    });
    $("#ClassID").change(function () {
        var classs = $("#ClassID").val();
        GetFacultyBasedOnClass(classs);

    });

    $("#StudentsDailyAttandanceSave").off().on('click', function (e)
    {
        if (!$('form#formStudentsDailyAttandanceEntry').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        } else {
            if ($("#StudentsDailyAttadanceList").data("kendoGrid").dataSource.total() == 0)
            {
                ShowMessage("Please Select At Least One Students");
            } else
            {                
                e.preventDefault();
                var studentdailylist = $("#StudentsDailyAttadanceList").data("kendoGrid").dataSource.data();
                $.ajax({
                    url: "/Admin/StudentsDailyAttandance/SaveStudentsDailyAttandance",
                    type: 'POST',
                    data: { data: JSON.stringify(studentdailylist),eng:$("#Date").val(),nep:$("#NepaliDate").val() },
                    dataType: 'json',
                    success: function (data)
                    {
                        ShowMessage(data.Message);                     
                        $("#StudentsDailyAttadanceList").data("kendoGrid").dataSource.read();

                    },
                    error: function (resonse)
                    {
                        ShowMessage("Warning !! Error Occured")
                    }
                })

            }
        }
    })

});


function checkAll(ele) {

    var checked = $('.chkSelectAll').prop('checked'), grid = $("#StudentsDailyAttadanceList").data("kendoGrid");
    for (var i = 0; i < grid.dataSource.data().length; i++) {
        var item = grid.dataSource.data()[i];
        var row = grid.element.find("tr[data-uid='" + item.uid + "']");
        var checkBox = row.find(".chkbx");
        if (!checkBox.prop('checked')) {
            checkBox.trigger("click");
        }
        if (!checked) {
            if (checkBox.prop('checked')) {
                checkBox.trigger("click");
            }
        }
    }
    if (!checked) {
        requisitionid = [];

    }
}
function GetFacultyBasedOnClass(classs, faculty) {
    $("#FacultyID").empty();
    $("#FacultyID").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/Students/GetFacultyBaseOnClass",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: classs,
        },
        global: false,
        success: function (data) {


            jQuery.each(data, function (index, value) {

                if (value.ID === faculty) {
                    $("#FacultyID").append('<option selected value=' + value.ID + '>' + value.Faculty + '</option>')
                }
                else {
                    $("#FacultyID").append('<option value=' + value.ID + '>' + value.Faculty + '</option>')
                }

            });


        }
    })

}

function GetSectionBasedOnClassAndFaculty(classs, faculty, section) {

    $("#Section").empty();
    $("#Section").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/Students/GetSectionBaseOnClassAndFaculty",
        type: 'POST',
        dataType: 'json',
        data: {
            ClassID: classs,
            FacultyID: faculty
        },
        global: false,
        success: function (data) {
            var sectionArray = data.Sections.split(',');
            jQuery.each(sectionArray, function (index, value) {
                if (value === section) {
                    $("#Section").append('<option selected value=' + value + '>' + value + '</option>')
                }
                else {
                    $("#Section").append('<option value=' + value + '>' + value + '</option>')
                }

            });

        }
    })

}

function ParamToAttandanceList(e) {
    var grid = $("#StudentsDailyAttadanceList").data("kendoGrid").dataSource;
    return {
        SessionID: $("#SessionID :selected").val() == "" ? -1 : $("#SessionID :selected").val(),
        ClassID: $("#ClassID :selected").val() == "" ? -1 : $("#ClassID :selected").val(),
        Section: $("#Section :selected").val() == "" ? "" : $("#Section :selected").val(),
        FacultyID: $("#FacultyID").val() == "" ? -1 : $("#FacultyID").val(),
        NepaliDate: $("#NepaliDate").val() == "" ? "" : $("#NepaliDate").val(),
        Date: $("#Date").val() == "" ? "" : $("#Date").val(),
       
    };

}



function Save(e) {

    if (e.values && (e.values.PresentDays)) {
        var grid = $("#StudentsAttadanceList").data("kendoGrid");
        var dataItem = grid.dataItem(grid.current().closest("tr"));


        $('#StudentsAttadanceList').data('kendoGrid').refresh();
        var totaldays = e.values.TotalDays || e.model.TotalDays;
        var presentdays = e.values.PresentDays || e.model.PresentDays;

        if (parseInt(presentdays) > parseInt(totaldays) || parseInt(presentdays) <= 0) {
            ShowMessage("Present Days is greater than " + totaldays + " Or Less than 0");
            // e.model.set("PresentDays", 0);
            // dataItem.set("PresentDays", "0");
            CallFunctionReloadData();
            $('#StudentsAttadanceList').data('kendoGrid').refresh();
            //    this.val(0);


        }
        else {
            e.model.set("IsAttend", true);
        }
    }
}


function resetRowNumberAttandanceEntryGrid(e) {
    var rows = e.sender.tbody[0].rows;
    $(rows).each(function (e)
    {
        var grid = $("#StudentsDailyAttadanceList").data("kendoGrid");
        var row = this;
        var dataItem = grid.dataItem(row);

        if (dataItem.IsAttend == true)
        {
            grid.tbody.find("tr[data-uid='" + dataItem.uid + "']")
           .addClass("k-alt k-state-selected gridselect")
             .find(".chkbx")
             .attr("checked", "checked");
        }
    })
    $(".k-grid-Edit").find("span").addClass("fa fa-pencil");
    $(".k-grid-Edit").removeClass("k-button");
    $(".k-grid-Delete").find("span").addClass("fa fa-trash");
    $(".k-grid-Delete").removeClass("k-button");
    $(".k-grid-Details").find("span").addClass("fa fa-eye");
    $(".k-grid-Details").removeClass("k-button");
    var grid = e.sender;
    if (grid.dataSource.total() == 0) {
        var colCount = 0;
        var columns = grid.columns;
        jQuery.each(columns, function (index) {
            if (!this.hidden) {
                colCount++;
            }
        });
        $(e.sender.wrapper)
            .find('tbody')
            .append('<tr class="kendo-data-row text-center"><td colspan="' + colCount + '" class="no-data">No Data Available</td></tr>');
    }
}

function onError(e, status)
{
    ShowMessage('Warning ! Error Occured');
}