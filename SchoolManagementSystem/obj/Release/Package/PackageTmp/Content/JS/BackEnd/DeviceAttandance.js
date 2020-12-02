var isconnected;
$(document).ready(function () {

    LoadOrgainsation();
   // $("#refreshDevice").prop("disabled", true);
    $(".connect").off().on('click', function (e) {
        Calculate();        
    });
    $(".cancel").off().on('click', function (e) {

        $("#hide").hide();
        $("#show").fadeIn(500);
        ResetFormData();
    });

    $("#OrganisationIDSearch").change(function () {
        var organisationid = $("#OrganisationIDSearch").val();
        GetEmployeeBasedOnOrganisation(organisationid);

    });

    $('#takeleaveSearch').click(function () {
        $("#DeviceAttandanceListGrid").data("kendoGrid").dataSource.read();

    });
    
 
  
    $("#connectDevice").off().on('click', function (e) {

        if (!$('form#formConnectDevice').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {

            $.ajax({
                url: "/Admin/DeviceAttandance/ConnectDevice",
                type: 'POST',
                data: AddAntiForgeryToken({                    
                    IpAddress: $('#BiometricDevice_IpAddress').val(),
                    Port: $('#BiometricDevice_Port').val(),
                    ConnectDevice:$('#connectDevice').text()
                }),
                dataType: 'json',
                success: function (data) {
                   
                    if (data.operationStatus == 200) {
                        $("#refreshDevice").prop("disabled", false);
                        ShowMessage(data.message, true);
                        isconnected = true;
                        $("#connected").text(":Connected");
                        document.getElementById("connectDevice").innerHTML = "Disconnect";
                        
                    }
                    else if (data.operationStatus == 300) {
                        $("#connected").text(":Disconnected");
                        document.getElementById("connectDevice").innerHTML = "Connect";
                    }
                    else {
                        isconnected = false;
                        ShowMessage(data.message, true);
                    }
                    
                    





                }
            })
        }
    });

    $("#refreshDevice").off().on('click', function (e) {
       
        $.ajax({
            url: "/Admin/DeviceAttandance/PullData",
            type: 'POST',                
            dataType: 'json',
            data: {                
                IpAddress: $('#BiometricDevice_IpAddress').val(),
                Port: $('#BiometricDevice_Port').val(),
               
            },
            success: function (data)
            {
                debugger;
                if (data.operationStatus == 200)
                {
                    ShowMessage(data.message, true);                       

                }
                else
                {
                    ShowMessage(data.message, true);
                }

            }
        });
});
});


function LineItems_Databound(status) {
    if (status == "Absent") {
        return "<div style='background: #e54040;text-align:center;color:white'>" + status + " </div>";
    }
    else if (status == "Present") {
        return "<div style='background:#52e540;text-align:center;color:white'>" + status + " </div>";
    }
   
}
function ParamToDeviceAttandanceList(e) {
      
    var grid = $("#DeviceAttandanceListGrid").data("kendoGrid").dataSource;
    return {
        OrganisationIDSearch: $("#OrganisationIDSearch  :selected").val() == "" ? -1 : $("#OrganisationIDSearch :selected").val(),
        EmployerIDSearch: $("#EmployerIDSearch :selected").val() == "" ? -1 : $("#EmployerIDSearch :selected").val(),
        YearSearch: $("#YearSearch").val() == "" ? -1 : $("#YearSearch").val(),
        MonthSearch: $("#MonthSearch").val() == "" ? -1 : $("#MonthSearch").val(),
        DateSearch: $("#DateSearch").val() == "" ? -1 : $("#DateSearch").val(),
        pageSize: grid._pageSize,
        pageNumber: grid._page

    };

}
function resetRowNumberDeviceAttandance(e) {

    //$(".k-grid-Edit").find("span").addClass("fa fa-pencil");
    //$(".k-grid-Edit").removeClass("k-button");
    //$(".k-grid-Delete").find("span").addClass("fa fa-trash");
    //$(".k-grid-Delete").removeClass("k-button");
    //$(".k-grid-Details").find("span").addClass("fa fa-eye");
    //$(".k-grid-Details").removeClass("k-button");
    //$(".k-grid-Approve").find("span").addClass("fa fa-check");
    //$(".k-grid-Approve").removeClass("k-button");
    //$(".k-grid-Details").find("span").addClass("fa fa-eye");
    //$(".k-grid-Details").removeClass("k-button");


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
function GetEmployeeBasedOnOrganisation(organisation, employee) {



    $("#EmployerIDSearch").empty();
    $("#EmployerIDSearch").append('<option value>--Employer--</option>')
    $.ajax({
        url: "/Admin/OfficialLeave/GetEmployeeBaesdOrganisation",
        type: 'POST',
        dataType: 'json',
        data: {
            OrganisationID: organisation,

        },
        global: false,
        async: true,
        success: function (data) {

            jQuery.each(data, function (index, value) {
                

                $("#EmployerIDSearch").append('<option value=' + value.ID + '>' + value.Name + '</option>')


            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured", false);  //
        }



    })

}

function Calculate(e) {   
    $('#deviceConnectPop').modal({ show: true, backdrop: 'static', keyboard: false })
 
}

function LoadOrgainsation() {

 

    $("#OrganisationIDSearch").empty();
    $("#OrganisationIDSearch").append('<option value>--Organisation--</option>')
    $.ajax({
        url: "/Admin/DropDown/GetOrganisationDropDown",
        type: 'POST',
        dataType: 'json',
        global: false,
        async: true,
        success: function (data) {
            jQuery.each(data, function (index, value) {               

                $("#OrganisationIDSearch").append('<option value=' + value.ID + '>' + value.Name + '</option>')
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured", false);  //
        }



    })

}





