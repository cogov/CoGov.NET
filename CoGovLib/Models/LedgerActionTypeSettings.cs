using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CoGov.Models
{

    public class LedgerActionTypeSettings
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public Guid? ActionTypeId { get; set; }

        public Guid? LedgerId { get; set; }

        public decimal? YeaThreshold { get; set; }

        public decimal? VetoThreshold { get; set; }

        public int? MinimumHoursToClose { get; set; }

        public bool? ExecuteImmediatelyUponThreshold { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }

        public virtual LedgerActionType ActionType { get; set; }
        public virtual Ledger Ledger { get; set; }
    }

}