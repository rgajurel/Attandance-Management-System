$(document).ready(function () {
    LoadOrgainsation();
    $('#LeaveEntryList').off().on('click', '.chkbx', function ()
    {
        debugger;
        var checked = $(this).is(':checked');
        var grid = $('#LeaveEntryList').data().kendoGrid;
        var dataItem = grid.dataItem($(this).closest('tr'));
      
        dataItem.set('IsMonthRule', checked);      
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
   
    $("#OrganisationID").change(function () 
    {
        var organisationid = $("#OrganisationID").val();
        GetOrganisationLeaveType(organisationid);     


    });

    $("#Search").click(function (e)
    {
        if (!$('form#formLeaveEntry').data('unobtrusiveValidation').validate()) 
        {
            e.preventDefault();
            return false;
        }
        else
         {
            $("#LeaveEntryList").data("kendoGrid").dataSource.read();
        }       
      

    });

    $("#leaveEntryCancel").off().on('click', function (e)
    {
        ResetFormData();

    });   

    $("#leaveEntryAdd").off().on('click', function (e)
    {
        debugger;
        if (!$('form#formLeaveEntry').data('unobtrusiveValidation').validate())
        {
            e.preventDefault();
            return false;
        }
        else if (!$('form#LeaveEntryDays').data('unobtrusiveValidation').validate())
        {
             e.preventDefault();
             return false;
        }
        else
        {            
            e.preventDefault();
            var leaveEntry = $("#LeaveEntryList").data("kendoGrid").dataSource.data();
            $.ajax({
                url: "/Admin/LeaveEntry/SaveLeaveEntry",
                type: 'POST',
                data: { data: JSON.stringify(leaveEntry), Year: $("#YearID").val() },
                dataType: 'json',
                success: function (data)
                {
                   
                    ShowMessage(data.Message,true);
                },
                error: function (response)
                {
                    ShowMessage("Warning! Error Occured",false);
                }
            })

        }
    })

    $("#addTotalDays").off().on('click', function (e)
    {
        if (!$('form#LeaveEntryDays').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {           
          var  dataItem = $("#LeaveEntryList").data("kendoGrid").dataSource.data();
          for (i = 0; i < dataItem.length; i++)
          {
                firstItem = $('#LeaveEntryList').data().kendoGrid.dataSource.data()[i];
                firstItem["TotalDayInMonth"] = $("#TotalDayInMonth").val();
                firstItem["TotalDays"] = $("#TotalDays").val();                
          }
          $('#LeaveEntryList').data('kendoGrid').refresh();
        }

    })

     
   
})

function checkAll(ele)
{
     var item, row, checkbox;   
    var checked = $('.chkSelectAll').prop('checked'),
        grid = $("#LeaveEntryList").data("kendoGrid");
    if (grid.dataSource.data().length == 0)
    {
        return false;
    }
    else {      
             
        for (var i = 0; i < grid.dataSource.data().length; i++)
        {
            item = grid.dataSource.data()[i];         
     
             if (checked)
             {
                 item.set('IsMonthRule', checked);
                 grid.tbody.find("tr[data-uid='" + item.uid + "']")
               .addClass("k-alt k-state-selected")
               .find(".chkbx")
               .attr("checked", "checked");
               
             }
             else {
                 item.set('IsMonthRule', checked);
                 grid.tbody.find("tr[data-uid='" + item.uid + "']")
               .removeClass("k-alt k-state-selected")
               
             }
        }        
       
    }
}
function GetOrganisationLeaveType(organisation) {

    $("#LeaveTypeID").empty();
    $("#LeaveTypeID").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/LeaveEntry/GetLeaveTypeBasedOnOrganisation",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: organisation,
        },
        global:false,
        success: function (data)
        {

            jQuery.each(data, function (index, value)
            {               
                $("#LeaveTypeID").append('<option value=' + value.ID + '>' + value.LeaveTypeName + '</option>')
                    })

        },

        error: function (response)
        {

            ShowMessage("Warning! Error Occured",false);  //
        }



    })

}
function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
    document.getElementsByClassName("panel-title")[0].innerHTML = "Edit Salary Head";

    $.ajax({
        url: "/Admin/SalaryHeadings/EditSalaryHeadings",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result) {
            $("#ID").val(result.ID);
            $("#HeadName").val(result.HeadName);

            if (result.IsAdd == true) {
                $('#IsAdd').prop('checked', true);

            }
            if (result.IsAdd == false) {
                $('#IsAdd').prop('checked', false);

            }


            if (result.IsSaving == true) {
                $('#IsSaving').prop('checked', true);

            }
            if (result.IsSaving == false) {
                $('#IsSaving').prop('checked', false);

            }

            if (result.IsTax == true) {
                $('#IsTax').prop('checked', true);

            }
            if (result.IsTax == false) {
                $('#IsTax').prop('checked', false);

            }

            if (result.IsSalaryCalculatePoint == true) {
                $('#IsSalaryCalculatePoint').prop('checked', true);

            }
            if (result.IsSalaryCalculatePoint == false) {
                $('#IsSalaryCalculatePoint').prop('checked', false);

            }



        },
        error: function (result) {

            ShowMessage('Error Occured',false);
        }
    });

}
function ParamToLeaveEntryList(e)
{
    var grid = $("#LeaveEntryList").data("kendoGrid").dataSource;
    return {
        OrganisationID: $("#OrganisationID :selected").val() == "" ? -1 : $("#OrganisationID :selected").val(),
        LeaveTypeID: $("#LeaveTypeID :selected").val() == "" ? -1 : $("#LeaveTypeID :selected").val(),
        YearID: $("#YearID :selected").val() == "" ? -1 : $("#YearID :selected").val(),
        // TermID: $("#TermID").val() == "" ? -1 : $("#TermID").val(),      
        //pageSize: grid._pageSize,
        //pageNumber: grid._page
    };

}
function Delete(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    $(e.currentTarget).closest("tr").remove();

}

function Save(e)
{

    if (e.values && (e.values.TotalDayInMonth))
    {
        var grid = $("#LeaveEntryList").data("kendoGrid");
        var dataItem = grid.dataItem(grid.current().closest("tr"));

       $('#LeaveEntryList').data('kendoGrid').refresh();
        var totaldaysinMonth = e.values.TotalDayInMonth || e.model.TotalDayInMonth;      

        if (totaldaysinMonth < 0)
        {
            ShowMessage("DaysInMonth Cannot Be Less than 0",false);
            e.model.set("TotalDays", 0);
            e.model.set("TotalDayInMonth", 0);
            e.model.set("IsMonthRule", false);
        }
        else
        {
            var totaldaysinYear = totaldaysinMonth * 12;
            e.model.set("TotalDays", totaldaysinYear);           
           
        }
                
            $('#LeaveEntryList').data('kendoGrid').refresh();         
          }
       
}
function LoadOrgainsation() {
    $("#OrganisationID").empty();
    $("#OrganisationID").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/DropDown/GetOrganisationDropDown",
        type: 'POST',
        dataType: 'json',
        global: false,
        success: function (data) {
           
            jQuery.each(data, function (index, value) {
                $("#OrganisationID").append('<option value=' + value.ID + '>' + value.Name + '</option>')
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured",false);  //
        }



    })

}

function resetRowNumberLeaveEntry(e) {
    
    var rows = e.sender.tbody[0].rows;
    $(rows).each(function (e) {
        var grid = $("#LeaveEntryList").data("kendoGrid");
        var row = this;
        var dataItem = grid.dataItem(row);

        if (dataItem.IsMonthRule == true) {
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

