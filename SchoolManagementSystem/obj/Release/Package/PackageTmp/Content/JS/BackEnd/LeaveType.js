$(document).ready(function () {


      LoadOrgainsation();  

      $(".create").off().on('click', function (e) {
          $("#hide").fadeIn(500);
          $("#show").hide();
      });
      $(".cancel").off().on('click', function (e) {

          $("#hide").hide();
          $("#show").fadeIn(500);
          ResetFormData();

      });

    $("#Save").off().on('click', function (e) {
        
        var isCommon = false;
        var isattandanceleave = false;
      var  isexpireleave = false;
        var isCommon = $('form#formLeaveType').find('#IsAccumulativeLeave').is(':checked');
        if (isCommon)
        {
            isCommon = true;
        }

        var isattandanceleave = $('form#formLeaveType').find('#IsAttandanceLeave').is(':checked');
        if (isattandanceleave) {
            isattandanceleave = true;
        }

        var isexpireleave = $('form#formLeaveType').find('#IsExpireLeave').is(':checked');
        if (isexpireleave) {
            isexpireleave = true;
        }

        if (!$('form#formLeaveType').data('unobtrusiveValidation').validate()) {

            e.preventDefault();
            return false;
        }
        else {

            $.ajax({
                url: "/Admin/LeaveType/SaveLeaveType",
                type: 'POST',
                data: AddAntiForgeryToken({
                    ID: $('#ID').val(),
                    LeaveTypeName: $('#LeaveTypeName').val(),
                    OrganisationID: $('#OrganisationID').val(),
                    IsAccumulativeLeave: isCommon,
                    IsAttandanceLeave: isattandanceleave,
                    IsExpireLeave: isexpireleave,
                   

                }),
                dataType: 'json',
                success: function (data) {
                    $("#hide").hide();
                    $("#show").fadeIn(500);                   
                    ResetFormData();
                    ShowMessage(data.Message,true);
                    $("#LeaveTypeGrid").data("kendoGrid").dataSource.read();
                }
            })
        }
    });

    $("#FieldFilter").keyup(function () {
        var value = $("#FieldFilter").val();
        grid = $("#LeaveTypeGrid").data("kendoGrid");
        rowNumber = 0;
        if (value) {
            grid.dataSource.filter({ field: "LeaveTypeName", operator: "contains", value: value });

        } else {
            grid.dataSource.filter({});
        }
    });

})

function Edit(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
   
    $.ajax({
        url: "/Admin/LeaveType/EditLeaveType",
        data: { id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result)
        {

            $("#hide").fadeIn(500);
            $("#show").hide();
            $("#ID").val(result.ID);
            $("#OrganisationID").val(result.OrganisationID);
            $("#LeaveTypeName").val(result.LeaveTypeName);
            if (result.IsAccumulativeLeave == true)
            {
                $('#IsAccumulativeLeave').prop('checked', true);

            }
            if (result.IsAccumulativeLeave == false) {
                $('#IsAccumulativeLeave').prop('checked', false);

            }

            if (result.IsAttandanceLeave == true)
            {
                $('#IsAttandanceLeave').prop('checked', true);

            }
            if (result.IsAttandanceLeave == false)
            {
                $('#IsAttandanceLeave').prop('checked', false);

            }

            if (result.IsExpireLeave == true) {
                $('#IsExpireLeave').prop('checked', true);

            }
            if (result.IsExpireLeave == false) {
                $('#IsExpireLeave').prop('checked', false);

            }
        },
        error: function (result) {

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
            url: "/Admin/LeaveType/DeleteleaveType",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {

                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message,false);
                ResetFormData();
                $("#LeaveTypeGrid").data("kendoGrid").dataSource.read();


            }
        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}
function LoadOrgainsation() {
    $("#OrganisationID").empty();
    $("#OrganisationID").append('<option value>--Organisation--</option>')
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

