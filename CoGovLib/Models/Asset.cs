using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CoGov.Models
{

    public class Asset
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public Guid CollectiveId { get; set; }

        [Display(Name = "Asset Name")]
        public string Name { get; set; }

        [Display(Description = "Short Description of Asset")]
        public string Description { get; set; }

        [UIHint("WYSIWYG")]
        [AllowHtml]
        [Display(Description = "Detailed Description of Asset")]
        [DataType(DataType.MultilineText)]
        public string Details { get; set; }

        public virtual Collective Collective { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }

    }

}