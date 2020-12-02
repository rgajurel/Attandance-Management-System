$(document).ready(function () {
    var Faculty = $("#Faculty").val();
    GetClassBasedOnFaculty(Faculty);

   
    $("#faculty").hide();
    $("#class").hide();
    $("#section").hide();

    $("#Faculty").change(function () {
        var Faculty = $("#Faculty").val();
        GetClassBasedOnFaculty(Faculty);
    });
    $("#Class").change(function () {
        var classs = $("#Class").val();
        var Faculty = $("#Faculty").val();
        GetSectionBasedOnClass(classs, Faculty);
    });

    $("#Overall").on("change", function (e) {
        
        if ($(this).is(":checked")) {
            $("#faculty").hide();
            $("#class").hide();
            $("#section").hide();
        } else {
            $("#faculty").show();
            $("#class").show();
            $("#section").show();
        }
    });
    
    //$("#viewDailyCollection").off().on('click', function (e) {
    //    if (!$('form#formDailyCollection').data('unobtrusiveValidation').validate()) {
    //        e.preventDefault();
    //        return false;
    //    }
    //    else {
            
    //        var dateStringFrom =new Date(document.getElementById('DateFrom').value);
    //        var dateStringTo =new Date(document.getElementById('DateTo').value);
            
    //        if (dateStringFrom>dateStringTo) {
    //            ShowMessage('Warning !!! Date From must be earlier than Date To.');
    //            return false;
    //        }
            
    //        var overallStatus = document.getElementById('Overall').value;
    //        var dateFrom = document.getElementById('DateFrom').value;
    //        var dateTo = document.getElementById('DateTo').value;
    //        var session = document.getElementById('Session').value;
    //        var faculty = document.getElementById('Faculty').value;
    //        var classs = document.getElementById('Class').value;
    //        var section = document.getElementById('Section').value;
            
    //        $.ajax({
    //            url: "/Admin/FeeDailyCollection/getDailyCollection",
    //            type: 'GET',
    //            data: {
    //                overallStatus: overallStatus,
    //                dateFrom: dateFrom,
    //                dateTo: dateTo,
    //                session: session,
    //                faculty: faculty,
    //                classs:classs,
    //                section: section
    //            },
    //            contentType: "application/json;charset=utf-8",
    //            dataType: "json",
    //            success: function (data)
    //            {
    //                console.log(data);
    //                window.open('data:application/vnd.ms-excel,' + JSON.stringify(data) + '');
    //                debugger
    //                $(".test1").html(JSON.stringify(data));
    //                //JSONToCSVConvertor(JSON.stringify(data), "Vehicle Report", true);
    //            }
    //        })


    //    }
    //});

});

function validateDate() {
    var dateStringFrom = new Date(document.getElementById('DateFrom').value);
    var dateStringTo = new Date(document.getElementById('DateTo').value);

    if (dateStringFrom > dateStringTo) {
        ShowMessage('Warning !!! Date From must be earlier than Date To.');
        return false;
    } else {
        return true;
    }
}
function JSONToCSVConvertor(JSONData, ReportTitle, ShowLabel) {
    //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

    var CSV = '';
    //Set Report title in first row or line

    CSV += ReportTitle + '\r\n\n';

    //This condition will generate the Label/Header
    if (ShowLabel) {
        var row = "";

        //This loop will extract the label from 1st index of on array
        for (var index in arrData[0]) {

            //Now convert each value to string and comma-seprated
            row += index + ',';
        }

        row = row.slice(0, -1);

        //append Label row with line break
        CSV += row + '\r\n';
    }

    //1st loop is to extract each row
    for (var i = 0; i < arrData.length; i++) {
        var row = "";

        //2nd loop will extract each column and convert it in string comma-seprated
        for (var index in arrData[i]) {
            row += '"' + arrData[i][index] + '",';
        }

        row.slice(0, row.length - 1);

        //add a line break after each row
        CSV += row + '\r\n';
    }

    if (CSV == '') {
        alert("Invalid data");
        return;
    }

    //Generate a file name
    var fileName = "MyReport_";
    //this will remove the blank-spaces from the title and replace it with an underscore
    fileName += ReportTitle.replace(/ /g, "_");

    //Initialize file format you want csv or xls
    var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);

    // Now the little tricky part.
    // you can use either>> window.open(uri);
    // but this will not work in some browsers
    // or you will not get the correct file extension    

    //this trick will generate a temp <a /> tag
    var link = document.createElement("a");
    link.href = uri;

    //set the visibility hidden so it will not effect on your web-layout
    link.style = "visibility:hidden";
    link.download = fileName + ".csv";

    //this part will append the anchor tag and remove it after automatic click
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function GetClassBasedOnFaculty(faculty, Class) {
    $("#Class").empty();
    $("#Class").append('<option value="0">Select</option>')
    $.ajax({
        url: "/Admin/CommonFeeDiscount/GetClassBasedOnFaculty",
        type: 'POST',
        dataType: 'json',
        data: {
            Faculty: faculty,
        },
        success: function (data) {
            jQuery.each(data, function (index, value) {

                if (value.ID === parseInt(Class)) {
                    $("#Class").append('<option selected value=' + Class + '>' + value.ClassName + '</option>')
                }
                else {
                    $("#Class").append('<option value=' + value.ID + '>' + value.ClassName + '</option>')
                }
            });
        }
    })
}



function GetSectionBasedOnClass(classs, faculty, section) {
    $("#Section").empty();
    $("#Section").append('<option value>Select</option>')
    $.ajax({
        url: "/Admin/CommonFeeDiscount/GetSectionBaseOnClass",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: classs,
            Faculty: faculty
        },
        success: function (data) {
            var sectionArray = data.Sections.split(',');
          
            jQuery.each(sectionArray, function (index, value) {
                if (value === section) {
                    $("#Section").append('<option selected value=' + value + '>' + value + '</option>')
                }
                else {
                    $("#Section").append('<option value=' + value + '>' + value + '</option>')
                }
            });
        }
    })
}



function onError(e, status) {
    ShowMessage('Warning ! Error Occured');
}
