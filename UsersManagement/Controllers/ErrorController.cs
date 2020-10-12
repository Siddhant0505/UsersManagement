using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace UsersManagement.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int StatusCode)
        {
            switch(StatusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Page you requested could not found";
                    break;
                case 500:
                    ViewBag.ErrorMessage = "Internal Server error. Please try again or contact administrator";
                    break;

            }
            return View();
        }
    }
}