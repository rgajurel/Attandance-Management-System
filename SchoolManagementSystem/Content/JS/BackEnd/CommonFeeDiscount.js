$(document).ready(function () {

     $("#Class").change(function () {
        var classs = $("#Class").val();
        GetFacultyBasedOnClass(classs);

    });

    $("#Faculty").change(function () {
        var classs = $("#Class").val();
        var faculty = $("#Faculty").val();
        GetSectionBasedOnClassAndFaculty(classs, faculty);

    });
    $("#Section").change(function () {
        var session = $("#Session").val();
        var classs = $("#Class").val();
        var Faculty = $("#Faculty").val();
        var Section = $("#Section").val();
        GetFeeTopic(Faculty, session, classs, Section);
    });


    $("#Type").change(function () {
        var session = $("#Session").val();
        var classs = $("#Class").val();
        var Faculty = $("#Faculty").val();
        var Section = $("#Section").val();
        var Type = $("#Type").val();
        GetMonth(Faculty, session, classs, Section, Type);
    });

    $("#Month").change(function (e)
    {
        $("#commonFeeDiscountGrid").data("kendoGrid").dataSource.read();
    });


    $("#FieldFilterStudent").keyup(function ()
    {
        var value = $("#FieldFilterStudent").val();
        grid = $("#commonFeeDiscountGrid").data("kendoGrid");
        rowNumber = 0;
        if (value) {
            grid.dataSource.filter({ field: "StudentName", operator: "startswith", value: value });

        } else {
            grid.dataSource.filter({});
        }
    });

    $("#commonFeeDiscountSave").off().on('click', function (e) {
        var session = $("#Session").val();
        var classs = $("#Class").val();
        var Faculty = $("#Faculty").val();
        var Section = $("#Section").val();
        var Type = $("#Type").val();
        var month = $("#Month").val();
        if (!$('form#formCommonFeeDiscount').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        } else {

            e.preventDefault();
            var batchDiscountEntry = $("#commonFeeDiscountGrid").data("kendoGrid").dataSource.data();
            $.ajax({
                url: "/Admin/CommonFeeDiscount/SaveCommonFeeDiscount",
                type: 'POST',
                data: {
                    data1: JSON.stringify(batchDiscountEntry), session: session, classs: classs, faculty: Faculty,
                    section: Section, type: Type, month: month
                },

                success: function (data)
                {
                    $("#commonFeeDiscountGrid").data("kendoGrid").dataSource.read();                    
                    //$('select').attr("disabled",false);
                    //document.getElementById("CourseFeeDiscountPanel").style.opacity = "0.8";
                    //document.getElementById("CourseFeeDiscountPanel").style.pointerEvents = "None";
                    //$('#commonFeeDiscountSave').attr('disabled', 'disabled');

                    ShowMessage(data.Message);
                }
            })

        }
    })

});


function GetFacultyBasedOnClass(classs, faculty) {
    $("#Faculty").empty();
    $("#Faculty").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/Students/GetFacultyBaseOnClass",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: classs,
        },

        success: function (data) {
           
            jQuery.each(data, function (index, value) {

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

function GetFeeTopic(Faculty, session, classs, Section) {
    $("#Type").empty();
    $("#Type").append('<option value="0">--Select--</option>')
    $.ajax({
        url: "/Admin/CommonFeeDiscount/GetFeeType",
        type: 'POST',
        dataType: 'json',
        data: {
            Faculty: Faculty,
            Session: session,
            Class: classs,
            Section: Section
        },

        success: function (data) {

            jQuery.each(data, function (index, value) {

                if (value.ID === parseInt(Type)) {
                    $("#Type").append('<option selected value=' + Type + '>' + value.Type + '</option>')
                }
                else {
                    $("#Type").append('<option value=' + value.ID + '>' + value.Type + '</option>')
                }
            });
        }
    })

}

function GetMonth(Faculty, session, classs, Section, Type) {
    $("#Month").empty();
    $("#Month").append('<option value="0">--Select--</option>')
    $.ajax({
        url: "/Admin/CommonFeeDiscount/GetMonthBasedOnFeeType",
        type: 'POST',
        dataType: 'json',
        data: {
            Faculty: Faculty,
            Session: session,
            Class: classs,
            Section: Section,
            Type: Type
        },

        success: function (data) {

            jQuery.each(data, function (index, value)
            {

                if (value.ID === parseInt(Type)) {
                    $("#Month").append('<option selected value=' + Month + '>' + value.Month + '</option>')
                }
                else {
                    $("#Month").append('<option value=' + value.ID + '>' + value.Month + '</option>')
                }
            });
        }
    })

}

function onError(e, status) {
    ShowMessage('Warning ! Error Occured');
}

function ParamToDiscountList(e) {

    var grid = $("#commonFeeDiscountGrid").data("kendoGrid").dataSource;
    return {
        Session: $("#Session :selected").val() == "" ? -1 : $("#Session :selected").val(),
        Class: $("#Class :selected").val() == "" ? -1 : $("#Class :selected").val(),
        Section: $("#Section :selected").val() == "" ? "" : $("#Section :selected").val(),
        Faculty: $("#Faculty :selected").val() == "" ? -1 : $("#Faculty :selected").val(),
        Type: $("#Type :selected").val() == "" ? -1 : $("#Type :selected").val(),
        Month: $("#Month :selected").val() == "" ? -1 : $("#Month :selected").val(),
    };

}

//function checkData() {
//    var grid = $("#commonFeeDiscountGrid").data("kendoGrid");
//    grid.dataSource.read();
//    var count = grid.dataSource.total();
//    if (count > 0) {
//        $('select').attr('disabled', 'disabled');
//       // document.getElementById("CourseFeeDiscountPanel").style.opacity = "1.0";
//        //document.getElementById("CourseFeeDiscountPanel").style.pointerEvents = "All";
//      //  document.getElementById("commonFeeDiscountSave").removeAttribute("disabled");
//    }
//}


function checkDiscountAmount(e) {

    if (e.values.Discount.length == 0) {
        e.value.set("Discount", 0);
        return;
    }
    if (e.values && (e.values.Discount))
    {

        var Discount = e.values.Discount || e.model.Discount;
        var fee = e.values.Fee || e.model.Fee;
        if (Discount > fee) {
            ShowMessage("Discount Cant be greater than Fee " + fee);
            e.value.set("Discount", 0);
        }
    }
}