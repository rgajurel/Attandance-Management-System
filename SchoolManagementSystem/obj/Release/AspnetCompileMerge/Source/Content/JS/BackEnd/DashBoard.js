
    var employee = [];
    var datestring = [];
    var students = [];
    var classWiseStudent = [];
    TotalStudentsClassWise(classWiseStudent);
     Init();  
      
    TotalAttandance(students, employee);
 
    UpcomingBirthDays();
 


function TotalAttandance(students,employee)
{
    $.ajax({
        url: "/Admin/DashBoard/GetAllStudentsAttendance",
        type: "POST",
        dataType: "json",
       
        async:true,
        success: function (result)
        {
              
            if (result.length > 0)
            {
              
                $.each(result, function (index, item)
                {
                    datestring = item.Dates.split("/");                  
                                
                    if (item.Category == 1) {
                        employee.push({ x: new Date(datestring[2], datestring[0] - 1, datestring[1]), y: item.TotalAttend });
                    }
                    else
                    {
                        students.push({ x: new Date(datestring[2], datestring[0] - 1, datestring[1]), y: item.TotalAttend });
                    }                    
                    
                })
              var chart = new CanvasJS.Chart("chartContainer",
    {
        animationEnabled: true,
        theme: "light2",
        zoomEnabled: true,
        panEnabled: true,
       
        toolTip: {
                    shared: true
                },
        title:{
            text: "Total Attandance By Day"
        },
        axisX:{
            title: "Days In Month",
            valueFormatString: "DD MMM YYYY",
            crosshair: {
                enabled: true,
                snapToDataPoint: true
            }
        },
        axisY: {
            title: "Total",
            minimum: 0,
            crosshair: {
                enabled: true,
                snapToDataPoint: true
            }
        },
        data: [
            //{
            //    type: "line",
            //    color: "red",
            //    name:"Employee",
            //    showInLegend: true,
            //    //dataPoints:students
            //    dataPoints: [
			//{ x: new Date(2017, 10, 1), y: 63 },
			//{ x: new Date(2017, 10, 2), y: 69 },
			//{ x: new Date(2017, 10, 3), y: 65 },
			//{ x: new Date(2017, 10, 4), y: 70 },
			//{ x: new Date(2017, 10, 5), y: 71 },
			//{ x: new Date(2017, 10, 6), y: 65 },
			//{ x: new Date(2017, 10, 7), y: 73 },
			//{ x: new Date(2017, 10, 8), y: 96 },
			//{ x: new Date(2017, 10, 9), y: 84 },
			//{ x: new Date(2017, 10, 10), y: 85 },
			//{ x: new Date(2017, 10, 11), y: 86 },
			//{ x: new Date(2017, 10, 12), y: 94 },
			//{ x: new Date(2017, 10, 13), y: 97 },
			//{ x: new Date(2017, 10, 14), y: 86 },
			//{ x: new Date(2017, 10, 15), y: 89 }
            //    ]
                   
            //},
        {        
            type: "line",
            color: "red",
            name: "Employee",
            markerType: "square",
            showInLegend: true,
             dataPoints: employee
            
                       }
         
        ]
    });

              chart.render();
              $('.canvasjs-chart-credit').css("display","none");
              
              
            }          

        }
       
    });
}

function Init() {
    $.ajax({
        url: "/Admin/DashBoard/GetAllOrganisationEvents",
        type: "POST",
        dataType: "json",
       
        async:true,
        success: function (result) {
                     
            if (result.length > 0) {
                $.each(result, function (index, item)
                {
                    
                    $("#events").append("<ul class='to_do'><li><p><strong style='color:#d41b35'>" + item.EventName + "</strong></p></li></ul>");
                    
                });
            }
            else {
                $("#events").append("<ul class='to_do'><li><p><strong style='color:#d41b35'>No Current Events Availiable</strong></p></li></ul>");

            }

        }

    });
}

function UpcomingBirthDays() {
    $.ajax({
        url: "/Admin/DashBoard/GetAllUpComingBirthdays",
        type: "POST",
        dataType: "json",        
        async:true,
        success: function (result) {

            if (result.length > 0)
            {
               
                var panel = result[0].Panel.split('-');
                $("#totalstudent").text(panel[1]);
                $("#totalemployee").text(panel[0]);
                $("#totaluser").text(panel[2]);
                $("#totalsubject").text(panel[3]);                
               
                $.each(result, function (index, item)
                {
                   
                    if (item.IsToday == true)
                    {
                        $("#birthdayevents").append("<ul class='to_do'><li><p>Today is  " + item.Name + "   Birthday !! Wish Birthday</p></li></ul>");
                    }
                    else if (item.dates == "1/1/0001")
                    {
                        $("#birthdayevents").append("<ul class='to_do'><li><p><strong style='color:#d41b35'>No Birthday Availiable For Next 10 days</strong></p></li></ul>");
                    }
                    else 
                    {
                        $("#birthdayevents").append("<ul class='to_do'><li><p>" + item.Name + "</strong>      " + item.dates + "</p></li></ul>");
                       
                    }
                
                    
                   
                });
            }
            else
            {
                $("#birthdayevents").append("<li class='fc-events-container'><a href='#'><strong>No Birthdays Availiable for Next 10 Days</strong></a></li");

            }

        }

    });
}

function TotalStudentsClassWise(classWiseStudent)
{
    $.ajax({
        url: "/Admin/DashBoard/GetAllStudentByClass",
        type: "POST",
        dataType: "json",     
        async: true,
        success: function (result) {
           
            if (result.length > 0) {
                               
                $.each(result, function (index, item) {
                    
                  
                    classWiseStudent.push({ y:item.Total,label:item.Class });
                    //students.push({ x: new Date(datestring[2], datestring[1], datestring[0]), y: item.TotalAttend });
                })
                var chart1 = new CanvasJS.Chart("chartContainer1", {
                    animationEnabled: true,
                    theme: "light2", // "light1", "light2", "dark1", "dark2"
                    title: {
                        text: "Organisation Vs Total Employee"
                    },
                    axisY: {
                        title: "Total",
                        crosshair: {
                            enabled: true,
                            snapToDataPoint: true
                        }
                    },
                    data: [{
                        type: "column",                     
                                    
                        dataPoints: classWiseStudent
                        //    [
                        //    { y: 900, label: "One" },
                        //    { y: 200, label: "Two" },
                        //    { y: 456, label: "Three" },
                        //    { y: 150, label: "Four" },
                        //    { y: 234, label: "Five" },
                        //    { y: 143, label: "Six" },
                        //    { y: 432, label: "Seven" },
                        //    { y: 765, label: "Eight" },
                        //     { y: 143, label: "Nine" },
                        //    { y: 432, label: "Ten" },                            

                        //]
                    }]
                });
                chart1.render();
                


            }

        }

    });
}










   