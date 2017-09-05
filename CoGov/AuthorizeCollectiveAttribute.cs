using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoGov.Models;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Startbutton.Web.Privilege;
using Startbutton.Data.DAL;

namespace CoGov.Web
{
    public class AuthorizeCollectiveAttribute : AuthorizePrivilegeAttribute
    {

        public AuthorizeCollectiveAttribute() : base()
        {
            base.ReferenceTable = "Collectives";
            base.IdRecordIdentificationMethod = AuthorizePrivilegeIdRecordIdentificationMethod.Session;
            base.IdRecordIdentifier = "CurrentCollectiveId";
            base.MissingRecordIdMethod = MissingRecordIdMethod.Redirect;
            base.MissingRecordIdParameter = "~/Collective/SelectActive";
            base.NotAuthorizedParameter = "~/Home/NonAuthorized";
        }

        public override AuthorizePrivilegeAttribute.TryAuthorizeWithoutRedirectResult TryAuthorizeWithoutRedirect(AuthorizationContext filterContext)
        {
            ApplicationDbContext db = new ApplicationDbContext((DbCache)HttpRuntime.Cache["DbCache"]);
            string uid = ((System.Web.Mvc.Controller)filterContext.Controller).User.Identity.GetUserId();

            var collectiveList = CollectiveList(db, uid);

            if (collectiveList.Count() == 1)
            {
                SetAuthorization(db, collectiveList.First().Id);
                return TryAuthorizeWithoutRedirectResult.AuthorizationComplete;
            }
            else if (collectiveList.Count() == 0)
                return TryAuthorizeWithoutRedirectResult.NotAuthorized;
            else
            {
                string[] p = HttpContext.Current.Request.Path.Split('/');

                Guid id;

                if (Guid.TryParse(p[p.Length - 1], out id))
                {
                    if (collectiveList.Select(c => c.Id).Contains(id))
                    {
                        SetAuthorization(db, collectiveList.Single(cl => cl.Id == id).Id);
                        return TryAuthorizeWithoutRedirectResult.AuthorizationComplete;
                    }
                }

                return TryAuthorizeWithoutRedirectResult.Redirect;
            }

        }

        public static IOrderedQueryable<Collective> CollectiveList(ApplicationDbContext db, string UserId)
        {
            return db.UserPrivilegeRoles.Where(upr => upr.Role.ReferenceTable == "Collectives" && upr.UserId == UserId)
                .Join(db.Collectives, upr => upr.ReferenceId, c => c.Id.ToString(), (upr, c) => new { upr, c })
                .Select(j => j.c)
                .Distinct()
                .OrderBy(c => c.Name);
        }

        public static void SetAuthorization(ApplicationDbContext db, Guid CollectiveId)
        {
            HttpContext.Current.Session["CurrentCollective"] = db.Collectives.Single(si => si.Id == CollectiveId);
            HttpContext.Current.Session["CurrentCollectiveId"] = CollectiveId;
        }

    }

}