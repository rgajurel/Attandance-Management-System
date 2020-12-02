$(function () {
    Init();
    $("div#roleContainer").find('input#saveRole').on('click', function ()
    {
        SaveMenuRole();
    });
    $("div#roleContainer").find('input.user-group-item').on('click', function ()
    {
        var multiItem = $(this).parent('td').attr('data-val');
        var data = $(this).attr('data');
        var arr = Array();
        if (multiItem.length > 0) {
            arr = multiItem.split(',');
        }
        if ($(this).prop('checked')) {
            arr.push(data);
        }
        else {
            arr.splice(arr.indexOf(data), 1);
        }
        $(this).parent('td').attr('data-val', arr.toString());
    });
    $("div#roleContainer").find("select.all-roles-list").on('change', function () {
        $("div#roleContainer").find('input.user-group-item:checked').prop('checked', false);
        $("div#roleContainer").find('td.menu-options').attr('data-val', '');
        SetMenuRole();
    });
    $("input#saveRole").click(function () { });
    $("div#roleContainer").find('input.add-to-dropdown').click(function ()
    {

        $('#formRole').find('.input-validation-error').addClass('input-validation-valid');
        $('#formRole').find('.input-validation-error').removeClass('input-validation-error');

        $('#formRole').find('.field-validation-error').addClass('field-validation-valid');
        $('#formRole').find('.field-validation-error').removeClass('field-validation-error');

        $('#formRole').find('#RoleID').val('0');
        $('#formRole').find('#Name').val('');
        $('#customPopupTitle').text('Add Role');
        $('#customPopupDialog').modal('show');


    });

    $("div#roleContainer").find('input.edit-to-dropdown').click(function ()
    {

        $('#formRole').find('.input-validation-error').addClass('input-validation-valid');
        $('#formRole').find('.input-validation-error').removeClass('input-validation-error');

        $('#formRole').find('.field-validation-error').addClass('field-validation-valid');
        $('#formRole').find('.field-validation-error').removeClass('field-validation-error');

        var roleName = $(".all-roles-list option:selected").text();
        var roleID = $(".all-roles-list option:selected").val();

        $('#formRole').find('#Name').val(roleName);
        $('#formRole').find('#RoleID').val(roleID);

        $('#customPopupTitle').text('Edit Role');
        $('#customPopupDialog').modal('show');


    });

    $('#customPopupDialog').find('button.ok').on('click', function ()
    {
       
         var value = $('input.add-role').val();
        var ele = $("div#roleContainer");
        //   console.log(ele.find('select.all-roles-list:has(option:contains(' + value + '))').length);
        if (ele.find('select.all-roles-list:has(option:contains(' + value + '))').length > 0)
        {
            var errorArray = {};
            errorArray["Name"] = 'Role Name Already Exists';
            $('#formRole').validate().showErrors(errorArray);
            return;
        }


        $("div#roleContainer").find('select.all-roles-list').val(0);

        var roleID = $('form#formRole').find('#RoleID').val();
        var roleName = $('form#formRole').find('#Name').val();
        if (roleID == "0")
        {

            if (ele.find('select.all-roles-list option[value=0]').length === 0)
            {
                $("div#roleContainer").find('select.all-roles-list').append('<option value="0">' + value + '</options>');
            }
            else {
                $("div#roleContainer").find('select.all-roles-list option[value=0]').text(value);
            }
            $("div#roleContainer").find('select.all-roles-list').val('0');
            $("div#roleContainer").find('input.user-group-item:checked').prop('checked', false);
            $("div#roleContainer").find('td.menu-options').attr('data-val', '');
        } else
        {
            $("div#roleContainer").find('select.all-roles-list option[value=' + roleID + ']').text(roleName);
            $("div#roleContainer").find('select.all-roles-list').val(roleID);
        }
        $('div#customPopupDialog').modal('hide');

    });
});

function Init() {
    SetMenuRole();
    var parentHeight = screen.height - 470;
    $('div.role-menu').attr('style', "height:" + parentHeight + "px");
}
function LoadMenu() {
    $.ajax({
        type: "get",
        url: "/Admin/Role/",
        //   url: "/api/admin/role/",
        data: "",
        success: function (data) {
            if (data !== null && typeof data.result !== 'undefined' && data.result) {
                var role = data.roleList;
                var roleHtml = '';
                for (var i = 0; i < role.length; i++) {
                    roleHtml += '<option value="' + role[i].RoleID + '">' + role[i].Name + '</option>'
                }
                $("div#roleContainer").find('select.all-roles-list').html(roleHtml);
            }
        }
        //,
        //error: function (jqXHR, textStatus) {
        //    if (jqXHR.redirect) {
        //        alert(jqXHR.redirect);
        //        // data.redirect contains the string URL to redirect to
        //        window.location.href = jqXHR.redirect;
        //    }
        //}
    });
}

function LoadRole() {
    $.ajax({
        type: "get",
        //  url: "/api/admin/role",
        url: "/Admin/Role",
        data: "",
        success: function (data) {
            if (data !== null && typeof data.result !== 'undefined' && data.result) {
                var role = data.roleList;
                var roleHtml = '';
                for (var i = 0; i < role.length; i++) {
                    roleHtml += '<option value="' + role[i].RoleID + '">' + role[i].Name + '</option>'
                }
                $("div#roleContainer").find('select.all-roles-list').html(roleHtml);
            }
        }
        //,
        //error: function (jqXHR, textStatus) {
        //    if (jqXHR.redirect) {
        //        alert(jqXHR.redirect);
        //        // data.redirect contains the string URL to redirect to
        //        window.location.href = jqXHR.redirect;
        //    }
        //} 
    });
}

function SetMenuRole() {

    var menus = $('div#roleContainer').find('select.all-roles-list option:selected').attr('data-val');

    if (menus != 'undefined' && menus !== null && menus.length > 0) {
        menus = unescape(menus);
        menus = JSON.parse(menus);
        $('div#roleContainer').find('table td.menu-options').each(function ()
        {
            var menuID = parseInt($(this).attr('data-id'));
            var ele = $(this);
            for (var j = 0; j < menus.length; j++) {
                if (menus[j].MenuID === menuID) {
                    ele.attr('data-val', menus[j].Options);
                    var optionsList = menus[j].Options.split(',');
                    ele.find('input[type=checkbox]').each(function ()
                    {
                        if (optionsList.includes($(this).attr('data')))
                        {
                            $(this).prop('checked', true);
                        }
                      
                    });
                }
            }
        });
    }
}

function SaveMenuRole() {
    var menu = Array();
    $('div#roleContainer').find('table td.menu-options input[type=checkbox]:checked').parents('td.menu-options').each(function (i, e) {
        var obj = new Object();
        obj.MenuID = parseInt($(this).attr('data-id'));
        obj.Options = $(this).attr('data-val');
        menu.push(obj);
    });
    var ele = $('div#roleContainer');
    $.ajax({
        type: "post",
        url: "/Admin/UserRole/SaveRole",        
        data: AddAntiForgeryToken({
            Name: ele.find('select.all-roles-list option:selected').text(),
            RoleID: ele.find('select.all-roles-list').val(),
            Menus: JSON.stringify(menu)
        }),
        success: function (data)
        {
            if (data !== null && typeof data.Result !== 'undefined' && data.Result)
            {
                $('div#roleContainer').find('select.all-roles-list option:selected').attr('data-val', escape(JSON.stringify(menu)));
                if (ele.find('select.all-roles-list').val() == '0')
                {
                    location.reload();

                }
                ShowMessage("Role Added Successfully",true);
            }

        }
        //,
        //error: function (jqXHR, textStatus) {
        //    if (jqXHR.redirect) {
        //        alert(jqXHR.redirect);
        //        // data.redirect contains the string URL to redirect to
        //        window.location.href = jqXHR.redirect;
        //    }
        //}
    });
}

function AddAntiForgeryToken(data) {
    data.__RequestVerificationToken = $('form input[name=__RequestVerificationToken]').val();
    return data;
};