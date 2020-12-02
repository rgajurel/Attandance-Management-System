$(document).ready(function ()
{

    $("#userGroupCancel").off().on('click', function (e)
    {
        ResetFormData();
        //$("#UserGroupList").data("kendoGrid").dataSource.read();
    })
      
    $("#Search").off().on('click', function (e) {

        $("#UserGroupList").data("kendoGrid").dataSource.read();
    })


    $("#userGroupSave").off().on('click', function (e) {

        if (!$('form#formUserGroup').data('unobtrusiveValidation').validate())
        {
            e.preventDefault();
            return false;
        }
        else {

            $.ajax({
                url: "/Admin/UserGroup/SaveUserGroup",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    GroupName: $('#GroupName').val(),
                    OrganisationID: $('#OrganisationID').val(),
                    StatusValue: $('#StatusValue').val(),

                }),
                dataType: 'json',
                success: function (data)
                {
                    ShowMessage(data.Message,true);
                    ResetFormData();
                    $("#OrganisationID").val($("#OrganisationID option:first").val());                 
                    $("#UserGroupList").data("kendoGrid").dataSource.read();
                  




                }
            })
        }
    });

    $("#FieldFilter").keyup(function () {
        var value = $("#FieldFilter").val();
        grid = $("#DepartmentGrid").data("kendoGrid");
        rowNumber = 0;
        if (value) {
            grid.dataSource.filter({ field: "DepartmentName", operator: "contains", value: value });

        } else {
            grid.dataSource.filter({});
        }
    });

    $("#userGroupSearch").off().on('click', function (e)
    {
        $("#UserGroupList").data("kendoGrid").dataSource.read();      
    });
})

function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
   

    $.ajax({
        url: "/Admin/UserGroup/EditUserGroup",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",
        global: false,
        success: function (result)
        {
           
            $("#ID").val(result.ID);
            $("#GroupName").val(result.GroupName);
            $("#OrganisationID").val(result.OrganisationID);
            $('#StatusValue').val(result.StatusValue);
        },
        error: function (result)
        {

            ShowMessage('Error Occured',false);
        }
    });

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
            url: "/Admin/UserGroup/DeleteUserGroup",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {

                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message,true);
                ResetFormData();
                $("#UserGroupList").data("kendoGrid").dataSource.read();             

            }
        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}

function ParamToUserGroupList(e) {
    var grid = $("#UserGroupList").data("kendoGrid").dataSource;
    return {
        StatusValue: $("#StatusValue :selected").val() == "" ? -1 : $("#StatusValue :selected").val(),       
        GroupName: $("#GroupName").val() == "" ? "" : $("#GroupName").val(),
        OrganisationID: $("#OrganisationID").val() == "" ? -1 : $("#OrganisationID").val(),
        pageSize: grid._pageSize,
        pageNumber: grid._page
        
    };

}


