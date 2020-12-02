$(document).ready(function () {
  
    LoadOrgainsation();

    $("#Print").off().on('click', function (e) {
        $("#attandanceResults").print();

    });


    $("#OrganisationID").change(function () {
        var organisationid = $("#OrganisationID").val();
        GetEmployeeBasedOnOrganisation(organisationid);

    });
    $("#monthlySummaryReportGenerate").off().on('click', function (e) {

        if (!$('form#formMonthlySummaryReport').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {

            $.ajax({
                url: "/Admin/MonthlySummaryReport/GenerateMonthlySummaryReport",
                type: 'POST',
                data: AddAntiForgeryToken({
                    OrganisationID: $('#OrganisationID').val(),
                    EmployeeID: $('#EmployeeID').val(),
                    Year: $('#Year').val(),
                    Month: $('#Month').val()
                }),
                dataType: 'json',
                success: function (result)
                {
                    var date,html = '';

                    $('#name').text(result.Name);
                    $('#organisation').text(result.Organisation);
                    $('#designation').text(result.Designation);
                    $('#totaldaysinmonth').text(result.TotalDaysInMonth);
                    $('#attandanceResults').show();
                    $('#attendtable').empty();
                    if (result.TotalDaysInMonth != 0)
                    {
                        date += '<tr>';
                        for (var i = 1; i <= result.TotalDaysInMonth; i++)
                        {
                            date += '<td><div class="square1" style="background-color:dimgrey;color:black">' + i + '</div></td>'
                        }
                        date += '</tr>';
                        $('#attendtable').append(date);
                        if (result.MonthlyAttandanceSummary != null)
                        {
                            html += '<tr>';
                            for (var j = 1; j <= result.MonthlyAttandanceSummary.length; j++)
                            {
                                html += '<td><div class="square1" style="background-color:dimgrey;color:black">' + result.MonthlyAttandanceSummary[i].Type + '</div></td>'
                            }
                            html += '</tr>';
                            $('#attendtable').append(html);

                            $("#Print").show();
                           
                        }
                        else
                        {                        
                            $("#Print").hide();
                            ShowMessage('No Attandace Summary Records Found', true);
                        }
                    }

                    else
                    {
                        $("#Print").hide();
                        ShowMessage('Please First Manage Calendar for this year', true);
                    }
                },
                error: function (e, xhr)
                {                   
                    ShowMessage("Warning !! Error Occured", false);
                }
            })
        }
    });

    $("#Print").off().on('click', function (e) {
        $("#salaryResults").print();

    });

});
function LoadOrgainsation() {

    $("#OrganisationID").empty();
    $("#OrganisationID").append('<option value>--Select--</option>')

    $.ajax({
        url: "/Admin/DropDown/GetOrganisationDropDown",
        type: 'POST',
        dataType: 'json',
        global: false,
        async: true,
        success: function (data) {
            jQuery.each(data, function (index, value) {
                $("#OrganisationID").append('<option value=' + value.ID + '>' + value.Name + '</option>');

            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured", false);  //
        }



    })

}

function GetEmployeeBasedOnOrganisation(organisation) {


    $("#EmployeeID").empty();
    $("#EmployeeID").append('<option value>--Select--</option>')

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

                $("#EmployeeID").append('<option value=' + value.ID + '>' + value.Name + '</option>');
            })
        },

        error: function (response) {

            ShowMessage("Warning! Error Occured");  //
        }



    })

}










