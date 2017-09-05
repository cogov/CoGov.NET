using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CoGov.Models;
using CoGov.Web;
using CoGov.Web.Models;

namespace CoGov.Controllers
{
    public class CollectiveController : ApplicationController
    {
        [AuthorizeCollective(PrivilegeRoles = "Member")]
        public ActionResult Dashboard(Guid id)
        {
            ViewBag.Title = CurrentCollective.Name + " - Dashboard";

            return View();
        }

        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Title = "Create a CoGovernance Collective";

            var model = new CreateCollective();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCollective collective)
        {
            if (ModelState.IsValid)
            {

                var c = Collective.CreateAndAdd(db, collective.Name, CurrentUser.Id, collective.ShareName, collective.ShareIdentifier, collective.TotalShares, collective.IsTransferable, collective.AssetBacked, collective.UserShares, collective.UserVoteClout);

                db.SaveChanges();

                return RedirectToAction("Dashboard", "Collective", new { id = c.Id });
            }

            return View(collective);
        }

    }

}
