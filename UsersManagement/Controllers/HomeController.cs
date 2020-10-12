using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UsersManagement.BAL.Contracts.Users;
using UsersManagement.Models;
using UsersManagement.BAL;
using UsersManagement.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using UsersManagement.Utility;
using UsersManagement.Utility.Security;
using Microsoft.AspNetCore.DataProtection;
using System.Net.Http;

namespace UsersManagement.Controllers
{
    //[HandleException(typeof(ILogger))]
    [ServiceFilter(typeof(HandleExceptionAttribute))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BAL.Contracts.Users.IUsers _objUser;
        private IConfiguration _config = null;
        private IDataProtectionProvider _dataProtProvider = null;
        EncryptionService _objEncryptionService = null;

        public HomeController(ILogger<HomeController> logger, IUsers objUser, IConfiguration config, IDataProtectionProvider dataProtProvider, EncryptionService objEncryptionService)
        {
            _logger = logger;
            _objUser = objUser;
            _config = config;
            _dataProtProvider = dataProtProvider;
            _objEncryptionService = objEncryptionService;
        }

        public async Task<IActionResult> Index()
        {
            //_logger.LogWarning("Info 3 logging");

            //int f = 0;
            //int s = 1 / f;

            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("https://localhost:44393/");
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //    //GET Method  
            //    var response = await client.GetAsync("Accounts");
            //    if (response.IsSuccessStatusCode)
            //    {
            //       var sdsd = response.Content.ReadAsStringAsync();
            //    }
            //    else
            //    {
            //        Console.WriteLine("Internal server Error");
            //    }
            //}

            return View();
        }

        public IActionResult Login()
        {
            UsersManagement.Domain.Home.Login objLogin = new Domain.Home.Login();
            return View(objLogin);
        }

        [HttpPost]
        public IActionResult Login(UsersManagement.Domain.Home.Login objLogin)
        {
            string strSalt = Salt.Create();
            string hshCode = Hashing.Create("Admin@123", strSalt);

            bool IsValid = Hashing.Validate("Admin@123", strSalt, hshCode);

            //EncryptionService objEncryptionService = new EncryptionService(_dataProtProvider);
            string strEncryption = _objEncryptionService.Encrypt("Admin@123");
            string str = _objEncryptionService.Decrypt(strEncryption);
            
            TokenProvider objProvider = new TokenProvider(_config);
            var userToken = objProvider.LoginUser(objLogin.Username, objLogin.Password);
            if (userToken != null)
            {
                //Save token in session object
                HttpContext.Session.SetString("JWToken", userToken);
                return Redirect("~/Employee/Dashboard");
            }
            //UsersManagement.Domain.Users.LoginViewModel objLogin = new Domain.Users.LoginViewModel();
            //var sdd = _objUser.Login("asas", "ewewe");
            return View();
        }

        public IActionResult NoPermission()
        {
            //TempData["ABC"] = "Employee";
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.Layout = controllerName;
            return View();
        }
        //[HttpPost]
        //public IActionResult Login()
        //{
        //    try
        //    {
        //        //using (var context = new Models.UserManagementContext())
        //        //{
        //        //    var std = new Models.Users()
        //        //    {
        //        //        UserName = "Will",
        //        //        Password = "SMith"
        //        //    };
        //        //    context.Users.Add(std);

        //        //    // or
        //        //    // context.Add<Student>(std);

        //        //    context.SaveChanges();

        //        //    var param = new SqlParameter("@UserName", "Bill");
        //        //    string name = "sas";
        //        //    var sd = context.GetUsers.FromSqlRaw($"GetUsers {name}").ToList();
        //        //}

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    return View();
        //}

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Logoff()
        {
            HttpContext.Session.Clear();

            // Removing Cookies
            CookieOptions option = new CookieOptions();
            if (Request.Cookies[".AspNetCore.Session"] != null)
            {
                option.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Append(".AspNetCore.Session", "", option);
            }

            if (Request.Cookies["AuthenticationToken"] != null)
            {
                option.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Append("AuthenticationToken", "", option);
            }

            return Redirect("~/Home/Index");
        }

        public JsonResult EndSession()
        {
            HttpContext.Session.Clear();
            return Json(new { result = "success" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            //_logger.LogError(ex.ToString());
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            controllerName = HttpContext.Request.Query["c"].ToString();
            ViewBag.Layout = controllerName;
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
