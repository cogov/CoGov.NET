using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CoGov.Models
{

    public class Permission
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public Guid GrantedToId { get; set; }
        public Guid PermissionId { get; set; }
        public bool? Propose { get; set; }
        public bool? ExecuteWithoutVote { get; set; }
        public int? MinimumHoursToClose { get; set; }
        public bool? ExecuteImmediatelyUponThreshold { get; set; }
        public bool? AssignToOthers { get; set; }
        public bool? Deny { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }

        public virtual ApplicationUser User { get; set; }

    }

}