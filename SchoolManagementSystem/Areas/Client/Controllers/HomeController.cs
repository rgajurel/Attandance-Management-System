using CaptchaMvc.HtmlHelpers;
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
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IClientLoginRepository clientRepo;
        private readonly IParentsChildRepository parentsChildRepo;

        public HomeController(IClientLoginRepository clientRepo, IParentsChildRepository parentsChildRepo)
        {
            this.clientRepo = clientRepo;
            this.parentsChildRepo = parentsChildRepo;
        }

        // GET: Client/Home
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(ClientLogin Login)
        {
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
                string key = (string)settingsReader.GetValue("SecurityKeyWithValue", typeof(String));
                string email = Login.Email;
                string password = EncryptString(Login.Password, key);
                string parentEmail = clientRepo.loginClient(email, password);
                if (parentEmail != "")
                {
                    Session["parentEmail"] = parentEmail;                    
                    return RedirectToAction("Parents", "Parents", new { area = "Client" });
                }
                else
                {
                    Session["parentEmail"] = "";
                    TempData["Login_Error"] = "Error: Username or Password Doesnot Match.";
                    return View("Index");
                }
            }
            TempData["Login_Error"] = "Error: Captcha is not valid.";
            return View("Index");
        }
        public ActionResult getSideBar()
        {
            ViewBag.StudentsMenu = parentsChildRepo.GetAllStudents(Session["parentEmail"].ToString());
            return PartialView("SideBar");
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
    }
}