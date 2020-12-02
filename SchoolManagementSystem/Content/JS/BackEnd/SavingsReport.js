$(document).ready(function () {
 
    LoadOrgainsation();  
    $("#OrganisationID").change(function () {
        var organisationid = $("#OrganisationID").val();
        GetEmployeeBasedOnOrganisation(organisationid);

    });
    $('#savingsReport').click(function (e) {
        var employeeid = $('#EmployeeID').val();
        var organisationid = $('#OrganisationID').val();
        var savingstypeid = $('#SavingsTypeID').val();

        var Employee = $('#EmployeeID option:selected').text();
        var Organisation = $('#OrganisationID option:selected').text();
        var SavingsType = $('#SavingsTypeID option:selected').text();
        if (!$('form#formSavingsReport').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else
        {
            $('#savingsResults').load('/SavingsReport/GetSalaryReport',
                { employeeid: employeeid, organisationid: organisationid, organisationid: organisationid, savingstypeid: savingstypeid, Employee: Employee, Organisation: Organisation, SavingsType: SavingsType });
        }
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
               
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured", false);  //
        }



    })

}
function GetEmployeeBasedOnOrganisation(organisation, employee) {


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

            jQuery.each(data, function (index, value)
            {              

                $("#EmployeeID").append('<option value=' + value.ID + '>' + value.Name + '</option>');           



            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured", false);  //
        }



    })

}









