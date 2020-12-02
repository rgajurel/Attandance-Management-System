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

    $("#OrganisationID").change(function () {
        var organisationid = $("#OrganisationID").val();
        GetUserGroup(organisationid, groupid);


    });


    $("#Create").off().on('click', function (e) {

        $("#notificationList").hide();
        $("#Notification").show();

    });

    $("#notificationCancel").off().on('click', function (e) {
        $("#notificationList").show();
        $("#Notification").hide();
        $("#triggerdate").show();
        $("#expirydate").show();
    });
    $("#Search").off().on('click', function (e) {

        $("#NotificationsList").data("kendoGrid").dataSource.read();
    })


    $('#TriggerNow').change(function () {
        var triggered = this.checked;
        if (triggered) {
            $("#triggerdate").hide();
            $("#expirydate").hide();
        }
        else {
            $("#triggerdate").show();
            $("#expirydate").show();
        }
    });


    $("#notificationSubmit").off().on('click', function (e) {

        var isTriggered = false;
        var isTriggered = $('form#formNotification').find('#TriggerNow').is(':checked');
        if (isTriggered) {
            isTriggered = true;
        }

        if (!$('form#formNotification').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {
            var selected;
            selected = $("#GroupID").val();

            $.ajax({
                url: "/Admin/Notification/SaveNotification",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    Title: $('#Title').val(),
                    Link: $('#Link').val(),
                    NotificationType: $('#NotificationType').val(),
                    OrganisationID: $('#OrganisationID').val(),
                    TriggerNow: isTriggered,
                    TriggerDate: $('#TriggerDate').val(),
                    ExpiryDate: $('#ExpiryDate').val(),
                    GroupArray: selected,
                    Description: $('#Description').val(),
                    Status: $('#Status').val(),

                }),
                dataType: 'json',
                success: function (data) {
                  
                  
                   $("#hide").hide();
                   $("#show").fadeIn(500);
                   $('#GroupID')[0].sumo.reload();
                    ShowMessage(data.Message,true);
                    $("#notificationList").show();
                    $("#Notification").hide();
                    $("#NotificationsList").data("kendoGrid").dataSource.read();

                },
                error: function () {
                    ShowMessage("Warning! Error Occured",false);
                }
            })
        }
    });



});
    function ParamToNotificationList(e) {
        var grid = $("#NotificationsList").data("kendoGrid").dataSource;
        return {
            NotificationTypeSearch: $("#NotificationTypeSearch :selected").val() == "" ? -1 : $("#NotificationTypeSearch :selected").val(),
            TitleSearch: $("#TitleSearch").val() == "" ? "" : $("#TitleSearch").val(),       
            OrganisationIDSearch: $("#OrganisationIDSearch").val() == "" ? -1 : $("#OrganisationIDSearch").val(),
            pageSize: grid._pageSize,
            pageNumber: grid._page

        };

    }


    function ResetUserGroup() {
        var obj = [];
        $('option:selected').each(function ()
        {
            obj.push($(this).index());
        });

        for (var i = 0; i < obj.length; i++) {
            $('#GroupID')[0].sumo.unSelectItem(obj[i]);
        }

    }

    function Init()
    { 
        $("#notificationList").show();
        $("#Notification").hide();

    }

    function InitializeGroupID()
    {   
        $('#GroupID').SumoSelect({
            okCancelInMulti: true,
            placeholder: 'Select Group'
        });
        $('#GroupID').prop("selectedIndex", -1);
    

    }


    function GetUserGroup(organisation,groupid)
    {
        $("#GroupID").empty();  
   
        $.ajax({
            url: "/Admin/Notification/GetGroupBasedOnOrganisation",
            type: 'POST',
            dataType: 'json',
            data: {
                ID: organisation,
            },
            global: false,
            success: function (data)
            {          
            
           
                if ( groupid!==null && typeof groupid !== 'undefined')
                {

                    var group = groupid.split(",");

                    jQuery.each(data, function (index, value)
                    {                   
                        $("#GroupID").append('<option  value=' + value.ID + '>' + value.GroupName + '</option>');
                                               

                    })
                    InitializeGroupID();
                    $('#GroupID')[0].sumo.reload();
                    $('#GroupID')[0].sumo.unSelectAll();
            
                    var selectbox = $('#GroupID')[0];
                    for (var i = 0; i < group.length; i++)
                    {
                        selectbox.sumo.selectItem(group[i]);
                    }
                }
                else
                {
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
                url: "/Admin/Notification/DeleteNotification",
                data: { id: dataItem.ID },
                type: 'POST',
                dataType: 'json',
                success: function (result) {
                    $("#window").data("kendoWindow").close();
                    ShowMessage(result.Message,true);
                    ResetFormData();
                    $("#NotificationsList").data("kendoGrid").dataSource.read();
                              
                }


            });

        });

        $("#no").off().on('click', function (e) {

            $("#window").data("kendoWindow").close();
        });

    }
    function Edit(e)
    {
        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
        e.preventDefault();
   

        $.ajax({
            url: "/Admin/Notification/EditNotification",
            data: { id: dataItem.ID },
            type: "POST",
            dataType: "json",
            global:false,
            success: function (result)
            {
                $("#hide").fadeIn(500);
                $("#show").hide();
                $("#notificationList").hide();
                $("#Notification").show();
                $("#ID").val(result.ID);
                $("#Title").val(result.Title);
                $("#Link").val(result.Link);
                $("#NotificationType").val(result.NotificationType);
                $("#OrganisationID").val(result.OrganisationID);
                if (result.TriggerNow == true)
                {
                    $("#TriggerNow").prop("checked", true);
                    $("#triggerdate").hide();
                    $("#expirydate").hide();
                }
                $("#TriggerDate").val(result.tDate);
                $("#ExpiryDate").val(result.eDate);
                $("#Description").val(result.Description);
                GetUserGroup(result.OrganisationID, result.GroupID)
                      
            },

            error: function (result) {

                ShowMessage('Error Occured',true);
            }
        });

    }

    function onAdditionalData() {
        return {
            text: $("#Name").val(),
            organisation: $("#OrganisationID :selected").val() == "" ? -1 : $("#OrganisationID :selected").val(),
        };
    }
  

