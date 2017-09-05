using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoGov.Models;

namespace CoGov.Web
{
    public class WebViewPage<TModel> : Startbutton.Web.Mvc.WebViewPage<TModel>
    {
        public ApplicationUser CurrentUser
        {
            get
            {
                return (ApplicationUser)RequestInfo.CurrentUser;
            }
        }

        protected Guid CurrentCollectiveId
        {
            get
            {
                return (Guid)Session["CurrentCollectiveId"];
            }
        }

        protected Collective CurrentCollective
        {
            get
            {
                return (Collective)Session["CurrentCollective"];
            }
        }

    }

}