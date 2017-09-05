using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CoGov.Models
{

    public class UserGroup
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }

        public IList<ApplicationUser> Users { get; set; }

    }

}