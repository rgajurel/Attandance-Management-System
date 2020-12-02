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
    public class ChangePassword1Controller : Controller
    {

        private readonly IChangePasswordRepository changePwd;

        public ChangePassword1Controller(IChangePasswordRepository changePwd)
        {
            this.changePwd = changePwd;
        }
        // GET: Client/ChangePassword
        public ActionResult Index()
        {
            return View("_ChangePassword");
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

        //[HttpPost]
        //public JsonResult changePassword(ChangePassword cp)
        //{
        //    System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
        //    string key = (string)settingsReader.GetValue("SecurityKeyWithValue", typeof(String));
        //    string email = Session["parentEmail"].ToString();
        //    string oldPassword = EncryptString(cp.oldPassword, key);
        //    string password1 = EncryptString(cp.newPassword1, key);
        //    string password2 = EncryptString(cp.newPassword2, key);
        //    if (password1 != password2)
        //    {
        //        return Json(new { success = true, responseText = "Warning!!! Two Password Doesnot Match." }, JsonRequestBehavior.AllowGet);
        //        //return Content(" Warning!!! Two Password Doesnot Match.");
        //    }
        //    else
        //    {
        //        bool checkUser = changePwd.checkUser(email,oldPassword);
        //        if (checkUser == true)
        //        {
        //            var passwordChange = changePwd.changePassword(email,oldPassword,password1);
        //            if (passwordChange == true)
        //            {
        //                //return Content(" Success!!! Password Changed Successfully.");
        //                return Json(new { success = true, responseText = "Success!!! Password Changed Successfully." }, JsonRequestBehavior.AllowGet);

        //            }
        //            else
        //            {
        //                return Json(new { success = true, responseText = "Error!!! Please Try again." }, JsonRequestBehavior.AllowGet);
        //                //return Content("Error!!! Please Try again.");
        //            }
        //        }else
        //        {
        //            return Json(new { success = true, responseText = "Warning!!! Old Password Is Incorrect" }, JsonRequestBehavior.AllowGet);
        //            //return Content(" ");
        //        }
        //        //return Content("Hi there!");
        //    }
            
        //}
    }
}