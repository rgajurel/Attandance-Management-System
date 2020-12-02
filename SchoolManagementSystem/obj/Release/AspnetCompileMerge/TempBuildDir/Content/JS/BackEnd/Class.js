$(document).ready(function () {
    var obj = [];
    InitializeSection();
    // $("#SectionID").multiselect();
    $(".create").off().on('click', function (e)
    {   InitializeSection();
       
        $("#hide").fadeIn(500);
        $("#show").hide();
        for (var i = 0; i < obj.length; i++) {
            $('#SectionID')[0].sumo.unSelectItem(obj[i]);
        }
    })

    $(".cancel").off().on('click', function (e) {
        InitializeSection();
      
        for (var i = 0; i < obj.length; i++) {
            $('#SectionID')[0].sumo.unSelectItem(obj[i]);
        }
        $("#hide").hide();
        $("#show").fadeIn(500);
       
    });



    $("#Save").off().on('click', function (e)
    {      
        
        $('#SectionIDs').text("");
        var selected = [];
        selected = $("#SectionID").val();
        var selectedlength = $("select[name='SectionID'] option:selected").length;

        if (!$('form#formClass').data('unobtrusiveValidation').validate()) {
           
            e.preventDefault();
            
        }

        if (selectedlength == 0) {

            $('#SectionIDs').text("Required");
            return false;
        }
        else {
            $.ajax({
                url: "/Admin/Class/SaveClass",
                type: 'POST',
                dataType: 'json',
                data: {
                    ID: $('#ID').val(),
                    ClassTypeID: $('#ClassTypeID').val(),
                    ClassID: $('#ClassID').val(),
                    FacultyID: $('#FacultyID').val(),
                    SectionArray: selected,
                    __RequestVerificationToken: $('form input[name=__RequestVerificationToken]').val(),
                },

                success: function (data) {


                    ResetFormData();
                    ShowMessage(data.Message);
                    $("#ClassGrid").data("kendoGrid").dataSource.read();                  
                    ResetSection();
                    $("#hide").hide();
                    $("#show").fadeIn(500);
                  



                }
            })

        }


    });


    $("#FieldFilter").keyup(function () {
        var value = $("#FieldFilter").val();
        $("#ClassGrid").data("kendoGrid").dataSource.filter({
            logic: "or",
            filters: [
                {
                    field: "ClassName",
                    operator: "contains",
                    value: value
                },
                {
                    field: "Faculty",
                    operator: "contains",
                    value: value
                }
            ]
        });


    });
});

function ResetSection()
{
    var obj = [];
    $('option:selected').each(function () {
        obj.push($(this).index());
    });

    for (var i = 0; i < obj.length; i++) {
        $('#SectionID')[0].sumo.unSelectItem(obj[i]);
    }
   
}
function Edit(e)
{
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
       

    $.ajax({
        url: "/Admin/Class/EditClass",
        data: {id: dataItem.ID },
        type: "POST",
        dataType: "json",

        success: function (result)
         {            
            
            $("#ID").val(result.ID);
            $("#ClassTypeID").val(result.ClassTypeID);
            $("#ClassID").val(result.ClassID);      
            $("#FacultyID").val(result.FacultyID);
            $('#SectionID')[0].sumo.unSelectAll();
            var sectionArray = result.Sections.split(",");

            var selectbox = $('#SectionID')[0];
            for (var i = 0; i < sectionArray.length; i++)
            {
                selectbox.sumo.selectItem(sectionArray[i]);
            }
            $("#hide").fadeIn(500);
            $("#show").hide();
        }
        //error: function (e, xhr)
        //{
        //    console.log(xhr.status)
           
        //    debugger;
        //    ShowMessage('Error Occured');
        //}
    });

}
function Delete(e) {

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    e.preventDefault();
    $("#window").kendoWindow({
        modal: true
    });
    $("#window").data("kendoWindow").open().center();

    $("#yes").off().on('click', function (e) {

        $.ajax({
            url: "/Admin/Class/DeleteClass",
            data: { id: dataItem.ID },
            type: 'POST',
            dataType: 'json',
            success: function (result) {

                $("#window").data("kendoWindow").close();
                ShowMessage(result.Message);
                ResetFormData();
                ResetSection();
                $("#ClassGrid").data("kendoGrid").dataSource.read();
                document.getElementsByClassName("panel-title")[0].innerHTML = "Add Class/Course";

            }
        });

    });

    $("#no").off().on('click', function (e) {

        $("#window").data("kendoWindow").close();
         
    });

}

function InitializeSection()
{
    $('#SectionID').SumoSelect({
        okCancelInMulti: true,        
    });   
  
    $('#SectionID').prop("selectedIndex", -1);

}

