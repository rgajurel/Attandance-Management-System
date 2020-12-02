var saladdtotal = 0;
var salsavingtotal = 0;
var employeeid = 0;
salaryinfo = [];
$(document).ready(function () {
    
    
    $(".create").off().on('click', function (e) {
        $("#hide").fadeIn(500);
        $("#show").hide();
    })
    $(".cancel").off().on('click', function (e) {
        
        $("#hide").hide();
        $("#show").fadeIn(500);
        $('#OrganisationID').prop('selectedIndex', 0);
       
      

    });
  
    LoadOrgainsation();
    


    $('#NepaliJoioningDate').nepaliDatePicker({
        ndpEnglishInput: 'EnglishJoioningDate',
        npdMonth: true,
        npdYear: true,
        npdYearCount: -25
    });

    $('#NepaliDateOfBirth').nepaliDatePicker({
        ndpEnglishInput: 'EnglishDateOfBirth',
        npdMonth: true,
        npdYear: true,
        npdYearCount: -25
    });

    $('#NepaliJoioningDate').change(function () {
        $('#EnglishJoioningDate').val(BS2AD($('#NepaliJoioningDate').val()));
    });

    $('#NepaliDateOfBirth').change(function () {
        $('#EnglishDateOfBirth').val(BS2AD($('#NepaliDateOfBirth').val()));
    });

    //$('#EnglishJoioningDate').change(function () {
    //    $('#NepaliJoioningDate').val(AD2BS($('#EnglishJoioningDate').val()));
    //});





    $("a[href=#contacts]").click(function () {
        $("#personalinfo").hide();
        $("#parentsdetails").hide();
        $("#previousqualification").hide();
        $("#contactsdetails").show();


    })

    $("#personal").off().on('click', function (e) {
        ShowPersonelHideOthers();

    });

    $("#parents").off().on('click', function (e) {

        $("#personalinfo").hide();
        $("#contactsdetails").hide();
        $("#parentsdetails").show();
        $("#previousqualification").hide();

    });


    $("#EntryTime").timepicker({
        timeFormat: 'HH:mm:ss '
    });
    $("#ExitTime").timepicker({
        timeFormat: 'HH:mm:ss'
    });

    $(document).bind('keypress', function (e) {

        if (e.keyCode == 13) {
            $('#employeeSearch').trigger('click');
        }
    });

    $('#employeeSearch').click(function () {
        //var grid = $("#EmployeeListGrid").data("kendoGrid");
        // grid.dataSource.transport.options.read.global = false;
        $("#EmployeeListGrid").data("kendoGrid").dataSource.read();

    });

    $("#employeeSubmit").off().on('click', function (e) {


        if (!$('form#formEmployee').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else {
            var formData = new FormData();
            formData.append("ID", $("#ID").val());
            formData.append("Name", $("#Name").val());
            formData.append("Gender", $("#Gender").val());
            formData.append("EnglishJoioningDate", $("#EnglishJoioningDate").val());            
            formData.append("Qualifications", $("#Qualifications").val());
            formData.append("EnglishDateOfBirth", $("#EnglishDateOfBirth").val());           
            formData.append("Email", $("#Email").val());
            formData.append("MobileNo", $("#MobileNo").val());
            formData.append("PhoneNo", $("#PhoneNo").val());
            formData.append("CitizenshipNo", $("#CitizenshipNo").val());
            formData.append("imageFile", $('#inputFile')[0].files[0]);
            formData.append("UserID", $("#UserID").val());
            formData.append("OrganisationID", $("#OrganisationID").val());
            formData.append("DepartmentID", $("#DepartmentID").val());
            formData.append("DesignationID", $("#DesignationID").val());
            formData.append("JobTypeID", $("#JobTypeID").val());
            formData.append("EntryTime", $("#EntryTime").val());
            formData.append("ExitTime", $("#ExitTime").val());
            formData.append("Status", $("#Status").val());
            formData.append("FatherName", $("#FatherName ").val());
            formData.append("PermanentAddress", $("#PermanentAddress").val());
            formData.append("TemporaryAddress", $("#TemporaryAddress").val());

            formData.append('__RequestVerificationToken', $('form input[name=__RequestVerificationToken]').val());
            $.ajax({
                url: "/Admin/Employer/SaveEmployer",
                type: 'POST',
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (data) {
                    ResetFormData1();
                    $("#hide").hide();
                    $("#show").fadeIn(500);
                    ShowMessage(data.Message,true);
                    $("#EmployeeListGrid").data("kendoGrid").dataSource.read();


                },
                error: function (response) {
                    ShowMessage("Error Occured",false)
                }
            })
        }
    });

    $("#studentCancel").off().on('click', function (e) {
        ResetFormData1();
    });


    $("#employeeFormShow").off().on(' click', function (e) {

        $("#employeeForm").fadeIn(800);
        $("#employeeList").hide();

    })

    $("#calculatetax").click(function () {
        
        debugger;
        var dd = employeeid;
        var taxableamount = parseFloat($('#saladdtotal').val()) - parseFloat($('#salsavingtotal').val())
         $.ajax({
        url: "/Admin/SalaryCalculation/CalculateTax",
        type: 'POST',
        dataType: 'json',
        data: {
            taxableamount: taxableamount, Employeeid: employeeid
        },
        global: false,

        success: function (data) {
            $("#saltax").empty();
            htmltax = "";
            htmltax += '<table class="table table-bordered tax"><thead><tr><th class="hide">SortOrder</th><th class="hide">SalaryHeadID</th><th>Salary Heading</th><th style="text-align:center">Amount</th></tr></thead><tbody>';
            jQuery.each(data.TaxInfo, function (index, value) {
                htmltax += '<tr><td class="hide">' + value.SortOrder + '</td><td class="hide">' + value.SalaryHeadingID + '</td><td class="black">' + value.SalHeadingName + '</td><td><input disabled class="saltax brown form-control" type="text" pattern="^[0-9]*\.[0-9]{2}$" value=' + value.Amount + '></input></td></tr>';

            });

            htmltax += '<tr><td class="hide">' + data.SalInfoFinalTotal.SortOrder + '</td><td class="hide">' + data.SalInfoFinalTotal.SalaryHeadingID + '</td><td class="black">' + data.SalInfoFinalTotal.SalHeadingName + '</td><td><input disabled class="saltax brown form-control"  type="text" pattern="^[0-9]*\.[0-9]{2}$" value=' + data.SalInfoFinalTotal.Amount + '></input></td></tr>';
            htmltax += '</table>';
            $("#saltax").append(htmltax);

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured", false);  //
        }



    })
    });

    $("#Save").click(function () {
        Year = $("#Year").text();
        Month = $("#Month").text();
        if (!confirm("Are you calculating salary of Year "+Year+ " and Month "+Month)) { //      
            return false;
        }
        salaryinfo = [];
        PushDataToArray();
        
        $.ajax({
            url: "/Admin/SalaryCalculation/SaveCalculatedSalary",
            type: 'POST',
            dataType: 'json',
            data: {
                salarycalculate: salaryinfo
            },
            global: false,
            success: function (data) {
               
                ShowMessage(data.Message, true);
                
               
            },

            error: function (response) {

                ShowMessage("Warning! Error Occured", false);  //
            }



        })
    });

    $("#OrganisationSearchID").change(function () {


        var organisation = $("#OrganisationSearchID").val();

        GetDepartmentBasedOnOrganisation(organisation);
        GetDesignationBasedOnOrganisation(organisation);


    });
    $("#OrganisationID").change(function () {


        var organisation = $("#OrganisationID").val();

        GetDepartmentBasedOnOrganisation1(organisation, null);
        GetDesignationBasedOnOrganisation1(organisation, null);


    });
       
    $(document).on("keyup", "input:text.salaryadd", function () {
        
        try
        {
            saladdtotal = 0;
            
            if (this.value === "" || this.value==0)
            {
                this.value = 0;
            }            
            $('.salaryadd').each(function (i, obj) {
               
                saladdtotal += parseFloat(this.value)
                console.log(saladdtotal)
            });

            $('#saladdtotal').val(saladdtotal);
            $('#calculatetax').trigger('click');
        }

        catch(error)
        {
            debugger;
            ShowMessage("Invalid Data", true);
        }
        
          
    });


    $(document).on("keyup", "input:text.salsaving", function () {

        try {
            salsavingtotal = 0;

            if (this.value === "") {
                this.value = 0;
            }
            $('.salsaving').each(function (i, obj) {

                salsavingtotal += parseFloat(this.value)
                
            });

            $('#salsavingtotal').val(salsavingtotal);
            $('#calculatetax').trigger('click');
        }
        catch (error) {
            debugger;
            ShowMessage("Invalid Data", true);
        }


    });


});



function PushDataToArray() {

    debugger;
    var tableadd = $(".add tbody");
    tableadd.find('tr').each(function (i)
    {
        var $tds = $(this).find('td'),
            SortOrder = $tds.eq(0).text(),
            SalaryHeadingID = $tds.eq(1).text(),
                SalHeadingName=$tds.eq(2).text(),
            Amount = ($tds.find('input.salaryadd').val() || $tds.find('input#saladdtotal').val());
            Year = $("#Year").val();
            Month = $("#Month").val();
            salaryinfo.push({SalHeadingName:SalHeadingName, SortOrder: SortOrder, SalaryHeadingID: SalaryHeadingID, Amount: Amount, Year: Year, Month: Month,EmployeeID:employeeid });           
        
        
    });
    var tablesaving = $(".saving tbody");
    tablesaving.find('tr').each(function (i) {
        var $tds = $(this).find('td'),
            SortOrder = $tds.eq(0).text(),
            SalaryHeadingID = $tds.eq(1).text(),
             SalHeadingName = $tds.eq(2).text(),
            Amount = $tds.find('input.salsaving').val();
        Year = $("#Year").val();
        Month = $("#Month").val();
        salaryinfo.push({SalHeadingName:SalHeadingName, SortOrder: SortOrder, SalaryHeadingID: SalaryHeadingID, Amount: Amount, Year: Year, Month: Month,EmployeeID:employeeid });           

    });
    var tabletax = $(".tax tbody");
    tabletax.find('tr').each(function (i) {
        var $tds = $(this).find('td'),
            SortOrder = $tds.eq(0).text(),
            SalaryHeadingID = $tds.eq(1).text(),
             SalHeadingName = $tds.eq(2).text(),
            Amount = $tds.find('input.saltax').val();
        Year = $("#Year").val();
        Month = $("#Month").val();
        salaryinfo.push({SalHeadingName:SalHeadingName, SortOrder: SortOrder, SalaryHeadingID: SalaryHeadingID, Amount: Amount, Year: Year, Month: Month,EmployeeID:employeeid });           

    });
    console.log(salaryinfo)

}
function resetRowNumber1(e) {
    $(".k-grid-Edit").addClass("buttoncolor");
    $(".k-grid-Edit").removeClass("k-button");
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

function ResetFormData1() {
    $('input:checkbox').removeAttr('checked');
    $("#ID").val("0");
    $("#DepartmentID").empty();
    $("#DepartmentID").append('<option value>--Department--</option>')
    $("#DesignationID").empty();
    $("#DesignationID").append('<option value>--Designation--</option>')
    $("input[type='text'], textarea,input[type='number'], input[type='password'],input[type='checkbox']").each(
      function ()
      {
          $(this).val('');

      }
    );
}
function Calculate(e)
{
    
    debugger;
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    employeeid = dataItem.ID;
    e.preventDefault();    
    $("#name").text(dataItem.Name);    
    $("#organisation").text(dataItem.Organisation);
    $("#department").text(dataItem.DepartmentName);
    $("#designation").text(dataItem.Designation);
    $("#imageupload").attr("src", dataItem.Image);
    $('#salarycalculatepopup').modal({ show: true, backdrop: 'static', keyboard: false })

    LoadLeaveInformation(dataItem.ID);
    LoadAttandanceInformation(dataItem.ID);
    LoadSalaryInformation(dataItem.ID);
}

function LoadLeaveInformation(employeeid) {

    $("#leaveinfo").empty();
    var month = $("#Month").val();;
    var year = $("#Year").val();
    var html = "";
    $.ajax({
        url: "/Admin/SalaryCalculation/GetEmployeeLeaveInformation",
        type: 'POST',
        dataType: 'json',
        data: {
            id: employeeid,Year:year,Month:month
        },
        global: false,
        success: function (data) {
            if (data.length == 0)
            {                
                               
               html += '<div class="col-md-6"><label class="brown">Records Not Found/Leave Not Taken</label></div>';
            }
            else
            { 
                jQuery.each(data, function (index, value)
                {
                    html += '<div class="col-md-4"><label class="black"> ' + value.LeaveName + ' :</label><label class="brown"> ' + value.Days + '</label></div>';                 

                });
                html += '<div class="col-md-4"><label class="brown">TOTAL LEAVE TAKEN:</label><label class="black"> ' + data[0].Total + '</label></div>';
            }
            
           
            $("#leaveinfo").append(html);
           

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured", false);  //
        }



    })
}
function LoadAttandanceInformation(employeeid) {

    $("#attandanceinfo").empty();
    var month = $("#Month").val();;
    var year = $("#Year").val();
    var html = "";
    $.ajax({
        url: "/Admin/SalaryCalculation/GetEmployeeAttandanceInformation",
        type: 'POST',
        dataType: 'json',
        data: {
            id: employeeid, Year: year, Month: month
        },
        global: false,
        success: function (data) {
            if (data.length == 0) {


                html += '<div class="col-md-6"><label class="brown">Records Not Found</label></div>';
            }
            else {
                jQuery.each(data, function (index, value) {
                    html += '<div class="col-md-4"><label class="black"> ' + value.AttandanceName + ' :</label><label class="brown"> ' + value.Days + '</label></div>';


                });


            }
            html += '<div class="col-md-4"><label class="brown">TOTAL ATTANDANCE:</label><label class="black"> ' + data[0].Total + '</label></div>';
            $("#attandanceinfo").append(html);


        },

        error: function (response) {

            ShowMessage("Warning! Error Occured", false);  //
        }



    })
}

function LoadSalaryInformation(employeeid) {

   
    var month = $("#Month").val();
    var year = $("#Year").val();
    var htmladd = "";
    var htmlsubtract = "";
    var htmltax = "";
    $.ajax({
        url: "/Admin/SalaryCalculation/GetEmployeeSalaryList",
        type: 'POST',
        dataType: 'json',
        data: {
            id: employeeid
        },
        global: false,
        success: function (data)
        {
              $("#saladd").empty();
              $("#salsubtract").empty();
              $("#saltax").empty();
            if (data.length == 0) {

                html += '<div class="col-md-6"><label class="brown">Records Not Found</label></div>';
            }
            else
            {
              //salary add region
                htmladd += '<table class="table table-bordered add"><thead><tr><th class="hide">SortOrder</th><th class="hide">SalaryHeadID</th><th class="hide"></th><th>Salary Heading</th><th style="text-align:center">Amount</th></tr></thead><tbody>';
                jQuery.each(data.SalInfoAdd, function (index, value) {
                    htmladd += '<tr><td class="hide">' + value.SortOrder + '</td><td class="hide">' + value.SalaryHeadingID + '</td><td class="black">' + value.SalHeadingName + '</td><td><input class="salaryadd brown form-control" type="text" pattern="^[0-9]*\.[0-9]{2}$" value=' + value.Amount + '></input></td></tr>';
                    
                });

                htmladd += '<tr><td class="hide">' + data.SalAddInfoTotal.SortOrder + '</td><td class="hide">' + data.SalAddInfoTotal.SalaryHeadingID + '</td><td class="black">' + data.SalAddInfoTotal.SalHeadingName + '</td><td><input disabled class="brown form-control" id="saladdtotal" type="text" pattern="^[0-9]*\.[0-9]{2}$" value=' + data.SalAddInfoTotal.Amount + '></input></td></tr>';
                htmladd += '</tbody></table>';
                $("#saladd").append(htmladd);               
                
                //

                //salary saving region
                htmlsubtract += '<table class="table table-bordered saving"><thead><tr><th class="hide">SortOrder</th><th class="hide">SalaryHeadID</th><th>Salary Heading</th><th style="text-align:center">Amount</th></tr></thead><tbody>';
                jQuery.each(data.SalInfoSaving, function (index, value) {
                    htmlsubtract += '<tr><td class="hide">' + value.SortOrder + '</td><td class="hide">' + value.SalaryHeadingID + '</td><td class="black">' + value.SalHeadingName + '</td><td><input class="salsaving brown form-control" type="text" pattern="^[0-9]*\.[0-9]{2}$" value=' + value.Amount + '></input></td></tr>';

                });

                htmlsubtract += '<tr><td class="hide">' + data.SalInfoSavingTotal.SortOrder + '</td><td class="hide">' + data.SalInfoSavingTotal.SalaryHeadingID + '</td><td class="black">' + data.SalInfoSavingTotal.SalHeadingName + '</td><td><input disabled class="brown form-control" id="salsavingtotal" type="text" pattern="^[0-9]*\.[0-9]{2}$" value=' + data.SalInfoSavingTotal.Amount + '></input></td></tr>';
                htmlsubtract += '</table>';
                $("#salsubtract").append(htmlsubtract);

                //

                //salary deduct region or tax

                htmltax += '<table class="table table-bordered tax"><thead><tr><th class="hide">SortOrder</th><th class="hide">SalaryHeadID</th><th>Salary Heading</th><th style="text-align:center">Amount</th></tr></thead><tbody>';
                jQuery.each(data.TaxInfo, function (index, value) {
                    htmltax += '<tr><td class="hide">' + value.SortOrder + '</td><td class="hide">' + value.SalaryHeadingID + '</td><td class="black">' + value.SalHeadingName + '</td><td><input disabled class="saltax brown form-control" type="text" pattern="^[0-9]*\.[0-9]{2}$" value=' + value.Amount + '></input></td></tr>';

                });

                htmltax += '<tr><td class="hide">' + data.SalInfoFinalTotal.SortOrder + '</td><td class="hide">' + data.SalInfoFinalTotal.SalaryHeadingID + '</td><td class="black">' + data.SalInfoFinalTotal.SalHeadingName + '</td><td><input disabled class="saltax brown form-control"  type="text" pattern="^[0-9]*\.[0-9]{2}$" value=' + data.SalInfoFinalTotal.Amount + '></input></td></tr>';
                htmltax += '</table>';
                $("#saltax").append(htmltax);
                                
            }
            


        },

        error: function (response) {

            ShowMessage("Warning! Error Occured", false);  //
        }



    })
}


function GetDepartmentBasedOnOrganisation1(organisation, department) {
   
    $("#DepartmentID").empty();
    $("#DepartmentID").append('<option value>--Department--</option>')
    $.ajax({
        url: "/Admin/Employer/GetDepartmentBasedOnOrganisation",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: organisation,
        },
        global:false,
        success: function (data) {

            jQuery.each(data, function (index, value) {
                if (value.ID === department)
                {
                    $("#DepartmentID").append('<option selected value=' + value.ID + '>' + value.DepartmentName + '</option>')
                }
                else {
                $("#DepartmentID").append('<option value=' + value.ID + '>' + value.DepartmentName + '</option>')
                 }

            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured",false);  //
        }



    })

}

function ShowPersonelHideOthers() {
    $("#personalinfo").show();
    $("#contactsdetails").hide();
    $("#parentsdetails").hide();
    $("#previousqualification").hide();
}

function GetDesignationBasedOnOrganisation1(organisation, designation) {
    $("#DesignationID").empty();
    $("#DesignationID").append('<option value>--Designation--</option>')
    $.ajax({
        url: "/Admin/Employer/GetDesignationBasedOnOrganisation",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: organisation,
        },
        global:false,
        success: function (data) {


            jQuery.each(data, function (index, value) {

                if (value.ID === designation)
                {
                    $("#DesignationID").append('<option selected value=' + value.ID + '>' + value.Designation + '</option>')
                }
                else {
                    $("#DesignationID").append('<option value=' + value.ID + '>' + value.Designation + '</option>')
                 }

            });


        },
             error: function (response) 
            {
                ShowMessage("Error Occured",false);
            }   
    })

}


function GetDepartmentBasedOnOrganisation(organisation, faculty)
{
   
    $("#DepartmentSearchID").empty();
    $("#DepartmentSearchID").append('<option value>--Department--</option>')
    $.ajax({
        url: "/Admin/Employer/GetDepartmentBasedOnOrganisation",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: organisation,
        },
        global:false,
        success: function (data)
        {
           
            jQuery.each(data, function (index, value)
            {
                    
                $("#DepartmentSearchID").append('<option value=' + value.ID + '>' + value.DepartmentName + '</option>')
                   

                })           
         
          
            },

        error: function (response) 
        {
            ShowMessage("Error Occured",false);
        }        
           


        })
   
}

function GetDesignationBasedOnOrganisation(organisation)
{
    $("#DesignationSearchID").empty();
    $("#DesignationSearchID").append('<option value>--Designation--</option>')
    $.ajax({
        url: "/Admin/Employer/GetDesignationBasedOnOrganisation",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: organisation,
        },
        global:false,
        success: function (data) {


            jQuery.each(data, function (index, value) {

               
                $("#DesignationSearchID").append('<option value=' + value.ID + '>' + value.Designation + '</option>')
               

            });


        },
        error: function (response) 
        {
            ShowMessage("Warning!Error Occured",false);
        }   
    })

}

ConvertDateObjectToDate = function (dateObject)
{   
    var dateString = dateObject.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var date = month + "-" + day + "-" + year;    
    return date;
};


function ParamToEmployeeList(e) {
    var grid = $("#EmployeeListGrid").data("kendoGrid").dataSource;
    return {
        EmployerSearchName: $("#EmployerSearchName").val() == "" ? "" : $("#EmployerSearchName").val(),
        OrganisationSearchID: $("#OrganisationSearchID :selected").val() == "" ? -1 : $("#OrganisationSearchID :selected").val(),
        DepartmentSearchID: $("#DepartmentSearchID :selected").val() == "" ? -1 : $("#DepartmentSearchID :selected").val(),
        DesignationSearchID: $("#DesignationSearchID").val() == "" ? -1 : $("#DesignationSearchID").val(),
        UserIDSearch: $("#UserIDSearch").val() == "" ? -1 : $("#UserIDSearch").val(),
        pageSize: grid._pageSize,
        pageNumber: grid._page

    };

}

function GetDeviceUserID() {
    $.ajax({
        url: "/Admin/Employer/GetUniqueDeivceUserID",
        type: 'POST',
        dataType: 'json',
        global: false,
        data: {
            UserID: "1",
        },
        success: function (data) {          
            $('#UserID').val(data.UserID)
            $("#UserID").attr("disabled", "disabled");
            // $('#RegistrationNo').attr("disabled", "disabled");

        }
    })

}

function LoadOrgainsation() {
    $("#OrganisationSearchID").empty();
    $("#OrganisationSearchID").append('<option value>--Organisation--</option>')
    
    
    $.ajax({
        url: "/Admin/DropDown/GetOrganisationDropDown",
        type: 'POST',
        dataType: 'json',
        global: false,
        success: function (data) {
          
            jQuery.each(data, function (index, value) 
            {
                $("#OrganisationSearchID").append('<option value=' + value.ID + '>' + value.Name + '</option>')
                $("#OrganisationID").append('<option value=' + value.ID + '>' + value.Name + '</option>')
            })

        },

        error: function (response) {

            ShowMessage("Warning! Error Occured",false);  //
        }



    })

}





