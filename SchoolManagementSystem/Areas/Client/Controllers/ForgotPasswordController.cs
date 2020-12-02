using DomainEntities;
using DomainInterface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Areas.Client.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly IForgotPasswordRepository forgotPwd;
        private readonly IMailSendRepository MailRepo;
        private readonly IChangePasswordRepository changePwRepo;
        public ForgotPasswordController(IChangePasswordRepository changePwRepo,IMailSendRepository MailRepo,IForgotPasswordRepository forgotPwd)
        {
            this.MailRepo = MailRepo;
            this.forgotPwd = forgotPwd;
            this.changePwRepo = changePwRepo;
        }
      
        // GET: Client/ForgotPassword
        public ActionResult Index()
        {
            return View("_ForgotPassword");
        }

        [HttpPost]
        public JsonResult ForgotPassword(ForgotPassword fg)
        {
            int status = forgotPwd.ForgotPassword(fg);
            if (status == 1)
            {
                string randomPassword = generateRandom();
                bool passwordChangeStatus = changePwRepo.changePassword(fg.email, "ctsForgotLinkClicked", randomPassword);
                if (passwordChangeStatus == true)
                {
                    System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
                    string key = (string)settingsReader.GetValue("SecurityKeyWithValue", typeof(String));
                    string decryptedPassword = DecryptString(randomPassword, key);
                    string MailBody = "Your Account Password bas been changed. Please Login with your account Information to See your Students Portal.";
                    MailBody += Environment.NewLine + "Email : " + fg.email + "";
                    MailBody += Environment.NewLine + "Password : " + decryptedPassword + "";
                    string MailSubject = "Parent Login Password Has Been Changed.";
                    var mailStatus = MailRepo.SendMail(fg.email, MailSubject, MailBody);
                    if (mailStatus == true)
                    {

                        return Json(new { success = true, responseText = "Success!!! Login Information has been sent on Email." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = true, responseText = "Password changed but failed to sent on email." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = true, responseText = "Warning!!! Failed to change password." }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { success = true, responseText = "Warning !!! Provided Information Dont verify You." }, JsonRequestBehavior.AllowGet);
            }
        }

        private static Random random = new Random();

        public string generateRandom()
        {
            int length = 8;
            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            string key = (string)settingsReader.GetValue("SecurityKeyWithValue", typeof(String));
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string randomPassword = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            string encryptedString = EncryptString(randomPassword, key);
            return encryptedString;
        }

        public string EncryptString(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToEncrypt = UTF8.GetBytes(Message);
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return Convert.ToBase64String(Results);
        }

        public string DecryptString(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToDecrypt = Convert.FromBase64String(Message);
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return UTF8.GetString(Results);
        }
    }
}