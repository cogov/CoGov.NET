using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CoGov.Models
{

    public class LedgerActionTypeLabel
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }

        public virtual List<LedgerActionType> Types { get; set; }

    }

}