$(document).ready(function () {
    $(".create").off().on('click', function (e) {
        $("#hide").fadeIn(500);
        $("#show").hide();
    });
    $(".cancel").off().on('click', function (e) {

        $("#hide").hide();
        $("#show").fadeIn(500);
        ResetFormData();

    });
       LoadOrgainsation();
    $("#OrganisationID").change(function () {
        var organisationid = $("#OrganisationID").val();
        GetOrganisationLeaveType(organisationid);
            });




    $("#Save").off().on('click', function (e) {

        if (!$('form#formAccumulativeLeave').data('unobtrusiveValidation').validate())
        {
            e.preventDefault();
            return false;
        }
        else {
            $.ajax({
                url: "/Admin/AccumulativeLeave/SaveAccumulativeLeave",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    OrganisationID: $('#OrganisationID').val(),
                    EmployeeID: $('#EmployeeID').val(),
                    LeaveTypeID: $('#LeaveTypeID').val(),
                    UserID: $('#UserID').val(),
                    Days: $('#Days').val(),
                    Name: $('#Name').val(),
                    YearID: $('#YearID').val()
                }),
                dataType: 'json',
                success: function (data) {
                    ResetFormData();
                    ShowMessage(data.Message);
                    $("#hide").hide();
                    $("#show").fadeIn(500);
                    $("#LeaveTypeID").val($("#LeaveTypeID option:first").val());
                    $("#AccumulativeLeaveList").data("kendoGrid").dataSource.read();
                    $("html, body").animate({ scrollTop: $(document).height() }, 200);
                }
            })
        }
    });


    $("#FieldFilter").keyup(function () {
       
        var value = $("#FieldFilter").val();
        $("#AccumulativeLeaveList").data("kendoGrid").dataSource.filter({
            logic: "or",
            filters: [
                {
                    field: "Name",
                    operator: "contains",
                    value: value
                },
               
            ]
        });


    });

});

function LoadOrgainsation()
{
    $("#OrganisationID").empty();
    $("#OrganisationID").append('<option value>--Organisation--</option>')
    $.ajax({
        url: "/Admin/DropDown/GetOrganisationDropDown",
        type: 'POST',
        dataType: 'json',       
        global: false,
        success: function (data)
        {
           
           
            jQuery.each(data, function (index, value)
            {
                $("#OrganisationID").append('<option value=' + value.ID + '>' + value.Name + '</option>')
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured");  //
        }



    })

}

function GetOrganisationLeaveType(organisation,leavetypeid) {

    $("#LeaveTypeID").empty();
    $("#LeaveTypeID").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/TakeLeave/GetAccumulativeLeaveTypeBasedOnOrganisation",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: organisation,
        },
        global:false,
        success: function (data) {
           
            jQuery.each(data, function (index, value)
            {
                if (value.ID == leavetypeid)
                {
                    $("#LeaveTypeID").append('<option selected value=' + value.ID + '>' + value.LeaveTypeName + '</option>')
                }
                else {
                    $("#LeaveTypeID").append('<option value=' + value.ID + '>' + value.LeaveTypeName + '</option>')
                }
                
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured");  //
        }



    })

}
function Delete(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
    $("#window").kendoWindow({
        modal: true
    });
    $("#window").data("kendoWindow").open().center();

    $("#yes").off().on('click', function (e) {

        $.ajax({
            url: "/Admin/BusInfo/DeleteBusInfo",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {
                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message);
                ResetFormData();
                $("#BusInfoGrid").data("kendoGrid").dataSource.read();
                $("html, body").animate({ scrollTop: 0, }, 1000);
                return false;
            }


        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}
function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();  

    $.ajax({
        url: "/Admin/AccumulativeLeave/EditAccumulativeLeave",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",     
        success: function (result)
        {
            $("#ID").val(result.ID);
            $("#OrganisationID").val(result.OrganisationID);
            GetOrganisationLeaveType(result.OrganisationID,result.LeaveTypeID);
            $("#EmployeeID").val(result.EmployeeID);
            $("#Name").val(result.Name);            
            $("#Days").val(result.Days);
            $("#UserID").val(result.UserID);
            $("#YearID").val(result.YearID);
            $("#hide").fadeIn(500);
            $("#show").hide();
            $("html, body").animate({ scrollTop: 0, }, 1000);
            return false;
        },

        error: function (result)
        {

            ShowMessage('Warning !! Error Occured');
        }
    });

}

function onAdditionalData()
{
    return {
        text: $("#Name").val(),
        organisation: $("#OrganisationID :selected").val() == "" ? -1 : $("#OrganisationID :selected").val(),
    };
}
function EmployeeSelect(e)
{

    var DataItem = this.dataItem(e.item.index());   
    $("#EmployeeID").val(DataItem.ID);   
    $("#UserID").val(DataItem.UserID);

}


