YearlyHolidaysList();
GetAllOrganisationEvents();
UpcomingBirthDays();



function GetAllOrganisationEvents() {
    $.ajax({
        url: "/Admin/DashBoard/GetAllOrganisationEvents",
        type: "POST",
        dataType: "json",

        async: true,
        success: function (result) {

            if (result.length > 0) {
                $.each(result, function (index, item) {

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
        async: true,
        success: function (result) {

            if (result.length > 0) {


                $.each(result, function (index, item) {

                    if (item.IsToday == true) {
                        $("#birthdayevents").append("<ul class='to_do'><li><p>Today is  " + item.Name + "   Birthday !! Wish Birthday</p></li></ul>");
                    }
                    else if (item.dates == "1/1/0001") {
                        $("#birthdayevents").append("<ul class='to_do'><li><p><strong style='color:#d41b35'>No Birthday Availiable For Next 10 days</strong></p></li></ul>");
                    }
                    else
                    {
                        $("#birthdayevents").append("<ul class='to_do'><li><p>" + item.Name + "</strong>      " + item.dates + "</p></li></ul>");

                    }



                });
            }
            else {
                $("#birthdayevents").append("<li class='fc-events-container'><a href='#'><strong>No Birthdays Availiable for Next 10 Days</strong></a></li");

            }

        }

    });
}

function YearlyHolidaysList() {
    $.ajax({
        url: "/Admin/DashBoard/GetAllYearlyHolidayList",
        type: "POST",
        dataType: "json",
        async: true,
        success: function (result) {

            if (result.length > 0)
            {            

                $.each(result, function (index, item) {

                   
                    if (item.dates == "1/1/0001")
                    {
                        $("#yearlyholidays").append("<ul class='to_do'><li><p><strong style='color:#d41b35'>No Yearly Holidays List</strong></p></li></ul>");
                    }
                    else
                    {
                        $("#yearlyholidays").append("<ul class='to_do'><li><p><strong style='color:#d41b35'>" + item.Title + ">>>>>>>>" + item.Date + "</strong></p></li></ul>");

                    }



                });
            }
            else {
                $("#yearlyholidays").append("<ul class='to_do'><li><p><strong style='color:#d41b35'>No Yearly Holidays List</strong></p></li></ul>");

            }

        }

    });
}












