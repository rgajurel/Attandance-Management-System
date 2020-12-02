var rowNumber = 0;
var wnd;
var selectedUserRow = [];
var ImageError = false;
var IsSuperUser = false;
var IsAdmin = false;
var IsClientUser = false;
var IsStudentUser = false;
IsParentUser = false;

$(function () {
    Init();


    $("#btnCancelUpdateUser").off().on('click', function (e)
    {
        $("#showUsers").show();
        $("#saveContainer").hide();
        ResetFormData();
        $("#ID").val("0");
    })

    $(".add").off().on('click', function (e)
    {
        ResetFormData();
        $("#showUsers").hide();
        $("#saveContainer").show();
    })

    $("#OrganisationID").change(function ()
    {
        var organisationid = $("#OrganisationID").val();
       
        IsSuperUser = $('div#saveContainer').find('#IsSuperAdmin').is(':checked');
        if (IsSuperUser)
        {
            IsSuperUser = true;
        }
               
        IsAdmin = $('div#saveContainer').find('#IsAdmin').is(':checked');
        if (IsAdmin)
        {
            IsAdmin = true;
        }

        IsClientUser = $('div#saveContainer').find('#IsClientUser').is(':checked');
        if (IsClientUser) {
            IsClientUser = true;
        }

        IsStudentUser = $('div#saveContainer').find('#IsStudentUser').is(':checked');
        if (IsStudentUser) {
            IsStudentUser = true;
        }
        IsParentUser = $('div#saveContainer').find('#IsParentUser').is(':checked');
        if (IsParentUser) {
            IsParentUser = true;
        }

        if (IsSuperUser == false && IsAdmin == false && IsClientUser == false && IsStudentUser == false && IsParentUser==false)
       {
            ShowMessage("Please Check One CheckBox",false);

            $("#OrganisationID").val($("#OrganisationID option:first").val());
           
      }
        
        //GetOrganisationLeaveType(organisationid);


    });
     

    $('#userGroupTab2').on('click', 'input[type=checkbox]', function ()
    {
        
        var checked = 0;
        $('div#saveContainer').find('p.user-group-list input:checked').each(function () {
            checked++;
            if (checked > 0)
            {
                $('div#saveContainer').find("#errorMessageUserSave").text('');
                $('div#saveContainer').find("#userGroup-tabLink2").removeClass('Error');
            }
            return false;
        });
    });

    $('#userRoleTab2').on('click', 'input[type=checkbox]', function ()
    {
       
        var checked = 0;
        $('div#saveContainer').find('p.user-role-list input:checked').each(function ()
        {
           
            checked++;
            if (checked > 0)
            {           

                $('div#saveContainer').find("#errorMessageUserRoleSave").text('');
                $('div#saveContainer').find("#userRole-tabLink2").removeClass('Error');
            }
            return false;
        });
    });




    $('#UsersGrid').on('click', '.chkbox', function () {
        var checked = $(this).is(':checked');
        var grid = $('#UsersGrid').data().kendoGrid;
        var dataItem = grid.dataItem($(this).closest('tr'));
        if (checked) {

            selectedUserRow.push(dataItem.UserName);
        } else {
            selectedUserRow.splice($.inArray(dataItem.UserName, selectedUserRow), 1);
        }
        if (selectedUserRow.length > 0) {
            $('.deleteOption').removeClass('hide');
        } else {
            $('.btnSelect').prop('checked', false);
            $('.deleteOption').addClass('hide');
        }

    });

    $('#SearchUsers').click(function () {
        $("#UserList").data("kendoGrid").dataSource.read();

    });

   

    $('div#saveContainer').find('button.save-user').click(function (e)
    {        
       
        if (IsSuperAdmin == false && IsAdmin == false && IsClientUser == false && IsStudentUser == false && IsParentUser==false) {

            ShowMessage("Please Check CheckBox",false);
            return false;
        }
       
        var userGroupIDs = [];
        var userRoleIDs = [];
        $('div#saveContainer').find('p.user-group-list input:checked').each(function ()
        {
            userGroupIDs.push($(this).attr('data-id'));
        });
        $('div#saveContainer').find('p.user-role-list input:checked').each(function () {
            userRoleIDs.push($(this).attr('data-id'));
        });      
       
       
      
        if ($('#formUserSave').data('unobtrusiveValidation').validate() && userGroupIDs.length > 0 && userRoleIDs.length > 0)
        {
            SaveUser(0, userGroupIDs, userRoleIDs);
        } else
        {
            
             e.isDefaultPrevented();
            
            if ($('div#saveContainer').find(".input-validation-error").length > 0)
            {                
                $('div#saveContainer').find("#userCrendentials-tabLink2").addClass('Error');
            }
            if (userGroupIDs <= 0)
            {
                $('div#saveContainer').find("#userGroup-tabLink2").addClass('Error');
                $('div#saveContainer').find("#errorMessageUserSave").text("Atleast one usergroup required.");
            }
            if (userRoleIDs <= 0)
            {
                $('div#saveContainer').find("#userRole-tabLink2").addClass('Error');
                $('div#saveContainer').find("#errorMessageUserRoleSave").text("Atleast one role required.");
            }
            return false;
        }
    });
   
});

function ParamToUsersList(e) {
    var grid = $("#UserList").data("kendoGrid").dataSource;
    return {
        SearchStatus: $("#SearchStatus :selected").val() == "" ? -1 : $("#SearchStatus :selected").val(),
        NameSearch: $("#NameSearch").val() == "" ? "" : $("#NameSearch").val(),
        UserNameSearch: $("#UserNameSearch").val() == "" ? "" : $("#UserNameSearch").val(),
        OrganisationIDSearch: $("#OrganisationIDSearch").val() == "" ? -1 : $("#OrganisationIDSearch").val(),
        pageSize: grid._pageSize,
        pageNumber: grid._page

    };

}

function onAdditionalData()
{
    IsSuperUser = $('div#saveContainer').find('#IsSuperAdmin').is(':checked');
    if (IsSuperUser) {
        IsSuperUser = true;
    }

    IsAdmin = $('div#saveContainer').find('#IsAdmin').is(':checked');
    if (IsAdmin) {
        IsAdmin = true;
    }

    IsClientUser = $('div#saveContainer').find('#IsClientUser').is(':checked');
    if (IsClientUser) {
        IsClientUser = true;
    }

    IsStudentUser = $('div#saveContainer').find('#IsStudentUser').is(':checked');
    if (IsStudentUser) {
        IsStudentUser = true;
    }

    IsParentUser = $('div#saveContainer').find('#IsParentUser').is(':checked');
    if (IsParentUser) {
        IsParentUser = true;
    }
    return {
        text: $("#Name").val(),
        organisation: $("#OrganisationID :selected").val() == "" ? -1 : $("#OrganisationID :selected").val(),
        IsSuperUser: IsSuperUser,
        IsAdmin:IsAdmin,
        IsClientUser: IsClientUser,
        IsStudentUser: IsStudentUser,
        IsParentUser: IsParentUser
    };
}
function EmployeeSelect(e)
{    
    var DataItem = this.dataItem(e.item.index());
   
    $("#Email").val(DataItem.Email);
    $("#EmployeeID").val(DataItem.ID);   
    $("#UserID").val(DataItem.UserID);

}
function SaveUser(id, userGroupIDs, userRoleIDs)
{
    

    var ele = $('div#saveContainer');
    var isValid = $('#formUserSave').data('unobtrusiveValidation').validate();  
    
    if (isValid) {
        var formData = new FormData();
        formData.append('ID', ele.find('input#ID').val());
        formData.append('Name', ele.find('input#Name').val());
        formData.append('UserName', ele.find('input#UserName').val());
        formData.append('IsSuperAdmin', $('div#saveContainer').find('#IsSuperAdmin').is(':checked'));
        formData.append('IsAdmin', $('div#saveContainer').find('#IsAdmin').is(':checked'));
        formData.append('IsParentUser', $('div#saveContainer').find('#IsParentUser').is(':checked'));
        formData.append('IsClientUser', $('div#saveContainer').find('#IsClientUser').is(':checked'));
        formData.append('IsStudentUser', $('div#saveContainer').find('#IsStudentUser').is(':checked'));
        formData.append('Password', ele.find('input#Password').val());
        formData.append('OrganisationID', ele.find('select#OrganisationID').val());
        formData.append('ConformPassword', ele.find('input#ConformPassword').val());
        formData.append('EmployeeID', ele.find('input#EmployeeID').val());
        formData.append('Email', ele.find('input#Email').val());
        formData.append('UserGroupID', userGroupIDs.join(","));
        formData.append('UserID', ele.find('input#UserID').val());
        formData.append('RoleID', userRoleIDs.join(","));//ele.find('select.all-roles-list').val()
        formData.append('Status', ele.find('select#Status').val());
        formData.append('__RequestVerificationToken', $('form input[name=__RequestVerificationToken]').val());

        $.ajax({
            type: "post",
            dataType: "json",
            url: '/Admin/Users/UserSave',
            data: formData,
            contentType: false,
            processData: false,                
            success: function (data)
            {                          
                ShowMessage(data.Message,true);
                $("#UserList").data("kendoGrid").dataSource.read();
                $("#showUsers").show();
                $("#saveContainer").hide();
            },
            error: function (jxhr, textStatus)
            {
                ShowMessage("Error Occured",false);
            }
        });
    
    }
    else
    {
         return false;
     }
}



function LoadUserGroup() {
    $.ajax({
        type: "get",
        url: "/Admin/Users/GetUserGroup",
        data: "",
        success: function (data) {
            if (data !== null && typeof data !== 'undefined' && data)
            {
                var group = data;
                var groupHtml = '';
                for (var i = 0; i < group.length; i++) {
                    groupHtml += '<input class="user-group-item" data-id="' + group[i].ID + '" type="checkbox" /> ' + group[i].GroupName + '<br />';
                }
                $("div#addContainer,div#saveContainer").find('p.user-group-list').html(groupHtml);
            }
        }
        ,
        error: function (jqXHR, textStatus) {
          
        }
    });
}
function LoadUserRole() {
    $.ajax({
        type: "get",
        url: "/Admin/Users/GetUserRole",
        data: "",
        success: function (data) {
            if (data !== null && typeof data !== 'undefined' && data) {
                var roleInfo = data;
                var roleHtml = '';
                for (var i = 0; i < roleInfo.length; i++) {
                    roleHtml += '<input class="user-role-item" data-id="' + roleInfo[i].RoleID + '" type="checkbox" /> ' + roleInfo[i].Name + '<br />';
                }
                $("div#addContainer,div#saveContainer").find('p.user-role-list').html(roleHtml);
            }
        },
        error: function (jqXHR, textStatus) {
           
        }
    });
}



function Init()
{
    $("#showUsers").show();
    $("#saveContainer").hide();

    LoadUserGroup();
    LoadUserRole();

    wnd = $("#modalWindow").kendoWindow({
        title: "Confirm",
        modal: true,
        visible: false,
        resizable: false,
        center: true,
        width: 400
    }).data("kendoWindow");

   }

function AddAntiForgeryToken(data) {
    data.__RequestVerificationToken = $('form input[name=__RequestVerificationToken]').val();
    return data;
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
            url: "/Admin/Users/DeleteUser",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {

                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message, true);
                ResetFormData();
                $("#UserList").data("kendoGrid").dataSource.read();

            }
        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}

function onDatabound(e) {
    rowNumber = 0;
    $(".k-grid-Edit").find("span").addClass("fa fa-pencil");
    $(".k-grid-Edit").removeClass("k-button");

    $(".k-grid-Delete").find("span").addClass("fa fa-trash");
    $(".k-grid-Delete").removeClass("k-button");
    //var pageSizes = LoadGridRecordPerPage(RecordPerPage, RecordPerPage, 5);
    //var pageSizearr = [];
    //if (pageSizes.length > 0) {
    //    $.each(pageSizes, function (val, size) {
    //        pageSizearr.push({ text: size, value: size });
    //    });
    //} else {
    //    pageSizearr = [10, 20, 30, 50, 80];
    //}

    //$('.k-pager-sizes select[data-role="dropdownlist"]').data('kendoDropDownList').setDataSource(new kendo.data.DataSource({ data: pageSizearr }));


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

function renderNumber(data) {
    return ++rowNumber;
}

function renderRecordNumber(data) {
    var page = parseInt($("#UsersGrid").data("kendoGrid").dataSource.page()) - 1;
    var pagesize = $("#UsersGrid").data("kendoGrid").dataSource.pageSize();
    return parseInt(rowNumber + (parseInt(page) * parseInt(pagesize)));
}
function Edit(e)
{
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));   
    $('div#saveContainer').find("#errorMessageUserSave").text("");
    $('div#saveContainer').find("#userCrendentials-tabLink2").removeClass('Error');
    $('div#saveContainer').find("#userGroup-tabLink2").removeClass('Error');
    $('div#saveContainer').find("#userRole-tabLink2").removeClass('Error');
      
    $.ajax({       
        url: '/Admin/Users/EditUser',
        type: "post",
        dataType: "json",
        data: AddAntiForgeryToken({ id: dataItem.ID }),      
        success: function (userInfo)
        {
            $("#showUsers").hide();
            $("#saveContainer").show();
           
            if (userInfo !== null && typeof userInfo !== 'undefined')
            {

               

                $("#ID").val(userInfo.ID);              
                if (userInfo.IsSuperAdmin == true) {
                    $('#IsSuperAdmin').prop('checked', true);

                }

                if (userInfo.IsAdmin == true) {
                    $('#IsAdmin').prop('checked', true);

                }

                if (userInfo.IsClientUser == true) {
                    $('#IsClientUser').prop('checked', true);

                }

                if (userInfo.IsParentUser == true) {
                    $('#IsParentUser').prop('checked', true);

                }
                if (userInfo.IsStudentUser == true) {
                    $('#IsStudentUser').prop('checked', true);

                }
               
                var ele = $('div#saveContainer');          

                ele.find('select#OrganisationID').val(parseInt(userInfo.OrganisationID));
                $("#OrganisationID").attr("disabled",true);
                ele.find('input#Name').val(userInfo.Name);
                ele.find('input#Email').val(userInfo.Email);
                              

                ele.find('input#UserName').val(userInfo.UserName);
                ele.find('input#UserID').val(userInfo.UserID);

                ele.find('input#EmployeeID').val(userInfo.EmployeeID);               
               
                ele.find('select#Status').val(userInfo.Status);

                ele.find('input#Password').val(userInfo.Password);//.prop("disabled",true);
                ele.find('input#ConformPassword').val(userInfo.ConformPassword);//.prop('disabled',true);

                                             
              

                var userGroupIDs = userInfo.UserGroupID.split(',');
                if (userGroupIDs.length > 0) {
                    ele.find('p.user-group-list input').each(function () {
                        $(this).prop('checked', false);

                        if (userGroupIDs.includes($(this).attr('data-id'))) {
                            $(this).prop('checked', true);
                        }

                        
                    });
                }

                var userRoleIDs = userInfo.RoleID.split(',');
                if (userRoleIDs.length > 0) {
                    ele.find('p.user-role-list input').each(function ()
                    {
                        $(this).prop('checked', false);

                        if (userRoleIDs.includes($(this).attr('data-id')))
                        {
                            $(this).prop('checked', true);
                        }
                        

                    });
                }


            }
        }
        ,
        error: function (jqXHR, textStatus) {
            ShowMessage("Warning! Error Occured",false);
        }
    });

}






