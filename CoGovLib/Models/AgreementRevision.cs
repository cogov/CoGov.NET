using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CoGov.Models
{

    public class AgreementRevision
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public Guid AgreementId { get; set; }

        public string Title { get; set; }

        public string Details { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }

        public virtual Agreement Agreement { get; set; }

    }

}