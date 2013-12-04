using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Client;

namespace RavenMvcApp.Controllers
{
    public class RavenBaseController : Controller
    {
        public IDocumentSession RavenSession { get; set; }

        public static IDocumentStore RavenStore { get { return MvcApplication.Store; } }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RavenSession = RavenStore.OpenSession();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                return;
            }
            using (RavenSession)
            {
                if (filterContext.Exception != null)
                {
                    return;
                }
                if (RavenSession != null)
                {
                    RavenSession.SaveChanges();
                }
            }
            base.OnActionExecuted(filterContext);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            using (RavenSession)
            {
                if (RavenSession != null)
                {
                    RavenSession.SaveChanges();
                }
            }
        }
    }
}
