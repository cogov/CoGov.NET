using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CoGov.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity.Infrastructure;
using Startbutton.Data.DAL;

namespace CoGov.Controllers
{
    public class ApplicationController : Startbutton.Web.Mvc.ControllerBase<ApplicationDbContext, ApplicationUser>
    {

        ApplicationDbContext _db = null;
        new public ApplicationDbContext db
        {
            get
            {
                if (_db == null)
                {
                    _db = new ApplicationDbContext((DbCache)HttpRuntime.Cache["DbCache"]);
                    if (CurrentUser != null)
                        _db.UserId = CurrentUser.Id;
                }

                return _db;
            }
        }

        private Guid _currentCollectiveId;

        protected Guid CurrentCollectiveId 
        {
            get
            {
                if (_currentCollectiveId == null)
                    _currentCollectiveId = (Guid)Session["CurrentCollectiveId"];

                return _currentCollectiveId;
            }
        }

        private Collective _currentCollective;

        protected Collective CurrentCollective
        {
            get
            {
                if (_currentCollective == null)
                    _currentCollective = (Collective)Session["CurrentCollective"];

                return _currentCollective;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.CurrentUser = CurrentUser;
                ViewBag.CurrentCollective = CurrentCollective;
            }

            base.OnActionExecuting(filterContext);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }

}