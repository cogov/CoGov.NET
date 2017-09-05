using System;
using System.ComponentModel.DataAnnotations;

namespace CoGov.Models
{
    public class Comment
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }
    }
}