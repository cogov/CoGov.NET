using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CoGov.Models
{

    public class CollectiveShareType
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public Guid CollectiveId { get; set; }

        [MaxLength(3)]
        public string Identifier { get; set; }

        public string Name { get; set; }

        public int? Quantity { get; set; }

        public bool IsTransferable { get; set; }

        public bool AssetBacked { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }

        public virtual Collective Collective { get; set; }

    }

}