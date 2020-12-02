
$(document).ready(function ()
{
    LoadOrgainsation();

    $("#OrganisationID").change(function ()
    {
        var organisationid = $("#OrganisationID").val();      
        GetEmployeeBasedOnOrganisation(organisationid);    

    });
    $("#GenerateSalarySlip").off().on('click', function (e) {
        
        if (!$('form#formSalarySlip').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {
           
            $.ajax({
                url: "/Admin/SalarySlip/GenerateSalarySlip",
                type: 'POST',
                data: AddAntiForgeryToken({
                    OrganisationID: $('#OrganisationID').val(),
                    EmployeeID: $('#EmployeeID').val(),
                    Year: $('#Year').val(),
                    Month: $('#Month').val()
                }),
                dataType: 'json',
                success: function (result) {
                    
                    var addsalary = '', deductsalary = '';
                    
                        $(".employee").text(result.EmployeeDetails.Name);
                        $(".designation").text(result.EmployeeDetails.Designation);
                        $(".department").text(result.EmployeeDetails.Department);
                        $(".jobtype").text(result.EmployeeDetails.Employment);
                        $(".dailyhour").text(result.EmployeeDetails.DailyHour);
                        $(".pf").text(result.EmployeeDetails.PFNumber);
                        $(".pan").text(result.EmployeeDetails.PANNumber);
                        $(".cit").text(result.EmployeeDetails.CITNumber);
                        $(".address").text(result.EmployeeDetails.OrganisationAddress);
                        $(".bankacnt").text(result.EmployeeDetails.BankAccountNumber);
                        $(".totaldays").text(result.EmployeeDetails.TotalDaysinMonth);
                        $(".holiday").text(result.EmployeeDetails.HolidaysInMonth);
                        $(".paidleave").text(result.EmployeeDetails.TotalPaidLeaveTaken);
                        $(".presentdays").text(result.EmployeeDetails.TotalPresentDays);
                        $(".absentdays").text(result.EmployeeDetails.TotaAbsentDays);
                        $(".workingdays").text(result.EmployeeDetails.TotalWorkingDays);
                        $(".joiningdate").text(result.EmployeeDetails.JoiningDate);

                        $(".organisation").text($("#OrganisationID option:selected").text());
                        $(".year").text($("#Year option:selected").text());
                        $(".month").text($("#Month option:selected").text());

                 
                      
                        if (result.AddSalaryDetails == null)
                        {                         
                            $("#noavailiable").remove();
                            addsalary += '<dir id="noavailiable" class="row centered txt-centered"><h2>No Details Availiable</h2></dir>';
                            $("#NODataAvailiable").append(addsalary);
                            $("#salaryResults").hide();
                            $("#NODataAvailiable").show();
                            $("#Print").hide();
                        }
                    
                    else
                        {
                            $("#NODataAvailiable").hide();
                            $("#salaryResults").show();
                            $("#addsalary").empty();
                            $("#deductsalary").empty();
                            addsalary += '<tr><td class="tdbold">S.N</td><td class="tdbold">Title</td><td class="tdbold">Calculation</td><td class="tdbold">Amount</td></tr>';
                            deductsalary += '<tr><td class="tdbold">S.N</td><td class="tdbold">Title</td><td class="tdbold">Calculation</td><td class="tdbold">Amount</td></tr>';

                                                    
                        
                            $("#grossincome").text(result.GrossSalary==null?"0.00":result.GrossSalary);
                            $("#totaldeduction").text(result.TotalDeduction == null ? "0.00" : result.TotalDeduction);
                            $("#totalsaving").text(result.TotalSaving == null ? "0.00" : result.TotalSaving);
                            $("#finalsalary").text(result.FinalSalary == null ? "0.00" : result.FinalSalary);
                            
                           
                            $(result.AddSalaryDetails).each(function (i, data)
                            {                                
                                addsalary += '<tr><td class="tdbold">' + parseInt(i + 1) + '</td><td class="tdbold">' + data.SalaryHeading + '</td><td class="tdbold">+</td><td class="tdbold">' + data.Amount + '</td></tr>';
                               
                            });

                            $(result.SalaryDeductionDetails).each(function (j, data1)
                            {
                                deductsalary += '<tr><td class="tdbold">' + parseInt(j + 1) + '</td><td class="tdbold">' + data1.SalaryHeading + '</td><td class="tdbold">-</td><td class="tdbold">' + data1.Amount + '</td></tr>';

                            });
                            $("#addsalary").append(addsalary);
                            $("#deductsalary").append(deductsalary);
                            $("#Print").show();
                            
                        }
                       


                },
                error: function (e,xhr) {
                    debugger;
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
                $("#OrganisationID").append('<option value=' + value.ID + '>' + value.Name + '</option>')

                $("#OrganisationIDSearch").append('<option value=' + value.ID + '>' + value.Name + '</option>')
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured");  //
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