using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CoGov.Models
{

    public class Agreement
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public int Sequence { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }

        public virtual Agreement Parent { get; set; }

    }

}