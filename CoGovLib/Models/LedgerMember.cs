using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CoGov.Models
{

    public class LedgerMember
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public string UserId { get; set; }
        public Guid LedgerId { get; set; }
        public int? VoteClout { get; set; }
        public bool CanPost { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Ledger Ledger { get; set; }

    }

}