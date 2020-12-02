$(document).ready(function () {

   

    $("#Class").change(function () {
        var classs = $("#Class").val();
        GetFacultyBasedOnClass(classs);

    });
    $("#PersonalFeeGrid").on("keydown", "#Discount", function (e) {
       
        var arrows = [38, 40]; // Down and Up arrow keys
        var key = e.keyCode;

        if (arrows.indexOf(key) >= 0) {
            //alert('arrow' + key);
            e.preventDefault();
           

            var grid = $("#PersonalFeeGrid").data("kendoGrid");

            var row = $(this).closest("tr");
            //get current row index
            var rowIdx = $("tr", grid.tbody).index(row);

            // to check first row and proceed further else exit - index start from 0 (first row)
            // 38 - Up key, 40 - Down key
            if (key == 38 && rowIdx == 0) {
                return false;
            }

            //get total number of records in grid
            var count = grid.dataSource.total();

            // to check last row and proceed further else exit - index start from 0 (first row)
            if (key == 40 && rowIdx == (count - 1)) {
                return false;
            }

            this.blur();
            row.removeClass('k-state-selected');

            row.trigger("change");

            if (key == 40) {
              
                var nextCell = $(this).closest("tr").next("tr[role='row']").find("td").eq(6);

               
            }
            else if (key == 38) {
                var nextCell = $(this).closest("tr").prev("tr[role='row']").find("td").eq(6);
              
            }
            grid.editCell(nextCell);

           
        }
    });

    $("#Faculty").change(function () {
        var classs = $("#Class").val();
        var faculty = $("#Faculty").val();
        GetSectionBasedOnClassAndFaculty(classs, faculty);

    });
    $("#Month").change(function (e) {
        $("#PersonalFeeGrid").data("kendoGrid").dataSource.read();
    });
    $("#Section").change(function (e) {
        $("#PersonalFeeGrid").data("kendoGrid").dataSource.read();
    });
    $("#Type").change(function (e) {
        $("#PersonalFeeGrid").data("kendoGrid").dataSource.read();
    });

    $("#FieldFilterStudent").keyup(function () {
        var value = $("#FieldFilterStudent").val();
        grid = $("#PersonalFeeGrid").data("kendoGrid");
        rowNumber = 0;
        if (value) {
            grid.dataSource.filter({ field: "StudentName", operator: "startswith", value: value });

        } else {
            grid.dataSource.filter({});
        }
    });



    $("#PersonalFeeSave").off().on('click', function (e) {
        var session = $("#Session").val();
        var classs = $("#Class").val();
        var Faculty = $("#Faculty").val();
        var Section = $("#Section").val();
        var Type = $("#Type").val();
        var month = $("#Month").val();
        if (!$('form#formPersonalFee').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        } else {

            e.preventDefault();
            var batchFeeEntry = $("#PersonalFeeGrid").data("kendoGrid").dataSource.data();
            $.ajax({
                url: "/Admin/PersonalFee/savePersonalFee",
                type: 'POST',
                data: {
                    data1: JSON.stringify(batchFeeEntry), session: session, classs: classs, faculty: Faculty,
                    section: Section, type: Type, month: month
                },

                success: function (data) {
                    $("#PersonalFeeGrid").data("kendoGrid").dataSource.read();

                    //$('select').attr("disabled", false);
                    //document.getElementById("PersonalFeeDiscountPanel").style.opacity = "0.8";
                    //document.getElementById("PersonalFeeDiscountPanel").style.pointerEvents = "None";
                    //$('#PersonalFeeSave').attr('disabled', 'disabled');

                    ShowMessage(data.Message);
                }
            })

        }
    })

});



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

function ParamToDiscountList(e) {
    var grid = $("#PersonalFeeGrid").data("kendoGrid").dataSource;
    return {
        Session: $("#Session :selected").val() == "" ? -1 : $("#Session :selected").val(),
        Class: $("#Class :selected").val() == "" ? -1 : $("#Class :selected").val(),
        Section: $("#Section :selected").val() == "" ? "" : $("#Section :selected").val(),
        Faculty: $("#Faculty :selected").val() == "" ? -1 : $("#Faculty :selected").val(),
        Type: $("#Type :selected").val() == "" ? -1 : $("#Type :selected").val(),
        Month: $("#Month :selected").val() == "" ? -1 : $("#Month :selected").val(),
    };

}

function checkData() {
    var grid = $("#PersonalFeeGrid").data("kendoGrid");
    grid.dataSource.read();
    var count = grid.dataSource.total();
    if (count > 0) {
        $('select').attr('disabled', 'disabled');
        document.getElementById("PersonalFeeDiscountPanel").style.opacity = "1.0";
        document.getElementById("PersonalFeeDiscountPanel").style.pointerEvents = "All";
        document.getElementById("PersonalFeeSave").removeAttribute("disabled");
    }
}


function validateFee(e) {    
    if (e.values && (e.values.Fee) || (e.values.Fee == 0)) {
        var Discount = e.values.Discount || e.model.Discount;
        var fee = e.values.Fee || e.model.Fee;
        if (Discount > fee) {
            ShowMessage("Discount Cant be greater than Fee");
            e.value.set("Discount", 0);
        } else if (e.values.Fee == 0) {
            
            try {
                e.model.set("Discount", 0);

            } catch (ex) {
                e.value.set("Discount", 0);
            }
                        
        }
    } else if (e.values && (e.values.Discount) || (e.values.Discount==0)) {
        var Discount = e.values.Discount || e.model.Discount;
        var fee = e.values.Fee || e.model.Fee;
        if (Discount > fee) {
            ShowMessage("Discount Cant be greater than Fee");
            e.value.set("Discount", 0);
        }
        else if (Discount==0) {
            e.value.set("Discount", 0);
        }
    } else {
        try{
            e.value.set("Fee", 0);
            e.value.set("Discount", 0);
        } catch (ex) {
            e.value.set("Discount", 0);
            e.value.set("Fee", 0);
        }
    }
}

function Delete(e) {
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    if (dataItem.ID == 0) {        
        ShowMessage("This Discount is not saved on database yet.");
        e.preventDefault();
        return;
    }
    e.preventDefault();
    $("#window").kendoWindow({
        modal: true
    });
    $("#window").data("kendoWindow").open().center();   
    $("#yes").off().on('click', function (e) {
        $.ajax({
            url: "/Admin/PersonalFee/DeletePersonalFee",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {
                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message);
                $("#PersonalFeeGrid").data("kendoGrid").dataSource.read();
            }
        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
    });

}

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