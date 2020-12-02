$(document).ready(function () {
    $("#Search").click(function (e)
    {
        if (!$('form#formSalaryHeadAmount').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {
            $("#SalaryHeadAmountList").data("kendoGrid").dataSource.read();
        }

    });
    $("#salaryHeadAmount").off().on('click', function (e) {
        if (!$('form#formSalaryHeadAmount').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        } else {
            e.preventDefault();
            var leaveEntry = $("#SalaryHeadAmountList").data("kendoGrid").dataSource.data();
            $.ajax({
                url: "/Admin/SalaryHeadAmount/SaveSalaryHeadAmount",
                type: 'POST',
                data: { data: JSON.stringify(leaveEntry), SalaryHeadID: $("#SalaryHeadID").val() },
                dataType: 'json',
                success: function (data) {
                    ShowMessage(data.Message, true);
                    $("#SalaryHeadAmountList").data("kendoGrid").dataSource.read();
                },
                error: function (response) {
                    ShowMessage("Warning! Error Occured", false);
                }
            })

        }
    })
    
})

function ParamToSalaryHeadAmount(e) {
    var grid = $("#SalaryHeadAmountList").data("kendoGrid").dataSource;
    return {
        OrganisationID: $("#OrganisationID :selected").val() == "" ? -1 : $("#OrganisationID :selected").val(),
        SalaryHeadID: $("#SalaryHeadID :selected").val() == "" ? -1 : $("#SalaryHeadID :selected").val(),

    };

}

function resetRowNumberSalaryHeadAmount(e) {
    var rows = e.sender.tbody[0].rows;
    $(rows).each(function (e) {
        var grid = $("#SalaryHeadAmountList").data("kendoGrid");
        var row = this;
        var dataItem = grid.dataItem(row);

        if (dataItem.IsAdded == true) {
            grid.tbody.find("tr[data-uid='" + dataItem.uid + "']")
           .addClass("k-alt k-state-selected gridselect")
            }
    })
    $(".k-grid-Edit").find("span").addClass("fa fa-pencil");
    $(".k-grid-Edit").removeClass("k-button");
    $(".k-grid-Delete").find("span").addClass("fa fa-trash");
    $(".k-grid-Delete").removeClass("k-button");
    $(".k-grid-Details").find("span").addClass("fa fa-eye");
    $(".k-grid-Details").removeClass("k-button");
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



