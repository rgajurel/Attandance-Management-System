var checkedIds = {};
$(document).ready(function () {
        
    LoadOrgainsation();
    InitialDate();
    CheckDate();
    $('#NepaliDateFrom').val(AD2BS($('#DateFrom').val()));
    $('#NepaliDateTo').val(AD2BS($('#DateTo').val()));
    $('#NepaliDateFrom').nepaliDatePicker({
        ndpEnglishInput: 'DateFrom',
        npdMonth: true,
        npdYear: true,
        npdYearCount: -25
    });

    $('#NepaliDateTo').nepaliDatePicker({
        ndpEnglishInput: 'DateTo',
        npdMonth: true,
        npdYear: true,
        npdYearCount: -25
    });

    $('#Date').change(function () {
        $('#NepaliDate').val(AD2BS($('#Date').val()));
    });

    $('#Date').attr("disabled", true);

    $("#StudentsDailyAttandanceSearch").off().on('click', function (e) {
        if (!$('form#formStudentsDailyAttandanceEntry').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }        
        else {
            
            CallDescriptionFunction($("#OrganisationID").val(), $("#Month").val(), $("#Year").val(), $("#DateFrom").val(), $("#DateTo").val())
            $("#ManagePublicHolidaysListGrid").data("kendoGrid").dataSource.read();
            // $("#marksEntrySearch").hide();
        }

    })

    $("#marksSaveRecords").off().on('click', function (e)
    {
        if (!$('form#formStudentsTotalAttandanceEntry').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }       
            else if ($("#ManagePublicHolidaysListGrid").data("kendoGrid").dataSource.total() == 0) {
                ShowMessage("Please Select At Least One Employee",false);
                e.preventDefault();
                return false;
            }

            else {               

                var dataItem;
                dataItem = $("#ManagePublicHolidaysListGrid").data("kendoGrid").dataSource.data();               

                for (i = 0; i < dataItem.length; i++)
                {
                    firstItem = $('#ManagePublicHolidaysListGrid').data().kendoGrid.dataSource.data()[i];
                    if (firstItem["IsAttend"] == false)
                    {
                        firstItem["Days"] = 0;
                    }

                    if (firstItem["IsAttend"] == true)
                    {
                        firstItem["Days"] = 1;
                    }                    
                   
                }
                $('#ManagePublicHolidaysListGrid').data('kendoGrid').refresh();
                e.preventDefault();
                var studentdailylist = $("#ManagePublicHolidaysListGrid").data("kendoGrid").dataSource.data();
                $.ajax({
                    url: "/Admin/ManagePublicHolidayAndSaturday/SaveManagePublicHoliday",
                    type: 'POST',
                    data: { data: JSON.stringify(studentdailylist), description: $("#Description").val(), datefrom: $("#DateFrom").val(), dateto: $("#DateTo").val() },
                    dataType: 'json',
                    success: function (data) {
                        ShowMessage(data.Message,false);
                        $("#ManagePublicHolidaysListGrid").data("kendoGrid").dataSource.read();

                    },
                    error: function (resonse) {
                        ShowMessage("Warning !! Error Occured",false)
                    }
                })

            }
    })

    $('#ManagePublicHolidaysListGrid').off().on('click', '.chkbx', function () {

        var checked = $(this).is(':checked');
        var grid = $('#ManagePublicHolidaysListGrid').data().kendoGrid;
        var dataItem = grid.dataItem($(this).closest('tr'));

        dataItem.set('IsAttend', checked);
        if (checked) {
            grid.tbody.find("tr[data-uid='" + dataItem.uid + "']")
                    .addClass("k-alt k-state-selected")
                    .find(".chkbx")
                    .attr("checked", "checked");
        }
        else {
            grid.tbody.find("tr[data-uid='" + dataItem.uid + "']")
                .removeClass("k-alt k-state-selected");
        }
    })
    
   
   
});

function CallDescriptionFunction(organisationid,month,year,datefrom,dateto)
{
    $.ajax({
        url: "/Admin/ManagePublicHolidayAndSaturday/GetDescription",
        type: 'POST',
        data:{
            OrganisationID: organisationid,
            Month: month,
            Year: year,
            DateFrom: datefrom,
            DateTo:dateto
        },
        dataType: 'json',
        success: function (data) {
            if (data != null || data != "")
            {
                $("#Description").val(data.Description)
            }
            else {
                $("#Description").val("");
            }
                     




        }
    })
}
function checkAll(ele) {
    var item, row, checkbox;
    var checked = $('.chkSelectAll').prop('checked'),
        grid = $("#ManagePublicHolidaysListGrid").data("kendoGrid");
    if (grid.dataSource.data().length == 0) {
        return false;
    }
    else {

        for (var i = 0; i < grid.dataSource.data().length; i++)
        {
            item = grid.dataSource.data()[i];

            if (checked) {
                item.set('IsAttend', checked);
                grid.tbody.find("tr[data-uid='" + item.uid + "']")
              .addClass("k-alt k-state-selected")
              .find(".chkbx")
              .attr("checked", "checked");

            }
            else {
                item.set('IsAttend', checked);
                grid.tbody.find("tr[data-uid='" + item.uid + "']")
              .removeClass("k-alt k-state-selected")

            }
        }
        $('#ManagePublicHolidaysListGrid').data('kendoGrid').refresh();

    }
}
function ParamToManageHolidayList(e) {
    var grid = $("#ManagePublicHolidaysListGrid").data("kendoGrid").dataSource;
    return {
        OrganisationID: $("#OrganisationID :selected").val() == "" ? -1 : $("#OrganisationID :selected").val(),
        Month: $("#Month :selected").val() == "" ? -1 : $("#Month :selected").val(),
        Year: $("#Year :selected").val() == "" ? "" : $("#Year :selected").val(),      
        datefrom: $("#DateFrom").val() == "" ? "" : $("#DateFrom").val(),
        DateTo: $("#DateTo").val() == "" ? "" : $("#DateTo").val(),
    };

}

function onError(e, status) {
    ShowMessage('Warning ! Error Occured',false);
}



function LoadOrgainsation() {
   
    $("#OrganisationID").empty();
    $("#OrganisationID").append('<option value>--Select--</option>')

   
    $.ajax({
        url: "/Admin/DropDown/GetOrganisationDropDown",
        type: 'POST',
        dataType: 'json',
        global: false,
        async: true,
        success: function (data) {
            jQuery.each(data, function (index, value) {
                $("#OrganisationID").append('<option value=' + value.ID + '>' + value.Name + '</option>');
              
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured",false);  //
        }



    })

}

function LoadLeaveDaysmaster() {
    $("#LeaveDaysID").empty();
    $("#LeaveDaysID").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/DropDown/GetLeaveDaysMaster",
        type: 'POST',
        dataType: 'json',
        async: true,
        global: false,
        success: function (data) {
            jQuery.each(data, function (index, value) {
                $("#LeaveDaysID").append('<option data-val=' + value.DataValue + ' value=' + value.ID + '>' + value.Name + '</option>')
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured",false);  //
        }



    })

}

function resetRowNumberManagePublicHolidayLeave(e) {
    debugger;
    var rows = e.sender.tbody[0].rows;
    $(rows).each(function (e) {
        var grid = $("#ManagePublicHolidaysListGrid").data("kendoGrid");
        var row = this;
        var dataItem = grid.dataItem(row);

        if (dataItem.IsAttend == true)
        {
            grid.tbody.find("tr[data-uid='" + dataItem.uid + "']")
           .addClass("k-alt k-state-selected")
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
    $(".k-grid-Approve").find("span").addClass("fa fa-check");
    $(".k-grid-Approve").removeClass("k-button");
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

ConvertDateObjectToDate = function (dateObject) {
    var dateString = dateObject.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var date = year + "-" + month + "-" + day;
    return date;
};

ConvertDateObjectToDate1 = function (dateObject) {

    var dateString = dateObject.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var date = year + "-" + month + "-" + day;
    return date;
};




