var rowNumber = 0;
var categoryType = "";
var identifier = 0;
$(document).ready(function () {
    //GetCourseCategoryType();
    //LoadStatusIdentifier();
    //LoadUserGroupDropDown();
    Init();

    $("#btnSearchCategory").click(function () {
        var grid = $("#CategoryTreeGrid").data("kendoGrid");
        grid.dataSource.page(1);
    });

    $("#btnCreateCategory").click(function () {
        $('#formHeading').text('Add Quiz Question Category');
        $('#btnSave').val('Submit');
        $('#btnCancel').val('Cancel');
        ResetFormData();
        $('img#ImgMediaManagementImage_coverImage').attr('src', '').attr('style', 'display:none');
        $('form#formCategory').find('label#lblPhotoPath_coverImage').text('');
        //   $('#UserGroup')[0].sumo.unSelectAll();
        UnSelectUserGroup();
        $(".sumo_UserGroup li").removeClass('selected');
        $("#CategoryTreeID").val(0);
        // $("#CategoryType").val(categoryType);
        // $('#IsPublic').prop('checked', false);
        $('#ParentCategoryID').prop('disabled', false);

        ShowFormAndHideList();
        //LoadUserGroupDropDown();
    });

    $("#btnSave").on('click', function (e) {
        if (!$('#formCategory').data('unobtrusiveValidation').validate()) {
            e.isDefaultPrevented();
            var errorArray = {};
            var a = 0;
            //if ($('.ddlMultiSlectBox').val() != null) {
            //    a = $('.ddlMultiSlectBox').val().length;
            //}

            //if (a <= 0) {
            //    errorArray["UserGroup"] = 'User Group Required';
            //}
            $('#formCategory').validate().showErrors(errorArray);
            return false;
        }
        else {
            var formData = $('#formCategory').serialize();
            //var isPublic = false;
            //var checked = $('form#formCategory').find('#IsPublic').is(':checked');
            //if (checked) {
            //    isPublic = true;
            //}
            var userGroupArray = [];
            //var userGroupArray = $('#UserGroup').val();

            $.ajax({
                url: '/Admin/EntranceQuestionCategory/CategoryTreeSave',
                type: 'POST',
                dataType: 'json',
                data: AddAntiForgeryToken({
                    CategoryTreeID: $('form#formCategory').find('#CategoryTreeID').val(),
                    // CategoryType: $('form#formCategory').find('input#CategoryType').val(),
                    CategoryName: $('form#formCategory').find('input#CategoryName').val(),
                    ParentCategoryID: $('form#formCategory').find('select#ParentCategoryID').val(),
                    userGroupArray: userGroupArray,
                    StatusValue: $('form#formCategory').find('select#StatusValue').val(),
                    Image: $('form#formCategory').find('img#ImgMediaManagementImage_coverImage').attr('src')
                }),
                success: function (data) {
                    ShowAlertMessage(data.ErrorOccured, data.Message);
                    LoadCategoryGrid();
                    LoadParentDropDown();
                }, beforeSend: function () {
                    //loadingNow($('div#divCategoryForm'), true);
                },
                complete: function () {
                    //loadingNow($('div#divCategoryForm'), false);
                },
                error: function () {
                    //loadingNow($('div#divCategoryForm'), false);
                }
            });

            ResetFormData();
            $("li.selected").removeClass("selected");

            ShowListAndHideForm();

        }

    });

    $('#btnCancel').on('click', function (e) {
        e.isDefaultPrevented();
        ResetFormData();
        //$('#UserGroup')[0].sumo.unSelectAll();
        UnSelectUserGroup();
        ShowListAndHideForm();
    });

    $("#btnReset").on('click', function (e) {
        e.preventDefault();
        $("#txtSearchKeyword").val('');
        $("#ddlStatus").val('');
        LoadCategoryGrid();
    });

});

function UnSelectUserGroup() {
    var num = $('select.ddlMultiSlectBox option').length;
    for (var i = 0; i < num; i++) {
        $('select.ddlMultiSlectBox')[0].sumo.unSelectItem(i);
    }
}

function EditCategory(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    $('#formHeading').text('Edit Quiz Question Category');
    $('#btnSave').val('Save');
    $('#btnCancel').val('Close');
    $('img#ImgMediaManagementImage_coverImage').attr('src', '');
    $('form#formCategory').find('label#lblPhotoPath_coverImage').text('');

    var data = {
        categoryTreeID: dataItem.CategoryTreeID,
    };

    $.ajax({
        url: '/Admin/EntranceQuestionCategory/CategoryTreeInfoByID',
        type: "post",
        dataType: "json",
        data: AddAntiForgeryToken(data),
        success: function (QuizQuestionInfo) {
            $("#CategoryTreeID").val(QuizQuestionInfo.data.CategoryTreeID);
            $("#ParentCategoryID").val(QuizQuestionInfo.data.ParentCategoryID).prop('disabled', true);
            // $('#ParentCategoryID')
            $("#CategoryName").val(QuizQuestionInfo.data.CategoryName);
            //   $('#IsPublic').prop('checked', QuizQuestionInfo.data.IsPublic);
            $("#StatusValue").val(QuizQuestionInfo.data.StatusValue);
            // $("#CategoryType").val(QuizQuestionInfo.data.CategoryType);
            //$('#UserGroup')[0].sumo.unSelectAll();
            //var userGroupArray = QuizQuestionInfo.data.UserGroup.split(",");
            //var selectbox = $('#UserGroup')[0];
            //for (var i = 0; i < userGroupArray.length; i++) {
            //    selectbox.sumo.selectItem(userGroupArray[i]);
            //}
            if (QuizQuestionInfo.data.Image != null) {
                $('form#formCategory').find('label#lblPhotoPath_coverImage').text(QuizQuestionInfo.data.Image);
                $('img#ImgMediaManagementImage_coverImage').attr('src', QuizQuestionInfo.data.Image).attr('style', 'dispaly:block');
            }


            ShowFormAndHideList();
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

function DeleteCategory(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    popupWindow.center().open();

    $("button#Yes").off().on('click', function (e) {
        e.preventDefault();

        var data = {
            categoryTreeID: dataItem.CategoryTreeID,
        };

        $.ajax({
            url: '/Admin/EntranceQuestionCategory/CategoryTreeDelete',
            type: "post",
            dataType: "json",
            data: AddAntiForgeryToken(data),
            success: function (data) {
                ShowAlertMessage(data.ErrorOccured, data.Message);
                LoadCategoryGrid();
                ShowListAndHideForm();
            }, beforeSend: function () {
                //loadingNow($('div#divCategoryList'), true);
            },
            complete: function () {
                //loadingNow($('div#divCategoryList'), false);
            },
            error: function () {
                //loadingNow($('div#divCategoryList'), false);
            }
        });

        popupWindow.close();
    });

    $("button#No").off().on('click', function (e) {
        e.preventDefault();
        popupWindow.close();
    });
}

function onDatabound(e) {
    rowNumber = 0;
    $(".k-grid-Edit").attr('title', 'Edit');
    $(".k-grid-Delete").attr('title', 'Delete');

    $(".k-grid-Edit").find("span").addClass("fa fa-pencil");
    $(".k-grid-Edit").removeClass("k-button");

    $(".k-grid-Delete").find("span").addClass("fa fa-trash");
    $(".k-grid-Delete").removeClass("k-button");

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

    var pageSizes = [10, 20, 30, 50, 80];;
    var pageSizearr = [];
    if (pageSizes.length > 0) {
        $.each(pageSizes, function (val, size) {
            pageSizearr.push({ text: size, value: size });
        });
    } else {
        pageSizearr = [10, 20, 30, 50, 80];
    }

    $('.k-pager-sizes select[data-role="dropdownlist"]').data('kendoDropDownList').setDataSource(new kendo.data.DataSource({ data: pageSizearr }));

}
function CheckModified(data) {
    if (data == "-") {
        data = "-";
    }
    else {
        data = kendo.toString(new Date(data), CustomDateFormat.replace("{", "").replace("}", "").replace("0:", ""));
    }
    return data;
}

function renderNumber(data) {
    return ++rowNumber;
}

function renderRecordNumber(data) {
    var page = parseInt($("#CategoryTreeGrid").data("kendoGrid").dataSource.page()) - 1;
    var pagesize = $("#CategoryTreeGrid").data("kendoGrid").dataSource.pageSize();
    return parseInt(rowNumber + (parseInt(page) * parseInt(pagesize)));
}

function ParamToLoadCategoryList(e) {
    var grid = $("#CategoryTreeGrid").data("kendoGrid").dataSource;
    return {
        statusID: ($('#ddlStatus').val() === '') ? -1 : $('#ddlStatus').val(),
        // categoryType: categoryType,
        searchParam: $('#txtSearchKeyword').val(),
        pageSize: grid._pageSize,
        pageNumber: grid._page
    };

}

function LoadCategoryGrid() {
    //var grid = $("#CategoryTreeGrid").data("kendoGrid");
    //grid.dataSource.read({
    //    statusID: ($('#ddlStatus').val() === '') ? -1 : $('#ddlStatus').val(),
    //    //categoryType: categoryType,
    //    searchParam: $('#txtSearchKeyword').val(),
    //    pageSize: grid._pageSize,
    //    pageNumber: grid._page
    //});
    //grid.refresh();
    var grid = $("#CategoryTreeGrid").data("kendoGrid");
    grid.dataSource.page(1);
}

function ShowFormAndHideList() {
    $("#divCategoryList").hide();
    $("#divCategoryForm").show();

}

function ShowListAndHideForm() {
    $("#divCategoryForm").hide();
    $("#divCategoryList").show();
    $("div#popupWindow").removeClass("hide");
}

//function LoadStatusDropDown() {
//    $("#StatusValue").empty();
//    $("#ddlStatus").empty();

//    $.ajax({
//        url: '/api/StatusAdminService/GetStatus/' + identifier,

//        type: 'POST',
//        success: function (statusData) {
//            $('select#ddlStatus').append('<option value="-1"> All Status </option>');

//            $.each(statusData.Data, function (val, Status) {

//                $('select#ddlStatus').append('<option value="' + Status.StatusValue + '">' + Status.StatusName + '</option>');
//                $('select#StatusValue').append('<option value="' + Status.StatusValue + '">' + Status.StatusName + '</option>');

//            });
//            LoadCategoryGrid();
//        }

//    });
//}

function LoadParentDropDown() {
    $("select#ParentCategoryID").empty();
    $('select#ParentCategoryID').append('<option value="0"> Root </option>');

    $.ajax({
        url: '/Admin/EntranceQuestionCategory/GetAllActiveParentForAdmin',
        type: "post",
        dataType: "json",
        success: function (categoryData) {
            if (categoryData != null) {
                $.each(categoryData.Data, function (val, CategoryTree) {
                    $('select#ParentCategoryID').append('<option value="' + CategoryTree.CategoryTreeID + '">' + CategoryTree.CategoryName + '</option>');

                });
            }
        }

    });
}

//function LoadUserGroupDropDown() {
//    $("#UserGroup").empty();
//    InitializeUserGroup();
//    $.ajax({
//        url: '/api/CategoryTreeAdminService/GetAllActiveUserGroup/',
//        type: 'POST',
//        success: function (userGroupData) {
//            $('#UserGroup')[0].sumo.reload();
//            $.each(userGroupData.Data, function (val, UserGroup) {
//                $('#UserGroup')[0].sumo.add(UserGroup.UserGroupID, UserGroup.GroupName);

//            });

//        }

//    });


//}

function InitializeUserGroup() {
    $('.ddlMultiSlectBox').SumoSelect({
        selectAll: true,
        search: true,
        searchText: 'Search Group',
        //locale: ['Select All', 'Cancel'],
        csvDispCount: 3
        //okCancelInMulti: true
    });
    // $('select#UserGroup').prepend('<option selected disabled hidden value="-1">Select User Group</option>');
    $('#UserGroup').prop("selectedIndex", -1);
    //$('div.MultiControls').find('p.btnOk').click(function () {
    //    $('select.ddlMultiSlectBox')[0].sumo.selectAll();
    //});

    //$('div.MultiControls').find('p.btnCancel').click(function () {
    //    $('select.ddlMultiSlectBox')[0].sumo.unSelectAll();
    //});
}

//function GetCourseCategoryType() {
//    $.ajax({
//        url: '/api/CategoryTypeAdminService/GetCourseCategoryType',
//        type: 'GET',
//        success: function (data) {
//            categoryType = data;
//            LoadParentDropDown();
//        }
//    });

//}

//function LoadStatusIdentifier() {
//    $.ajax({
//        url: '/api/StatusIdentifierAdminService/GetCategoryTreeStatusIdentifier',
//        type: 'GET',
//        success: function (data) {
//            identifier = data;
//            LoadStatusDropDown();

//        }
//    });

//}

function Init() {
    $('select#ParentCategoryID').prepend('<option value="0"> Root </option>');

    popupWindow = $("div#popupWindow").kendoWindow({
        title: "Delete Confirmation",
        modal: true,
        visible: false,
        resizable: false,
        width: 400
    }).data("kendoWindow");

    InitializeUserGroup();
    LoadCategoryGrid();
    ShowListAndHideForm();
}

function AddAntiForgeryToken(data) {
    data.__RequestVerificationToken = $('form input[name=__RequestVerificationToken]').val();
    return data;
};