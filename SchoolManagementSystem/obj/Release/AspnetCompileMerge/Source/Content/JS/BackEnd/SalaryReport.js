$(document).ready(function () {
 
    $('#salaryReport').click(function (e) {
        var year = $('#Year').val();
        var month = $('#Month').val();
        var Years = $('#Year option:selected').text();
        var Months = $('#Month option:selected').text();
        if (!$('form#formSalaryReport').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else
        {
            $('#salaryResults').load('/SalaryReport/GetSalaryReport', { year: year, month: month, Years:Years, Months:Months });
        }
    });

});










