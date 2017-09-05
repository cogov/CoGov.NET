using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using CoGov.DAL;
using System.Linq;
using System.Data.Entity;

namespace CoGov.Models
{
    [Table("Ledger")]
    public class Ledger : EntityObject
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public Guid CollectiveId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }

        public virtual IList<LedgerEntry> Entries { get; set; }
        public virtual Collective Collective { get; set; }

        public LedgerEntry Entry(LedgerEntry LE)
        {
            LE.LedgerId = this.Id;
            return LE;
        }

        public T Entry<T>(T LE) where T:LedgerEntry
        {
            LE.LedgerId = this.Id;
            return LE;
        }

        public List<LedgerEntry> GetDisplayView()
        {
            var rtn = new List<LedgerEntry>();

            foreach (var le in db.LedgerEntries.Include("Parameters").Include("SubmitterUser").Where(lex => lex.LedgerId == Id && lex.ParentId == null).OrderByDescending(lex => lex.TimeStamp).ToList())
            {
                rtn.Add(le);

                foreach (var le2 in db.LedgerEntries.Include("Parameters").Include("SubmitterUser").Where(lex => lex.LedgerId == Id && lex.ParentId == le.Id).OrderBy(lex => lex.TimeStamp).ToList())
                    rtn.Add(le2);
            }

            return rtn;

        }

    }
}