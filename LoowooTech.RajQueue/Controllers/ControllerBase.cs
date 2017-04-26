using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LoowooTech.RajQueue.Common;
using LoowooTech.RajQueue.Managers;

namespace LoowooTech.RajQueue.Controllers
{
    public class ControllerBase : AsyncController
    {
        protected ManagerCore Core { get { return ManagerCore.Instance; } }
        protected ActionResult SuccessJsonResult(object data = null)
        {
            return new ContentResult { Content = new { result = 1, content = "操作成功", data }.ToJson(), ContentEncoding = System.Text.Encoding.UTF8, ContentType = "text/json" };
        }

        protected ActionResult ErrorJsonResult(string message)
        {
            return new ContentResult { Content = new { result = 0, content = message }.ToJson(), ContentEncoding = System.Text.Encoding.UTF8, ContentType = "text/json" };
        }

        protected ActionResult ErrorJsonResult(Exception ex)
        {
            return new ContentResult { Content = new { result = 0, content = ex.Message, data = ex }.ToJson(), ContentEncoding = System.Text.Encoding.UTF8, ContentType = "text/json" };
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Controller = filterContext.RequestContext.RouteData.Values["controller"];
            ViewBag.Action = filterContext.RequestContext.RouteData.Values["action"];
            base.OnActionExecuting(filterContext);
        }
        private int GetStatusCode(Exception ex)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;
            if (ex is HttpException)
            {
                statusCode = (ex as HttpException).GetHttpCode();
            }
            else if (ex is UnauthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Forbidden;
            }
            return statusCode;
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;

            filterContext.ExceptionHandled = true;
            if (filterContext.HttpContext.Response.StatusCode == 200)
            {
                filterContext.HttpContext.Response.StatusCode = GetStatusCode(filterContext.Exception);
            }
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                filterContext.Result = ErrorJsonResult(filterContext.Exception);
            }
            else
            {
                ViewBag.Exception = filterContext.Exception;
                filterContext.Result = View("Error");
            }
        }
    }
}