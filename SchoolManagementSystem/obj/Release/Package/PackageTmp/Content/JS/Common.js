function CheckDate()
{
    if (isNepaliDate) {
       
        $('.nepalishow').show();
        $('.englishshow').hide();
    }
    else {
      
        $('.nepalishow').hide();
        $('.englishshow').show();
    }
}


function InitialDate()
{
    var date = GetTodayDate();
    $('#DateFrom').val(date);
    $('#DateTo').val(date);
    $('#NepaliDateFrom').val(AD2BS($('#DateFrom').val()));
    $('#NepaliDateTo').val(AD2BS($('#DateTo').val()));

}
function GetTodayDate() {
    var nowDate = new Date();
    return nowDate.getFullYear() + '-' + (nowDate.getMonth() + 1) + '-' + nowDate.getDate();

}
var isNepaliDate = false;
$(document).ready(function () {
    var html = "";
    var previousurl = "";       
     NProgress.start();    
     NProgress.configure({ easing: 'ease', speed: 1000 });
     NProgress.configure({ showSpinner: true });
     NProgress.done();
     GetUserName();
     
    
});
$(document).ajaxStart(function ()
{
    
    NProgress.start();
   
   
});
$(document).ajaxComplete(function ()
{
    NProgress.done();  
   
});
$(document).ajaxError(function (e, xhr) {
    
    if (xhr.status == 302)
    {
        alert("Warning !! Session Expire. Redirect To Login Page");       
        window.location.href = "/Login?ReturnUrl=" + encodeURIComponent(window.location.pathname + "");
    }

    else if (xhr.status == 403)
        ShowMessage("You have no enough permissions to request this resource.", true);
    else if (xhr.status == 404)
        ShowMessage("Warning !! Page Not Found", true);
    else {
        ShowMessage("Warning !! Error Occured");

    }
});



function fancyTimeFormat(time) {
    // Hours, minutes and seconds
    var hrs = ~~(time / 3600);
    var mins = ~~((time % 3600) / 60);
    var secs = time % 60;

    // Output like "1:01" or "4:03:59" or "123:03:59"
    var ret = "";

    if (hrs > 0) {
        ret += "" + hrs + " h " + (mins < 10 ? "0" : "");
    }

    ret += "" + mins + " m " + (secs < 10 ? "0" : "");
    ret += "" + secs + " s ";
    return ret;
}

function GetUserName()
{    
    $.ajax({
        url: "/Admin/Menu/GetLoggedInUserName",
        type: "POST",
        dataType: "json",
        global: false,
        success: function (data) {
            $("#loginusername").text(data.UserName.toUpperCase());
            $("#activeyear").text(data.ActiveYear.toUpperCase());
            isNepaliDate = data.isNepaliDate;
            CheckDate();

           
           
        }
    });
}

function ChangeColor() {
    $(".child_menu li").each(
    function () {
        $(this).removeClass("active");

    }
  );
}

function Init1() {

    $("#hide").fadeIn(500);
    $("#show").hide();

}

function Cancel1() {
    $("#hide").hide();
    $("#show").fadeIn(500);
    $("#IsActive").prop('selectedIndex', 0);
}


$('.child_menu li a').off().on('click', function (e)
{   

    var pathname = window.location.pathname; // Returns path only   
        
    NProgress.start();
    ChangeColor();     
     e.preventDefault(); // prevent default link button redirect behaviour
     var url = $(this).attr("href");
    url1 = url.split("/");
    url2 = url1[0] + "/" + url1[1] + "/" + url1[2];
    if (pathname != url2)
    {    
      $(".loadbody").load(url, function (response, status, xhr) {
         
          if (status == "success")
          {
          history.pushState('', '', url2);
            NProgress.done();
        }
        if (status == "error")
        {
            ShowMessage("Warning ! Error Occured",true);
        }
    });
   
    }
    else {
        NProgress.done();
    }

});

    // Declare a proxy to reference the hub.
    var chat = $.connection.notificationMessage;
    // Create a function that the hub can call to broadcast messages.
    chat.client.allmessage = function (data) {       
        var counter = 0;
        var counter1 = 0;
        var i = 1;
        
        
        $("#notificationtitle").empty();
        $("#emailnotificationtitle").empty();

        if (data.length == 0) {
            $("#notificationtitle").append('<li class="brown">No Notifications Availiable</li>');
            $("#emailnotificationtitle").empty();
        }
        else {

            $.each(data, function (index, item) {

               
                //if (index == 5) {
                //    return false;
                //}
                // else 
                {

                    if (item.NotificationType == "Bell Notification") {

                      
                        counter++;
                        if (counter <=6)
                        {
                            
                            html = $("<li><a><span class='image'><img src='/Content/Images/School/2.jpg'/></span><span><span class='brown'>Added By--" + item.AddedBy + "</span><span><i class='fa fa-trash pull-right' notify=" + item.UserNotificationID + "></i></span><span class='message'>" + item.Title + "</span></span></a></li>");
                                $("#notificationtitle").append(html);
                            
                           
                                
                           }

                        }
                    }

                    if (item.NotificationType == "Email Notification") {

                        counter1++;
                        if (item.Link != null)
                        {
                            $("#emailnotificationtitle").append("<li><div class='dashboard-notification'><img src='/Content/Images/School/2.jpg' width='30' height='30'/><i class='mdi mdi-comment-processing'></i><a style='text-decoration:none;' href=" + item.Link + "><span>" + '   ' + item.Title + '  --<strong class="brown">By ' + item.AddedBy + "</strong></span></a><i class='fa fa-trash' notify=" + item.UserNotificationID + "></i></div><hr/></li>");//<li class='divider'></li>");
                        }
                        else {
                            $("#emailnotificationtitle").append("<li><div class='dashboard-notification'><img src='/Content/Images/School/2.jpg'width='30' height='30'/><i class='mdi mdi-comment-processing'></i><a style='text-decoration:none;' href='#'><span>" + '   ' + item.Title + '  --<strong class="brown">By ' + item.AddedBy + "</strong></span></a><i class='fa fa-trash' notify=" + item.UserNotificationID + "></i></div><hr/></li>");//<li class='divider'></li>");
                        }

                    }


                
                if (item.NotificationType == "PopUp Notification") {

                    $('#PendingModalAdmin').modal('show');
                    $("div#PendingModalAdmin #title").empty();
                    $("div#PendingModalAdmin #title").html(item.Title);
                    $("div#PendingModalAdmin #notificationDescription").empty();
                    $("div#PendingModalAdmin #notificationDescription").html(item.Description);
                    $("#notificationid").val('');
                    $("#notificationid").val(item.UserNotificationID);

                    $("div#PendingModalAdmin #notificationdisable").off().on('click', function (e) {
                        var notificationid = $("#notificationid").val()
                        $.ajax({
                            url: "/Admin/Notification/DisableNotification",
                            data: { userNotificationID: notificationid },
                            type: "POST",
                            dataType: "json",
                            global:false,
                            success: function (data) {
                                $('#PendingModalAdmin').modal('hide');
                            }
                        });

                    });
                    
                }
                

                $(".fa.fa-trash").off().on('click', function (e) {
                   
                    var notificationid = $(this).attr('notify');                 
                   $(this).closest('li').remove();
                    $.ajax({
                        url: "/Admin/Notification/DisableNotification",
                        data: { userNotificationID: notificationid },
                        type: "POST",
                        dataType: "json",
                        global:false,
                        success: function (data) {
                          
                            counter = counter - 1;

                            if (counter == 0) {
                                $("#envelope").empty();
                            }
                            else {
                                $("#envelope").text(counter);
                            }
                        }
                    });

                });
            });
            if (counter == 0) {
                $("#envelope").hide();
            }
            else {
                $("#envelope").show();
                $("#envelope").text(counter);


            }

            if (counter1 == 0) {
                $("#envelope1").hide();
            }
            else {
                $("#envelope1").show();
                $("#envelope1").text(counter1);


            }
           
            document.title = '(' + (parseInt(counter) + parseInt(counter1)) + ') ' + variable1;
            if (counter >= 6) {

                $("#notificationtitle").append("<li><div class='text-center'><a href='/Admin/Notification/SeeAll'><strong>See All</strong></a></div></li>");
                //$("#seeall").append("<li style='text-align:center'><a href='/Admin/Notification/SeeAll' class='text-center'><strong>See All Notifications </strong></li>");
            }
            else {
                $("#seeall").remove();
            }
        }

       


    };
    $.connection.hub.logging = true;
    $.connection.hub.start().done(function () {
        chat.server.send();

    });

    // Start the connection.
  









// if a back or forward button is clicked
window.addEventListener("popstate", function (e)
{
    
    location.reload();

});
function fancyTimeFormat(time) {
    // Hours, minutes and seconds
    var hrs = ~~(time / 3600);
    var mins = ~~((time % 3600) / 60);
    var secs = time % 60;

    // Output like "1:01" or "4:03:59" or "123:03:59"
    var ret = "";

    if (hrs > 0) {
        ret += "" + hrs + " h " + (mins < 10 ? "0" : "");
    }

    ret += "" + mins + " m " + (secs < 10 ? "0" : "");
    ret += "" + secs + " s ";
    return ret;
}

    
function ResetFormData()
{
    $('input:checkbox').removeAttr('checked');
    $("#ID").val("");
    $("input[type='text'], textarea,input[type='number'], input[type='password'],input[type='checkbox']").each(
      function ()
      {
          $(this).val('');

      }
    );
}

 function resetRowNumber(e)
{       
        $(".k-grid-Edit").find("span").addClass("fa fa-pencil");
        $(".k-grid-Edit").removeClass("k-button");
        $(".k-grid-Delete").find("span").addClass("fa fa-trash");
        $(".k-grid-Delete").removeClass("k-button");
        $(".k-grid-Details").find("span").addClass("fa fa-eye");
        $(".k-grid-Details").removeClass("k-button");
        $(".k-grid-Download").find("span").addClass("fa fa-download");
        $(".k-grid-Download").removeClass("k-button");
      $(".k-grid-Approve").find("span").addClass("fa fa-check");
        $(".k-grid-Approve").removeClass("k-button");

        var grid = e.sender;
        if (grid.dataSource.total() == 0)
        {
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


 function AddAntiForgeryToken(data)
 {
     data.__RequestVerificationToken = $('form input[name=__RequestVerificationToken]').val();
     return data;
 };

 function ShowMessage(message,event)
 { 
     if (event == true)
     {
         toastr.error(message, "Information", { timeOut:2000 });
     }
     if (event == false)
     {
         toastr.error(message, "Information", { timeOut: 2000 });
     }
         
 }

 