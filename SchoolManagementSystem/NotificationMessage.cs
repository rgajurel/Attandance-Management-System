using System;
using System.Linq;

using Infrastructure;
using DomainEntities;

using Quartz;
using Quartz.Impl;
using SchoolManagementSystem;
using Microsoft.AspNet.SignalR;

namespace SchoolManagementSystem
{
    public class NotificationMessage : Hub
    {
        NotificationRepository notificationrepository = new NotificationRepository();

        public void client()
        {

            LoginUser oLoginUser = new LoginUser();
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationMessage>();
            var activenotification = notificationrepository.GetAllNotificationByloginUser(oLoginUser.UserName);
            context.Clients.User(oLoginUser.UserName).allmessage(activenotification.OrderByDescending(model => model.ID));
        }

        public void Send()
        {
            JobScheduler.Start();
        }    


       

    }
    public class NotificationJob : IJob
    {
        NotificationMessage noti = new NotificationMessage();
        public void Execute(IJobExecutionContext context)
        {

            noti.client();

        }
    }

   
    public static class JobScheduler
    {
        
        public static void Start()
        {
            try
            {
                ISchedulerFactory schedFact = new StdSchedulerFactory();
                IScheduler sched = schedFact.GetScheduler();
                sched.Start();

                // define the job and tie it to our HelloJob class
                IJobDetail job = JobBuilder.Create<NotificationJob>()
                .WithIdentity("myJob", "group") // name "myJob", group "group1"
                .Build();


                var jobKey = new JobKey("myJob", "group");
                // Trigger the job to run now, and then every 40 seconds
                if (!sched.CheckExists(jobKey))
                {                   
                    ITrigger trigger = TriggerBuilder.Create()
                  .WithIdentity("myTrigger", "group")
                  .StartNow()
                  .WithSimpleSchedule(x => x
                   .WithIntervalInSeconds(3)
                   .RepeatForever())
               .StartNow()
                  .Build();
                    sched.ScheduleJob(job, trigger);
                }




            }
            catch (Exception ex)
            {
               
            }
        }
    }
}