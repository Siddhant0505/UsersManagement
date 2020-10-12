using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using UsersManagement.Security;
using UsersManagement.Utility;
using UsersManagement.Utility.Common;

namespace UsersManagement.Controllers
{
    [UnAuthorized]
    [ServiceFilter(typeof(HandleExceptionAttribute))]
    public class EmployeeController : Controller
    {
        private readonly IHostingEnvironment _environment;

        public EmployeeController(IHostingEnvironment hostingEnvironment)
        {
            _environment = hostingEnvironment;
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        [Authorize(Roles.DIRECTOR, Roles.SUPERVISOR, Roles.ANALYST)]
        public IActionResult Profile()
        {
            UsersManagement.Domain.Employee.Employee obj = new Domain.Employee.Employee();
            return View(obj);
        }

        [Authorize(Roles.DIRECTOR)]
        [HttpPost]
        public IActionResult Profile(UsersManagement.Domain.Employee.Employee obj)
        {
            var upload = HttpContext.Request.Form.Files;

            if (HttpContext.Request.Form.Files.Count == 0)
            {
                ModelState.AddModelError("File", "Please Upload Your file");
            }
            else
            {
                foreach (var file in upload)
                {
                    if (file.Length > 0)
                    {

                        byte[] tempFileBytes = null;

                        // getting File Name
                        var fileName = file.FileName.Trim();

                        using (BinaryReader reader = new BinaryReader(file.OpenReadStream()))
                        {
                            // getting filebytes
                            tempFileBytes = reader.ReadBytes((int)file.Length);
                        }

                        // Creating new FileName
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                        var filetype = Path.GetExtension(fileName).Replace('.', ' ').Trim();

                        // getting FileExtension
                        var fileExtension = Path.GetExtension(fileName);

                        var types = FileUploadCheck.FileType.Image;  // Setting Image type
                        var result = FileUploadCheck.IsValidFile(tempFileBytes, types, filetype); // Validate Header

                        if (result)
                        {
                            var newFileName = string.Concat(myUniqueFileName, fileExtension);
                            fileName = Path.Combine(_environment.WebRootPath, "images") + $@"\{newFileName}";
                            using (FileStream fs = System.IO.File.Create(fileName))
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                            }
                        }

                    }
                }
            }
            //string s = null;
            //s.ToString();
            return View();
        }

        [Authorize(Roles.DIRECTOR, Roles.SUPERVISOR, Roles.ANALYST)]
        public IActionResult Complaint()
        {
            UsersManagement.Domain.Employee.Employee obj = new Domain.Employee.Employee();
            return View(obj);
        }

        public IActionResult CheckAjaxCalls()
        {
            ViewBag.UserRole = GetRole();
            return View("CheckAjaxCalls");
        }

        public JsonResult AuthenticateAjaxCalls()
        {
            return Json(new { result = "success" });
        }

        [Authorize(Roles.DIRECTOR, Roles.SUPERVISOR)]
        public JsonResult AuthorizeAjaxCalls()
        {
            return Json(new { result = "success" });
        }

        private string GetRole()
        {
            if (this.HavePermission(Roles.DIRECTOR))
                return " - DIRECTOR";
            if (this.HavePermission(Roles.SUPERVISOR))
                return " - SUPERVISOR";
            if (this.HavePermission(Roles.ANALYST))
                return " - ANALYST";
            return null;
        }
    }
}