using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CoGov.Models
{

    public class LedgerActionOrLedgerActionLabel
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
    }

}