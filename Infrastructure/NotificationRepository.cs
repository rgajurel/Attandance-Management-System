using DomainInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using System.Data;
using Dapper;
using DomainEntities.Mobile;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using MoreLinq;

namespace Infrastructure
{
    public class NotificationRepository : INotificationRepository
    {
        List<Notification> listNotification = new List<Notification>();

        public UserNotification GetAllUser(string userGroup)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@UserGroup", userGroup);                   
                    UserNotification notification = SqlMapper.Query<UserNotification>(connection, "[dbo].[GetAllUser]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return notification;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public bool AddUpdateNotification(Notification notification)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", notification.ID);
                    parameters.Add("@Title", notification.Title);
                    parameters.Add("@Link", notification.Link);
                    parameters.Add("@OrganisationID", notification.OrganisationID);
                    parameters.Add("@NotificationType", notification.NotificationType);
                    parameters.Add("@TriggerNow", notification.TriggerNow);
                    parameters.Add("@TriggerDate", notification.TriggerDate);
                    parameters.Add("@ExpiryDate", notification.ExpiryDate);
                    parameters.Add("@Description", notification.Description);
                    parameters.Add("@IsInternal", notification.IsInternal);
                    parameters.Add("@GroupID", notification.GroupID);
                    parameters.Add("@AddedBy", new LoginUser().UserName);
                    parameters.Add("@UpdatedBy", new LoginUser().UserName);
                    var savechanges = connection.Execute("[dbo].[AddUpdateNotification]", parameters, commandType: CommandType.StoredProcedure);
                    if (savechanges > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteNotification(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    parameters.Add("@DeleteSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Execute("[dbo].[DeleteNotification]", parameters, commandType: CommandType.StoredProcedure);
                    var savechanges = parameters.Get<Boolean>("@DeleteSuccess");
                    if (savechanges)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Notification EditNotification(int id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", id);
                    Notification notification = SqlMapper.Query<Notification>(connection, "[dbo].[EditNotification]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return notification;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Notification> GetAllNotification(Notification notificationSearch)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@TitleSearch", notificationSearch.TitleSearch==null?"":notificationSearch.TitleSearch);
                    param.Add("@OrganisationIDSearch", notificationSearch.OrganisationIDSearch);
                    param.Add("@NotificationTypeSearch", notificationSearch.NotificationTypeSearch);
                    param.Add("@IsAdmin", new LoginUser().IsAdmin);
                    param.Add("@IsSuperAdmin", new LoginUser().IsSuperAdmin);
                    param.Add("@ID", new LoginUser().LoggedInuserID);
                    param.Add("@offset", notificationSearch.offset);
                    param.Add("@PageSize", notificationSearch.pageSize);
                    List<Notification> notificaitonList = SqlMapper.Query<Notification>(connection, "[dbo].[GetAllNotification]",param, commandType: CommandType.StoredProcedure).ToList();

                    return notificaitonList;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public List<UserGroup> GetUserGroupBasedOnOrganisation(string id)
        {
            try
            {
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@ID", id);
                    List<UserGroup> userGroupBasedOnOrganisation = SqlMapper.Query<UserGroup>(connection, "[dbo].[GetUserGroupBasedOnOrganisation]", param, commandType: CommandType.StoredProcedure).ToList(); ;
                    return userGroupBasedOnOrganisation;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IQueryable<Notification> GetAllNotificationByloginUser(string userid)
        {

            List<Notification> activenotification = new List<Notification>();
          var date= TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, TimeZoneInfo.Local.Id, "Nepal Standard Time"); ;

            try
            {
                using (IDbConnection connection = Infrastructure.DBManager.DbConnect())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@UserID", userid);
                    parameters.Add("@Date", date.ToShortDateString());

                    var getallactivenotifications = connection.Query<Notification>("GetAllActiveNotification",
                                    parameters,
                                    commandType: CommandType.StoredProcedure).ToList();

                    foreach (var allactivenotification in getallactivenotifications)
                    {
                        if (allactivenotification.TriggerNow == true)
                        {
                            activenotification.Add(allactivenotification);


                        }
                        else
                        {
                            DateTime d1 = Convert.ToDateTime(allactivenotification.TriggerDate);
                            DateTime d2 = date;
                            double totalsecs = (d1 - d2).TotalSeconds;
                            if (totalsecs <= 0)
                            {
                                activenotification.Add(allactivenotification);
                            }

                        }

                    }

                    return activenotification.AsQueryable();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void DisableNotification( string userNotificationID)
        {
            try
            {

                using (IDbConnection connection = Infrastructure.DBManager.DbConnect())
                {
                    DynamicParameters notificationParam = new DynamicParameters();
                    notificationParam.Add("@userNotificationID", userNotificationID);
                    connection.Execute("[dbo].[DisableUserNotificationNotification]", notificationParam, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void PushNotificationToUser(Notification model)
        {
            try
            {
                var pushNotification = new PushNotificationViewModel()
            {
                notification = model,
                DeviceToken = new List<DeviceTokenViewModel>()
            };
            
                using (IDbConnection connection = DBManager.DbConnect())
                {
                    var devicelogList = SqlMapper.Query<DeviceLog>(connection, "[dbo].[GetAllDeviceLog]", commandType: CommandType.StoredProcedure).ToList();
                    var userDeviceKey = devicelogList.Where(x => x.EmployeeId == model.EmployeeID).ToList();
                    pushNotification.DeviceToken = userDeviceKey.Select(x => new DeviceTokenViewModel() {DeviceToken=x.DeviceToken }).ToList();
                    this.FireBaseExecute(pushNotification);

                }
            }
            catch (Exception ex)
            {

                
            }

            
        }

        //for sending to all to="/topics/UAT_ANDROID_BROADCAST_HRMANAGEMENT"
        private void FireBaseExecute(PushNotificationViewModel model)
        {
            dynamic data=null;
            string applicationID = "AAAAfTlyxBE:APA91bFq96K-DydDKnRF76x7Qi-bxNr1XE9_Pubyz_rrDqmYKeRtW4-VCsXb9cqOvDN0HOeyHyFNcpvJJ67P7SfpmDFdC6XTw9la4uGN1GMmDk0zue7Yu2ExkniAEZdH17o8r5enzp53";
            string senderId = "537834734609";
            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            
            tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));          
            tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
            tRequest.ContentType = "application/json";
            foreach(var datas in model.DeviceToken.ToList())
            {
               data = new
                {
                    to = datas.DeviceToken.ToString(),
                    mutable_content = true,
                    content_available = true,
                    data = new
                    {
                        message = model.notification.Description,
                        title = model.notification.Title,
                        type = "mcw"
                    },
                };          
          

            var serializer = new JavaScriptSerializer();
            string postbody = serializer.Serialize(data).ToString();
            Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
            tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            if (dataStreamResponse != null)
                                using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                    //result.Response = sResponseFromServer;
                                }
                        }
                    }
                }
            }
        }
        //private void FireBaseExecute(PushNotificationViewModel model)
        //{

        //    try
        //    {
        //        string json;
        //        string applicationID = "AAAAfTlyxBE:APA91bFq96K-DydDKnRF76x7Qi-bxNr1XE9_Pubyz_rrDqmYKeRtW4-VCsXb9cqOvDN0HOeyHyFNcpvJJ67P7SfpmDFdC6XTw9la4uGN1GMmDk0zue7Yu2ExkniAEZdH17o8r5enzp53";
        //        string senderId = "537834734609";               


        //            foreach (var batchDevice in model.DeviceToken)
        //            {
        //                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
        //                tRequest.Method = "post";
        //                tRequest.ContentType = "application/json";

        //                var data = new
        //                {
        //                    to = batchDevice.DeviceToken.ToString(),
        //                    data = new
        //                    {
        //                        // image = model.Notification.ImagePath ?? string.Empty,
        //                        message = model.notification.Description,
        //                        title = model.notification.Title,
        //                        // type = string.IsNullOrEmpty(model.NotificationServiceType.ToString()) ? NotificationServiceType.Home.ToString() : model.NotificationServiceType.ToString()
        //                    }
        //                };
        //                var serializer = new JavaScriptSerializer();
        //                json = serializer.Serialize(data);



        //                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
        //                tRequest.Headers.Add($"Authorization: key={applicationID}");
        //                tRequest.Headers.Add($"Sender: id={senderId}");

        //                tRequest.ContentLength = byteArray.Length;
        //                try
        //                {
        //                    using (Stream dataStream = tRequest.GetRequestStream())
        //                    {
        //                        dataStream.Write(byteArray, 0, byteArray.Length);
        //                        using (WebResponse tResponse = tRequest.GetResponse())
        //                        {
        //                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
        //                            {
        //                                using (StreamReader tReader = new StreamReader(dataStreamResponse))
        //                                {
        //                                    String sResponseFromServer = tReader.ReadToEnd();
        //                                    string str = sResponseFromServer;
        //                                    try
        //                                    {
        //                                        //FireBaseLog(new FireBaseLogResponseViewModel() { Response = str, Data = json });
        //                                    }
        //                                    catch (Exception ex) { }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {

        //                }
        //            }

        //    }
        //    catch (Exception ex)
        //    {

        //    }


        //}
    }
}
