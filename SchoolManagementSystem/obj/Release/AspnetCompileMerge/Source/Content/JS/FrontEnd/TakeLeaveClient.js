
var id, employeeid, organisationid;
$(document).ready(function ()
{
    CheckDate();
    $('#NepaliDateFrom').val(AD2BS($('#DateFrom').val()));
    $('#NepaliDateTo').val(AD2BS($('#DateTo').val()));

    $('#NepaliDateFrom').nepaliDatePicker({
        ndpEnglishInput: 'DateFrom',
        npdMonth: true,
        npdYear: true,
        npdYearCount: -25
    });

    $('#DateFrom').change(function () {
        $('#NepaliDateFrom').val(AD2BS($('#DateFrom').val()));
    });

    $('#NepaliDateTo').nepaliDatePicker({
        ndpEnglishInput: 'DateTo',
        npdMonth: true,
        npdYear: true,
        npdYearCount: -25
    });

    $('#DateTo').change(function () {
        $('#NepaliDateTo').val(AD2BS($('#DateFrom').val()));
    });
    $("#leavelist").show();
      

 
    $('#takeleaveSearch').click(function () {
        $("#TakeLeaveListGridClient").data("kendoGrid").dataSource.read();

    });
    $(".cancel").off().on('click', function () {
        ResetFormData();
        $("#hide").hide();
        $("#leavelist").show();
        InitialDate();

    });
    
    LoadOrgainsation();
    LoadEmployee();
    LoadLeaveDaysmaster();
    LoadAdminAndSuperAdmin();
    GetLeaveTypeBasedOnEmployee();

    $("#EmployeeID").change(function ()
    {
        var employeeid = $("#EmployeeID").val();
        if (employeeid != null || employeeid != 'undefined' || employeeid != "")
        {
            GetLeaveTypeBasedOnEmployee(employeeid);
        }
       
        

    });

    $("#LeaveDaysID").change(function ()
    {      
       var datavalue = $(this).find(':selected').attr('data-val');
      var  datefrom = $('#DateFrom').val().split("-");
      var dateto = $('#DateTo').val().split("-");
      if (datefrom == "" || dateto == "")
      {
          ShowMessage("Warning !! Please Enter DateFrom and DateTo");
          return false;
      }
     
      var d1 = new Date(datefrom);
      var d2 = new Date(dateto);
          isWeekend = false;
          var i = 1;
          while (d1 < d2)
          {
           var day = d1.getDay();
           isWeekend = (day === 6);
           if (isWeekend)
           {               
               if (confirm("It Contains Saturday"))
               { //      
                   i = i - 1;
               }
              
           } // return immediately if weekend found
           d1.setDate(d1.getDate() + 1);
           i++;
       }
     
       $('#Days').val(i * datavalue);
       var totaldays = $('#Days').val();
       if (totaldays > parseFloat($('#RemainingLeave').val()))
       {
           ShowMessage("Warning!! You Cannot Take "+totaldays+"  Leave");
           $('#Days').val("");
           return false;
       }      
    

    });


    $("#Show").off().on('click', function (e) {
        $("#leavelist").hide();
        $("#hide").show();
        InitialDate();
       
    });

    $("#takeLeaveSubmit").off().on('click', function (e) {

        if (!$('form#formTakeLeave').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        } else if ($('#Days').val()==""|| parseFloat($('#RemainingLeave').val()==""))
        {
            ShowMessage("Warning!! You Cannot Take Leave");
            $('#Days').val("");
            return false;
        }
        else {
            $.ajax({
                url: "/Client/TakeLeave/SaveTakeLeave",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    OrganisationID: $('#OrganisationID').val(),
                    EmployeeID: $('#EmployeeID').val(),
                    LeaveTypeID: $('#LeaveTypeID').val(),                    
                    DateFrom: $('#DateFrom').val(),
                    DateTo: $('#DateTo').val(),
                    NepaliDateFrom: $('#NepaliDateFrom').val(),
                    NepaliDateTo: $('#NepaliDateTo').val(),
                    Year: $('#Year').val(),
                    Month: $('#Month').val(),
                    Days: $('#Days').val(),
                    LeaveDaysID: $('#LeaveDaysID').val(),
                    RemainingLeave: $('#RemainingLeave').val(),
                    ApprovedBy: $('#ApprovedBy').val(),
                    NotificationType: $('#NotificationType').val()
                }),
                dataType: 'json',
                success: function (data)
                {
                    ResetFormData();
                    ShowMessage(data.Message,true);
                    $("#leavelist").show();
                    $("#hide").hide();
                    $("#TakeLeaveListGridClient").data("kendoGrid").dataSource.read();
                   
                },
                error: function () {
                    ShowMessage("Warning !! Error Occured",false);
                }
            })
        }
    });

    $("#calculateremainingleave").off().on('click', function (e) {
                     
            $.ajax({
                url: "/Admin/TakeLeave/CalculateRemainingLeave",
                type: 'POST',
                data:{
                    OrganisationID: $('#OrganisationID').val(),
                    EmployeeID: $('#EmployeeID').val(),
                    LeaveTypeID: $('#LeaveTypeID').val(),
                    Year: $('#Year').val(),
                    Month: $('#Month').val(),
                },
                dataType: 'json',
                global:false,
                success: function (data)
                {
                    
                    $('#RemainingLeave').val(parseFloat(data));
                 
                                                  
                   
                },
                error: function () {
                    ShowMessage("Warning !! Error Occured");
                }
            })
      
    });

    $(".ok").off().on('click', function (e)
    {
        ApproveLeave(id, employeeid, organisationid);      

    });

   

});

function LineItems_Databound(status) {
    if (status == "Rejected") {
        return "<div style='background: #e54040;text-align:center;color:white'>" + status + " </div>";
    }
    else if (status == "Pending") {
        return "<div style='background:#52e540;text-align:center;color:white'>" + status + " </div>";
    }
    else if (status == "Approved") {
        return "<div style='background: skyblue;text-align:center;color:white'>" + status + " </div>";
    }
}

function ApproveLeave(id,employeeid,organisationid)
{    
    $.ajax({
        url: "/Admin/TakeLeave/ApproveLeave",
        type: 'POST',
        data: {
            status: $('select#status').val(),
            notificationtype: $('select#notificationtype').val(),
            id: id,
            employeeid: employeeid,
            organisationid: organisationid
        },
        dataType: 'json',
        global: false,
        success: function (data)
        {
            ShowMessage(data.Message);
            $('#customPopupDialog').modal('hide');
            $("#TakeLeaveListGrid").data("kendoGrid").dataSource.read();

        },
        error: function ()
        {
            ShowMessage("Warning !! Error Occured");
        }
    })
}



function LoadOrgainsation()
{
   
    $("#OrganisationID").empty();  

    $("#OrganisationIDSearch").empty();
    $("#OrganisationIDSearch").append('<option value>--Organisation--</option>')
    $.ajax({
        url: "/Admin/DropDown/GetOrganisationDropDown",
        type: 'POST',
        dataType: 'json',
        global: false,
        async:true,
        success: function (data) {           
            jQuery.each(data, function (index, value) {
                $("#OrganisationID").append('<option value=' + value.ID + '>' + value.Name + '</option>')

                $("#OrganisationIDSearch").append('<option value=' + value.ID + '>' + value.Name + '</option>')
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured");  //
        }



    })

}

function LoadEmployee() {
    $("#EmployeeID").empty();
    //$("#EmployeeID").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/DropDown/GetLoginEmployee",
        type: 'POST',
        dataType: 'json',
        async:true,
        global: false,
        success: function (data) {
            jQuery.each(data, function (index, value)
            {
                $("#EmployeeID").append('<option value=' + value.ID + '>' + value.Name + '</option>')
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured");  //
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

            ShowMessage("Warning! Error Occured");  //
        }



    })

}

function resetRowNumberTakeLeave(e)
{
   
        var rows = e.sender.tbody[0].rows;
    $(rows).each(function (e) {
        var grid = $("#TakeLeaveListGridClient").data("kendoGrid");
        var row = this;
        var dataItem = grid.dataItem(row);
        if (dataItem.Statuss == "Approved")
        {
            grid.tbody.find("tr[data-uid='" + dataItem.uid + "']")
           .addClass("k-alt k-state-selected gridselect")            
        }
    })

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

function ParamToTakeLeaveList(e) {
    var grid = $("#TakeLeaveListGridClient").data("kendoGrid").dataSource;
    return {               
        LeaveTypeIDsearch: $("#LeaveTypeIDsearch").val() == "" ? -1 : $("#LeaveTypeIDsearch").val(),
        YearSearch: $("#YearSearch").val() == "" ? -1 : $("#YearSearch").val(),
        MonthSearch: $("#MonthSearch").val() == "" ? -1 : $("#MonthSearch").val(),
        StatusSearch: $("#StatusSearch").val() == "" ? -1 : $("#StatusSearch").val(),
        pageSize: grid._pageSize,
        pageNumber: grid._page

    };

}


function GetLeaveTypeBasedOnEmployee(employeeid){
    $("#LeaveTypeID").empty();
    $("#LeaveTypeID").append('<option value>--Select--</option>')
    $("#LeaveTypeIDsearch").empty();
    $("#LeaveTypeIDsearch").append('<option value>--Leave Type--</option>')
    $.ajax({
        url: "/Admin/DropDown/GetLeaveTypeBasedOnEmployee",
        type: 'POST',
        dataType: 'json',
        data:{employeeid:employeeid},
        async:true,
        global: false,
        success: function (data) {
            jQuery.each(data, function (index, value)
            {
                $("#LeaveTypeID").append('<option value=' + value.ID + '>' + value.Name + '</option>');
                $("#LeaveTypeIDsearch").append('<option value=' + value.ID + '>' + value.Name + '</option>');
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured");  //
        }



    })
}
function LoadAdminAndSuperAdmin() {
   
    $("#ApprovedBy").empty();
    
    $.ajax({
        url: "/Admin/DropDown/GetSuperAdminAndAdmin",
        type: 'POST',
        dataType: 'json',
        global: false,
        async: true,
        success: function (data) {
            jQuery.each(data, function (index, value)
            {
                $("#ApprovedBy").append('<option value='+value.ID+'>' + value.Name + '</option>')

                
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured");  //
        }



    })

}



ConvertDateObjectToDate = function (dateObject)
{
    var dateString = dateObject.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var date = month + "-" + day + "-" + year;
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

function CalculateRemainingLeave(OrganisationID,EmployeeID,LeaveTypeID,Year,Month,Days)
{
    $.ajax({
        url: "/Admin/TakeLeave/CalculateRemainingLeave",
        type: 'POST',
        data: {
            OrganisationID: OrganisationID,
            EmployeeID:EmployeeID,
            LeaveTypeID:LeaveTypeID,
            Year: Year,
            Month: Month
        },
        dataType: 'json',
        global: false,
        success: function (data) {
           
            $('#RemainingLeave').val(parseFloat(data)+parseFloat(Days));



        },
        error: function () {
            ShowMessage("Warning !! Error Occured");
        }
    })
}


