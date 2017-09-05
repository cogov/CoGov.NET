using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CoGov.Models
{

    public class MemberShare
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public Guid TypeId { get; set; }
        public string UserId { get; set; }

        public int Held { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }

        public virtual CollectiveShareType Type { get; set; }
        public virtual ApplicationUser User { get; set; }

    }

}