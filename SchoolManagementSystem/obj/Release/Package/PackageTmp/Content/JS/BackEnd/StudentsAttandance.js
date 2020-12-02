$(document).ready(function () {      
  
    $("#StudentsAttandanceSearch").off().on('click', function (e) {
        if (!$('form#formStudentsAttandanceEntry').data('unobtrusiveValidation').validate()) 
        {
            e.preventDefault();
            return false;
        }
        else
         {
            $("#StudentsAttadanceList").data("kendoGrid").dataSource.read();
            // $("#marksEntrySearch").hide();
        }

    })
     $("#addTotalDays").off().on('click', function (e)
    {
        if (!$('form#formStudentsTotalAttandanceEntry').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        }
        else 
        {
            var dataItem;
            dataItem = $("#StudentsAttadanceList").data("kendoGrid").dataSource.data();
            for (i = 0; i < dataItem.length; i++)
            {
                firstItem = $('#StudentsAttadanceList').data().kendoGrid.dataSource.data()[i];
                firstItem["TotalDays"] = $("#TotalDays").val();

               $('#StudentsAttadanceList').data('kendoGrid').refresh();
                // $("#marksEntrySearch").hide();
            }
        }

    })
     $('#StudentsAttadanceList').off().on('click', '.chkbx', function ()
     {
         debugger;
        
       var checked = $(this).is(':checked');
        var grid = $('#StudentsAttadanceList').data().kendoGrid;
        var dataItem = grid.dataItem($(this).closest('tr'));
        row = $(this).closest("tr");
        checkedIds[dataItem.SN] = checked;
        dataItem.set('IsAttend', checked);


        var view = $("#StudentsAttadanceList").data("kendoGrid").dataSource.view();
        for (var i = 0; i < view.length; i++) {

            if (checkedIds[view[i].SN]) {
                grid.tbody.find("tr[data-uid='" + view[i].uid + "']")
                .addClass("k-alt k-state-selected")
                .find(".chkbx")
                .attr("checked", "checked");
            }
            else {
                grid.tbody.find("tr[data-uid='" + view[i].uid + "']")
                row.removeClass("k-alt k-state-selected");
            }
        }
    })
    
     $("#StudentsAttandanceSearch").off().on('click', function (e)
     {         
         if (!$('form#formStudentsAttandanceEntry').data('unobtrusiveValidation').validate())
         {
             e.preventDefault();
             return false;
         } else {
             $("#StudentsAttadanceList").data("kendoGrid").dataSource.read();
         }
         // $("#marksEntrySearch").click();
     }
     );
    $("#FacultyID").change(function () {

        var faculty = $("#FacultyID").val();
        var classes = $("#ClassID").val();

          GetSectionBasedOnClassAndFaculty(classes, faculty);
         
        // GetSectionBasedOnClass(classs);

    });
    $("#ClassID").change(function ()
    {
        var classs = $("#ClassID").val();
        GetFacultyBasedOnClass(classs);          
      
    });

    $("#marksSaveRecords").off().on('click', function (e)
    {
        debugger;
        if (!$('form#formStudentsTotalAttandanceEntry').data('unobtrusiveValidation').validate()) {
            e.preventDefault();
            return false;
        } else
        {
            e.preventDefault();
            var batchstudentattandance = $("#StudentsAttadanceList").data("kendoGrid").dataSource.data();
            $.ajax({
                url: "/Admin/StudentsAttandance/SaveStudentsAttandance",
                type: 'POST',
                data: { data: JSON.stringify(batchstudentattandance) },
                dataType: 'json',
                success: function (data)
                {
                    ShowMessage(data.Message);
                    $('.divAttandanceError').empty();
                    if (data.ErrorOccured == true)
                    {
                        var html = '';
                        html += '<table class="AttandanceListWithError">';
                        html += '<tr>';
                        html += '<td>SN</td>';
                        html += '<td>StudentName</td>';
                        html += '<td>RollNo</td>';
                        html += '<td>Present Days</td>';
                        html += '<td>TotalDays</td>';                       
                        html += '</tr>';
                        $.each(data.Data, function (index, item) {
                            html += '<tr>';
                            html += '<td>' + item.SN + '</td>';
                            html += '<td>' + item.StudentName + '</td>';
                            html += '<td>' + item.RollNo + '</td>';
                            html += '<td>' + item.PresentDays + '</td>';
                            html += '<td>' + item.TotalDays + '</td>';                           
                            html += '</tr>';
                            
                        })
                        html += '</table>';

                        $('.divAttandanceError').append(html);
                        $(function () {
                            $(".AttandanceListWithError").table2excel({
                                exclude: ".noExl",
                                name: "AttandanceEntryError",
                                fileext: ".xlsx",
                                exclude_img: true,
                                exclude_links: true,
                                exclude_inputs: true
                            });
                        });
                    }
                    else
                    {
                        ShowMessage(data.Message);
                    }

                  

                }
            })

        }
    })

 
});

function GetFacultyBasedOnClass(classs, faculty) {
    $("#FacultyID").empty();
    $("#FacultyID").append('<option value>--Select--</option>')
    $.ajax({
        url: "/Admin/Students/GetFacultyBaseOnClass",
        type: 'POST',
        dataType: 'json',
        data: {
            ID: classs,
        },
        global:false,
        success: function (data) {


            jQuery.each(data, function (index, value) {

                if (value.ID === faculty) {
                    $("#FacultyID").append('<option selected value=' + value.ID + '>' + value.Faculty + '</option>')
                }
                else {
                    $("#FacultyID").append('<option value=' + value.ID + '>' + value.Faculty + '</option>')
                }

            });


        }
    })

}

function GetSectionBasedOnClassAndFaculty(classs, faculty, section)
{
   
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
        global:false,
        success: function (data)
        {
            var sectionArray = data.Sections.split(',');
            jQuery.each(sectionArray, function (index, value)
            {
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

function ParamToAttandanceList(e) {
    var grid = $("#StudentsAttadanceList").data("kendoGrid").dataSource;
    return {
        SessionID: $("#SessionID :selected").val() == "" ? -1 : $("#SessionID :selected").val(),
        ClassID: $("#ClassID :selected").val() == "" ? -1 : $("#ClassID :selected").val(),
        Section: $("#Section :selected").val() == "" ? "" : $("#Section :selected").val(),       
        FacultyID: $("#FacultyID").val() == "" ? -1 : $("#FacultyID").val(),
        TermID: $("#TermID").val() == "" ? -1 : $("#TermID").val(),       

        // TermID: $("#TermID").val() == "" ? -1 : $("#TermID").val(),      
        //pageSize: grid._pageSize,
        //pageNumber: grid._page
    };

}



function Save(e) {
    debugger;

    if (e.values && (e.values.PresentDays))
    {
        var grid = $("#StudentsAttadanceList").data("kendoGrid");
        var dataItem = grid.dataItem(grid.current().closest("tr"));
      
      
        $('#StudentsAttadanceList').data('kendoGrid').refresh();
        var totaldays = e.values.TotalDays || e.model.TotalDays;
        var presentdays = e.values.PresentDays || e.model.PresentDays;       

        if (parseInt(presentdays) > parseInt(totaldays) || parseInt(presentdays) <= 0)
        {
            ShowMessage("Present Days is greater than " + totaldays + " Or Less than 0");
           // e.model.set("PresentDays", 0);
           // dataItem.set("PresentDays", "0");
           
            $('#StudentsAttadanceList').data('kendoGrid').refresh();
            //    this.val(0);

            
        }
        else
        {
            e.model.set("IsAttend", true);
        }
    }
}


function resetRowNumberAttandanceEntryGrid(e)
{    
    var rows = e.sender.tbody[0].rows;
    $(rows).each(function (e)
    {
        var grid = $("#StudentsAttadanceList").data("kendoGrid");
        var row = this;
        var dataItem = grid.dataItem(row);

        if (dataItem.IsAttend == true)
        {
            grid.tbody.find("tr[data-uid='" + dataItem.uid + "']")
           //.addClass("k-alt k-state-selected gridselect")
            // .find(".chkbx")
            // .attr("checked", "checked");
        }
    })
    $(".k-grid-Edit").find("span").addClass("fa fa-pencil");
    $(".k-grid-Edit").removeClass("k-button");
    $(".k-grid-Delete").find("span").addClass("fa fa-trash");
    $(".k-grid-Delete").removeClass("k-button");
    $(".k-grid-Details").find("span").addClass("fa fa-eye");
    $(".k-grid-Details").removeClass("k-button");
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

function onError(e, status) {
    ShowMessage('Warning ! Error Occured');
}