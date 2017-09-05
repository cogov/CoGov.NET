using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace CoGov.Web.Models
{
    public class CreateCollective
    {
        [Required]
        [Display(Name = "Collective Name", Prompt = "The name of the collective.")]
        public string Name { get; set; }

        [Required]
        [Display(Description = "Short Description of Collective")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [UIHint("WYSIWYG")]
        [AllowHtml]
        [Display(Description = "Long Description of Collective for Non-Members")]
        [DataType(DataType.MultilineText)]
        public string Details { get; set; }

        [Display(Description = "Prevent public discovery of this Collective?")]
        public bool Private { get; set; }

        [Display(Name = "Allow Public Join", Description = "Allow people to join without an invite?")]
        public bool AllowPublicJoin { get; set; }

        [Display(Name = "Your Vote Clout", Prompt = "The amount of vote clout you will have upon creation of this Collective.")]
        public int UserVoteClout { get; set; }

        [Display(Name = "Create Shares?")]
        public bool CreateShares { get; set; }

        [Display(Name = "Share Name")]
        public string ShareName { get; set; }

        [Display(Name = "Share Identifier")]
        public string ShareIdentifier { get; set; }

        [Display(Name = "Fixed Quantity of Shares?")]
        public bool FixedNumberOfShares { get; set; }

        [Display(Name = "Fixed Quantity")]
        public int? TotalShares { get; set; }

        [Display(Name = "Is Transferable")]
        public bool IsTransferable { get; set; }

        [Display(Name = "Asset Backed")]
        public bool AssetBacked { get; set; }

        [Display(Name = "Qty Issued to You")]
        public int? UserShares { get; set; }


    }
}