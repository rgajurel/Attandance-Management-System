$(document).ready(function () {
    var groupid;
    

    $(".create").off().on('click', function (e) {
        $("#hide").fadeIn(500);
        $("#show").hide();
    });
    $(".cancel").off().on('click', function (e) {

        $("#hide").hide();
        $("#show").fadeIn(500);
        ResetFormData();

    });


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
        $('#NepaliDateTo').val(AD2BS($('#DateTo').val()));
    });
    $("#OrganisationID").change(function () {
        var organisationid = $("#OrganisationID").val();
        GetUserGroup(organisationid, groupid);


    });

    $("#Create").off().on('click', function (e) {

        $("#organisationEventList").hide();
        $("#OrganisatinEventForm").show();

    });

    $("#Search").off().on('click', function (e) {

        $("#OrganisationEventList").data("kendoGrid").dataSource.read();
    })




    $("#organisationEventsCancel").off().on('click', function (e) {

        $("#organisationEventList").show();
        $("#OrganisatinEventForm").hide();
      
    });

  


    $("#organisationEventsSubmit").off().on('click', function (e) {

        if (!$('form#formOrganisationEvent').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else
        {
            var selected;
            selected = $("#GroupID").val();
           
            $.ajax({
                url: "/Admin/OrganisationEvents/SaveOrganisationEvents",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    EventName: $('#EventName').val(),                 
                    NotificationType: $('#NotificationType').val(),
                    OrganisationID: $('#OrganisationID').val(),
                    NepaliDateFrom: $('#NepaliDateFrom').val(),
                    DateFrom: $('#DateFrom').val(),
                    NepaliDateTo: $('#NepaliDateTo').val(),
                    DateTo: $('#DateTo').val(),
                    GroupArray: selected,
                    EventDescription: $('#EventDescription').val(),                  

                }),
                dataType: 'json',
                success: function (data) {
                    ResetFormData();
                    $("#hide").hide();
                    $("#show").fadeIn(500);
                    ShowMessage(data.Message,true);
                    $("#organisationEventList").show();
                    $("#OrganisatinEventForm").hide();
                    $("#OrganisationEventList").data("kendoGrid").dataSource.read();

                },
                error: function ()
                {
                    ShowMessage("Warning! Error Occured",false);
                }
            })
        }
    });


 

});

function ResetUserGroup() {
    var obj = [];
    $('option:selected').each(function () {
        obj.push($(this).index());
    });

    for (var i = 0; i < obj.length; i++) {
        $('#GroupID')[0].sumo.unSelectItem(obj[i]);
    }

}

function Init()
{
    $("#organisationEventList").show();
    $("#OrganisatinEventForm").hide();

}

function InitializeGroupID() {
    $('#GroupID').SumoSelect({
        okCancelInMulti: true,

    });

    $('#GroupID').prop("selectedIndex", -1);
    
}
function ParamToOrganisationEventsList(e) {
    var grid = $("#OrganisationEventList").data("kendoGrid").dataSource;
    return {
        NotificationTypeSearch: $("#NotificationTypeSearch :selected").val() == "" ? -1 : $("#NotificationTypeSearch :selected").val(),
        EventNameSearch: $("#EventNameSearch").val() == "" ? "" : $("#EventNameSearch").val(),
        OrganisationIDSearch: $("#OrganisationIDSearch").val() == "" ? -1 : $("#OrganisationIDSearch").val(),
        pageSize: grid._pageSize,
        pageNumber: grid._page

    };

}



function GetUserGroup(organisation, groupid) {
    $("#GroupID").empty();

    $.ajax({
        url: "/Admin/Notification/GetGroupBasedOnOrganisation",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: organisation,
        },
        global: false,
        success: function (data) {


            if (groupid !== null && typeof groupid !== 'undefined') {

                var group = groupid.split(",");

                jQuery.each(data, function (index, value) {
                    $("#GroupID").append('<option  value=' + value.ID + '>' + value.GroupName + '</option>');


                })
                InitializeGroupID();
                $('#GroupID')[0].sumo.reload();
                $('#GroupID')[0].sumo.unSelectAll();

                var selectbox = $('#GroupID')[0];
                for (var i = 0; i < group.length; i++) {
                    selectbox.sumo.selectItem(group[i]);
                }
            }
            else {
                jQuery.each(data, function (index, value) {
                   

                    $("#GroupID").append('<option value=' + value.ID + '>' + value.GroupName + '</option>')
                    InitializeGroupID();
                    $('#GroupID')[0].sumo.reload();
                });
            }



        },

        error: function (response) {

            ShowMessage("Warning! Error Occured",false);  //
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
            url: "/Admin/OrganisationEvents/DeleteOrganisationEvents",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result)
            {
                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message,true);
                ResetFormData();
                $("#OrganisationEventList").data("kendoGrid").dataSource.read();

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
        url: "/Admin/OrganisationEvents/EditOrganisationEvents",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result)
        {
            $("#hide").fadeIn(500);
            $("#show").hide();
            $("#organisationEventList").hide();
            $("#OrganisatinEventForm").show();
            $("#ID").val(result.ID);
            $("#EventName").val(result.EventName);
            $("#NotificationType").val(result.NotificationType);          
            $("#OrganisationID").val(result.OrganisationID);
            $("#DateFrom").val(ConvertDateObjectToDate(result.DateFrom))
            $("#NepaliDateFrom").val(ConvertDateObjectToDate1(result.NepaliDateFrom));
            $("#DateTo").val(ConvertDateObjectToDate(result.DateTo));
            $("#NepaliDateTo").val(ConvertDateObjectToDate1(result.NepaliDateTo));
            $("#EventDescription").val(result.EventDescription);
            GetUserGroup(result.OrganisationID, result.GroupID);

        },

        error: function (result) {

            ShowMessage('Error Occured',false);
        }
    });

}

function onAdditionalData() {
    return {
        text: $("#Name").val(),
        organisation: $("#OrganisationID :selected").val() == "" ? -1 : $("#OrganisationID :selected").val(),
    };
}
function EmployeeSelect(e) {
    var DataItem = this.dataItem(e.item.index());
    $("#EmployeeID").val(DataItem.ID);
    $("#EmployeeID").val("");
    $("#UserID").val(DataItem.UserID);

}

ConvertDateObjectToDate = function (dateObject) {
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


