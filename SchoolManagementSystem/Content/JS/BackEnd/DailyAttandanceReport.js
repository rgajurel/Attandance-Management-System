$(document).ready(function () {

    CheckDate();
    LoadOrgainsation();
    $('#NepaliDate').val(AD2BS($('#Date').val()));

    $('#NepaliDate').nepaliDatePicker({
        ndpEnglishInput: 'Date',
        npdMonth: true,
        npdYear: true,
        npdYearCount: -25
    });

    $('#Date').change(function () {
        $('#Nepali').val(AD2BS($('#Date').val()));
    });
    $('#dailyAttandanceReport').click(function (e)
    {    
        var organisationid = $('#OrganisationID').val();
        var yearid = $('#Year').val();
        var monthid = $('#Month').val();      

        var date = $('#Date').val();
        var nepaliDate = $('#NepaliDate').val();


        var Organisation = $('#OrganisationID option:selected').text();
        var Year = $('#Year option:selected').text();
        var Month = $('#Month option:selected').text();
     
        if (!$('form#formDailyAttandanceReport').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {
            $('#attandanceResults').load('/DailyAttandanceReport/GetDailyAttandanceReport',
                { organisationid: organisationid, Organisation: Organisation,Date:date,NepaliDate:nepaliDate,yearid:yearid,monthid:monthid,Month:Month,Year:Year});
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
                $("#OrganisationID").append('<option value=' + value.ID + '>' + value.Name + '</option>');

            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured", false);  //
        }



    })

}










