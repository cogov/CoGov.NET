using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoGov.Models;

namespace CoGov.Controllers
{
    public class HomeController : ApplicationController
    {
        public ActionResult Index()
        {
            ViewBag.PublicCollectives = CoGov.Library.Collective.GetPublicCollectives(db);

            ViewBag.DocText = Startbutton.GoogleDocs.GetDocBodyStripStyle("1YW0-xppWH_OtrJkhdzMrPUJko4mo9QGk1XnITFDmQp4");

            return View();
        }

        public ActionResult Support()
        {
            return View();
        }

    }

}