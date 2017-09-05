using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CoGov.Models
{

    public class AgreementSignature
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public string RequestedByUserId { get; set; }

        public Guid AgreementRevisionId { get; set; }

        public string UserId { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }

        public Nullable<DateTime> TimeStampSigned { get; set; }

    }

}