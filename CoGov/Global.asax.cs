using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity.Migrations;
using CoGov.Models;
using Startbutton.Data.DAL;

namespace CoGov
{
    public class MvcApplication : Startbutton.Web.HttpApplication
    {
        protected void Application_Start()
        {
            StartbuttonPreInit();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var migratorConfig = new CoGov.Migrations.Configuration();
            var dbMigrator = new DbMigrator(migratorConfig);

            //try
            //{
                dbMigrator.Update();
            //}
            //catch (Exception Ex)
            //{
            //    if (Ex is System.Data.Entity.Migrations.Infrastructure.AutomaticDataLossException)
            //    {
            //        if (Request.Form["AllowDataLoss"] == "yespleasemrcomputer!")
            //        {
            //            migratorConfig.AutomaticMigrationDataLossAllowed = true;
            //            dbMigrator.Update();
            //        }
            //        else
            //        {
            //            Response.Write(@"
            //                <center>
            //                <h1>DATA LOSS WILL OCCUR</h1>
            //                <form method=post>
            //                Password: <input name=AllowDataLoss>
            //                </form>
            //            ");
            //            Response.End();
            //        }
            //    }
            //    else
            //        throw Ex;

            //}

            RunStartbuttonSeeds();

            ApplicationDbContext adb = new ApplicationDbContext((DbCache)HttpRuntime.Cache["DbCache"]);

            Startbutton.Data.DAL.Seeds.Run<ApplicationDbContext, ApplicationUser>("CoGov.DAL.Seeds.CreateIndexes", adb, CoGov.DAL.Seeds.CreateIndexes);
            adb.SaveChanges();

            Startbutton.Data.DAL.Seeds.Run<ApplicationDbContext, ApplicationUser>("CoGov.DAL.Seeds.AdminUser", adb, CoGov.DAL.Seeds.AdminUser);
            adb.SaveChanges();

            Startbutton.Data.DAL.Seeds.Run<ApplicationDbContext, ApplicationUser>("CoGov.DAL.Seeds.General", adb, CoGov.DAL.Seeds.General);
            adb.SaveChanges();

        }

    }

}
