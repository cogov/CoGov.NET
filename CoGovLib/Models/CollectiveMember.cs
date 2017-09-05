using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CoGov.Models
{

    public class CollectiveMember
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public Guid CollectiveId { get; set; }

        public string UserId { get; set; }

        public string ProxyUserId { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }

        public virtual Collective Collective { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationUser ProxyUser { get; set; }

    }

}