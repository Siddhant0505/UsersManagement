using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersManagement.Utility
{
    public class HandleExceptionAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<HandleExceptionAttribute> _logger;

        public HandleExceptionAttribute(ILogger<HandleExceptionAttribute> logger)
        {
            _logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            //var result = new ViewResult { ViewName = "Error" };
            //var modelMetadata = new EmptyModelMetadataProvider();
            //result.ViewData = new ViewDataDictionary(
            //        modelMetadata, context.ModelState);
            //result.ViewData.Add("HandleException",
            //        context.Exception);
            //context.Result = result;
            
            var routeValuesNew = new RouteValueDictionary {
              { "id", (string)context.RouteData.Values["Controller"] }
            };
            //context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            //{
            //    controller = "Home",
            //    action = "Error"

            //}));
            context.Result = new RedirectResult("~/Home/Error?c=" + (string)context.RouteData.Values["Controller"]);
            context.ExceptionHandled = true;
            //logger.Error(context.Exception.ToString());
            _logger.LogError(context.Exception.ToString() + " - sd");

        }
    }
}

