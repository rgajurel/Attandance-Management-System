var monthsList = "";
var stuId = "";
var sessId = "";
var clId = "";
var facId = "";
var secId = "";

$(document).ready(function () {
   


    $("#paymentCancel").off().on('click', function (e)
    {
        
        $('#MonthList').find('input[type=checkbox]:checked').removeAttr('checked');
        monthsList = "";
        $("#FeeLists").data("kendoGrid").dataSource.read();

    });


    $("#paymentBill").off().on('click', function (e) {   
        if (!$('form#formStudentPayment').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {
            
            var previousDue = parseFloat(document.getElementById('PreviousDue').value);
            var totalDiscount = parseFloat(document.getElementById('TotalDiscount').value);
            var grandTotal = parseFloat(document.getElementById('GrandTotal').value);
            var balance = $("#Balance").val() == "" ? 0 : $("#Balance").val();
            var totalPaid = parseFloat(document.getElementById('TotalPaid').value);
            var totalFee = parseFloat(document.getElementById('TotalFee').value);

            var batchFeeEntry = $("#FeeLists").data("kendoGrid").dataSource.data();
            $.ajax({
                url: "/Admin/FeeCollection/SaveFeeCollection",
                type: 'POST',
                data: {
                    data1: JSON.stringify(batchFeeEntry), stuId: stuId, session: sessId, faculty: facId, classs: clId, section: secId,
                    previousDue:previousDue,totalDiscount:totalDiscount,totalFee:totalFee,grandTotal:grandTotal,balance:balance,totalPaid:totalPaid,
                },

                success: function (data)
                {
                    window.open("/Admin/FeeCollection/PrintBill?id=" + data + "", '_blank', 'fullscreen=yes, scrollbars=auto');
                    $("#FeeLists").data("kendoGrid").dataSource.read();
                    calculatePreviousDue(stuId, sessId, facId, clId, secId);
                }
            })

        }
    });


    $("#dueBill").off().on('click', function (e) {
        $("#TotalPaid").hide();
        if (!$('form#formStudentPayment').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {

            var previousDue = parseFloat(document.getElementById('PreviousDue').value);
            var totalDiscount = parseFloat(document.getElementById('TotalDiscount').value);
            var grandTotal = parseFloat(document.getElementById('GrandTotal').value);
            var balance = $("#Balance").val() == "" ? 0 : $("#Balance").val();
            var totalFee = parseFloat(document.getElementById('TotalFee').value);

            var batchFeeEntry = $("#FeeLists").data("kendoGrid").dataSource.data();
            $.ajax({
                url: "/Admin/FeeCollection/SaveDueBill",
                type: 'POST',
                data: {
                    data1: JSON.stringify(batchFeeEntry), stuId: stuId, session: sessId, faculty: facId, classs: clId, section: secId,
                    previousDue: previousDue, totalDiscount: totalDiscount, totalFee: totalFee, grandTotal: grandTotal,
                },

                success: function (data) {
                    window.open("/Admin/FeeCollection/PrintDueBill?id=" + data + "", '_blank', 'fullscreen=yes, scrollbars=auto');
                    //$("#FeeLists").data("kendoGrid").dataSource.read();
                    //calculatePreviousDue(stuId, sessId, facId, clId, secId);
                    $("#TotalPaid").show();
                }
            })

        }
        $("#TotalPaid").show();
    });


    $(document).bind('keypress', function (e) {

        if (e.keyCode == 13) 
        {
            $('#searchStudent').trigger('click');
        }
    });

    $('#searchStudent').click(function () 
    {
        $("#studentsList").data("kendoGrid").dataSource.read();

    });


    $("#Faculty").change(function ()
    {
        var classs = $("#Class").val();
        var faculty = $("#Faculty").val();
        GetSectionBasedOnClassAndFaculty(classs, faculty);
       
    });
    $("#Class").change(function ()
    {
        var classs = $("#Class").val();
        GetFacultyBasedOnClass(classs);

    });

    $("#studentsList").on("change", "input.chkbx", function (e)
    {
       
        var check = $(this).is(":checked");
        $("input.chkbx", "#studentsList").prop("checked", false);
        $(this).prop("checked", check);
        var grid = $("#studentsList").data("kendoGrid"),
        dataItem = grid.dataItem($(e.target).closest("tr"));
        if (check) {
            document.getElementById('MonthList').style.display = "none";
            $("#MonthList").fadeIn(800);
            $.ajax({
                url: "/Admin/FeeCollection/GetMonth",
                type: 'POST',
                dataType: 'json',
                data: {
                    studentId: dataItem.ID,
                },
                success: function (data) {
                   
                    if (data.length > 0) {
                        $('#Months').empty();
                        for (var i = 0; i < data.length; i++)
                        {
                            var item = data[i];
                            $("#Months").append('<tr><td><input type="checkbox" class="checkMonths" name="month" value="' + item.ID + '"></td><td>' + item.Month + '</td></tr>')
                        }
                    } else
                    {
                        document.getElementById('MonthList').style.display = "none";
                    }
                }
            })

        } else {
            document.getElementById('MonthList').style.display = "none";
        }
        monthsList = "";
        $("#FeeLists").data("kendoGrid").dataSource.read();
        document.getElementById("PreviousDue").value = "";
        calculateGrandTotal(0, 0, 0);       
    });

    $("#MonthList").on("change", "input.checkMonths", function (e) {
        var selected = [];
        var mnths = "";
        $('div#MonthList input[type=checkbox]').each(function () {
            if ($(this).is(":checked")) {
                selected.push($(this).attr('value'));
            }
        });

        var grid = $("#studentsList").data("kendoGrid");
        $('div#studentsList input[type=checkbox]').each(function () {
            if ($(this).is(":checked")) {
                dataItem = grid.dataItem($(this).closest("tr"));
                stuId = dataItem.ID;
                sessId = dataItem.SessionID;
                facId = dataItem.FacultyID;
                clId = dataItem.ClassID;
                secId = dataItem.Section;
            }
        });
        if (selected.length > 0 && stuId != "") {
            for (var i = 0; i < selected.length; i++) {
                // mnths += '\'' + selected[i] + '\'' + ',';
                mnths += selected[i] + ',';
            }
            monthsList = mnths.replace(/,\s*$/, "");
        }

        $("#FeeLists").data("kendoGrid").dataSource.read();
        calculatePreviousDue(stuId, sessId, facId, clId, secId);

    });


    jQuery('#TotalPaid').on('input propertychange paste', function () {
      
        try{
            if (parseFloat($("#TotalPaid").val()) > 0 && (parseFloat($("#TotalPaid").val()) <= parseFloat($("#GrandTotal").val()))) {
               
                $('#paymentBill').prop('disabled', false);
            } else {
                $('#paymentBill').prop('disabled', true);
            }
        } catch (ex) {
            $('#paymentBill').prop('disabled', true);
        }
    });

    $("#FeeListSelect").on("change", "input.chkbxFee", function (e) {
        
        var totalFee = 0;
        var totalDiscount = 0;

        var grid = $("#FeeLists").data("kendoGrid");       

        $('#FeeListSelect input.chkbxFee:checked').each(function () {
            dataItem = grid.dataItem($(this).closest("tr"));
            totalFee += dataItem.Fee;
            totalDiscount += dataItem.Discount;
        });
        
        var dataItem = grid.dataItem($(this).closest('tr'));
        if ($(this).is(':checked')) {
            dataItem.set('IsAdmin', true);
           
            grid.tbody.find("tr[data-uid='" + dataItem.SN + "']").addClass("k-alt k-state-selected");

        } else {
            grid.tbody.find("tr[data-uid='" + dataItem.SN + "']").removeClass("k-alt k-state-selected");
            dataItem.set('IsAdmin', false);
        }
        


        var previousDue = parseFloat(document.getElementById('PreviousDue').value);
        calculateGrandTotal(totalFee, totalDiscount, previousDue)
    });


    function calculateGrandTotal(totalFee, totalDisocunt, previousDue) {
        var balance = $("#Balance").val() == "" ? 0 : $("#Balance").val();
        var total = parseFloat((previousDue + totalFee) - totalDisocunt);
        if (total > 0) {
            $("#TotalPaid").attr("readonly", false);
            $("#TotalPaid").attr("max", total);
            //$('#paymentBill').prop('disabled', false);
            $('#dueBill').prop('disabled', false);
        } else {
            $("#TotalPaid").attr("readonly", true);
            $("#TotalPaid").attr("max", 0);
            //$('#paymentBill').prop('disabled', true);
            $('#dueBill').prop('disabled', true);
        }
        document.getElementById('TotalFee').value = totalFee;
        document.getElementById('TotalDiscount').value = totalDisocunt;
        document.getElementById('TotalPaid').value = 0;
        document.getElementById('GrandTotal').value = total;
        document.getElementById('Balance').value = 0;
    }


    function calculatePreviousDue(a, b, c, d, e) {
        var due = 0;
        $.ajax({
            url: "/Admin/FeeCollection/GetPreviousDue",
            type: 'POST',
            dataType: 'json',
            data: {
                StudentId: a,
                SessionId: b,
                FacultyId: c,
                ClassId: d,
                Section: e
            },
            success: function (data) {
                document.getElementById("PreviousDue").value = data;
                calculateGrandTotal(0, 0, data);
            }
        })

    }


    //$("#Section").change(function () {
    //    $("#studentsList").data("kendoGrid").dataSource.read();
    //});

    $("#FieldFilterStudent").keyup(function () {
        var value = $("#FieldFilterStudent").val();
        grid = $("#studentsList").data("kendoGrid");
        rowNumber = 0;
        if (value) {
            grid.dataSource.filter({ field: "StudentName", operator: "startswith", value: value });

        } else {
            grid.dataSource.filter({});
        }
    });



    $("#FieldFilterFee").keyup(function () {
        var value = $("#FieldFilterFee").val();
        grid = $("#FeeLists").data("kendoGrid");
        rowNumber = 0;
        if (value) {
            grid.dataSource.filter({ field: "Type", operator: "startswith", value: value });

        } else {
            grid.dataSource.filter({});
        }
    });


    //$('#StudentName').on('input', function (e) {
    //    $("#studentsList").data("kendoGrid").dataSource.read();
    //});

});




function GetFacultyBasedOnClass(classs, faculty)
{
    $("#Faculty").empty();
    $("#Faculty").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/Students/GetFacultyBaseOnClass",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: classs,
        },

        success: function (data)
        {
           

            jQuery.each(data, function (index, value)
            {
               
                if (value.ID === parseInt(faculty)) {
                    $("#Faculty").append('<option selected value=' + value.ID + '>' + value.Faculty + '</option>')
                }
                else {
                    $("#Faculty").append('<option value=' + value.ID + '>' + value.Faculty + '</option>')
                }

            });


        }
    })

}

function GetSectionBasedOnClassAndFaculty(classs, faculty, section) {
    $("#Section").empty();
    $("#Section").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/Students/GetSectionBaseOnClassAndFaculty",
        type: 'POST',
        dataType: 'json',
        data: {
            ClassID: classs,
            FacultyID: faculty
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


function ParamToStudentList(e)
{
    var grid = $("#studentsList").data("kendoGrid").dataSource;
    return {
        pageSize: grid._pageSize,
        pageNumber: grid._page,
        Session: $("#Session :selected").val() == "" ? -1 : $("#Session :selected").val(),
        Class: $("#Class :selected").val() == "" ? -1 : $("#Class :selected").val(),
        Section: $("#Section :selected").val() == "" ? "" : $("#Section :selected").val(),
        Faculty: $("#Faculty :selected").val() == "" ? -1 : $("#Faculty :selected").val(),
        StudentName: $("#StudentName").val() == "" ? "" : $("#StudentName").val(),
    };

}


function ParamToFeeList(e) {
    var grid = $("#FeeLists").data("kendoGrid").dataSource;
    return {
        StudentId: stuId,
        SessionID: sessId,
        FacultyID: facId,
        ClassID: clId,
        Section: secId,
        Month: monthsList,
    };

}

function resetRowNumberFeeCollection(e)
{
    var grid = $("#FeeLists").data("kendoGrid");
    var gridData = grid.dataSource.view();
    for (var i = 0; i < gridData.length; i++) {
        var currentUid = gridData[i].uid;
        if (gridData[i].PaidStatus >= 1) {
            var currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
            $(currenRow).addClass("makeDisableColumn");   
        }
    }

}

