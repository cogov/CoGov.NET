using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CoGov.Models
{

    public class Vote
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public Guid LedgerActionId { get; set; }

        public char? How { get; set; }
        public int Clout { get; set; }
        public string Comment { get; set; }
        public Guid ProxyUserId { get; set; }

        [ScaffoldColumn(false)]
        public DateTime VoteTimeStamp { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }

        public virtual LedgerActionType LedgerAction { get; set; }
        public virtual ApplicationUser ProxyUser { get; set; }

    }

}