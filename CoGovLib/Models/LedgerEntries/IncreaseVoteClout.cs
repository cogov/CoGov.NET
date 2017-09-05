using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity;

namespace CoGov.Models.LedgerEntryTypes
{
    public class IncreaseVoteClout : LedgerEntry
    {
        public IncreaseVoteClout() : base()
        {
            CoreInit();
        }

        public void CoreInit()
        {
            ActionTypeId = LEDGER_ACTION_TYPE.IncreaseVoteClout;
        }

        public IncreaseVoteClout Initialize(string SubmitterUserId, string UserId, Guid ForLedgerId, int NewVoteClout)
        {
            CoreInit();
            Description = "Increase Vote Clout for " + UserId;
            this.UserId = UserId;
            this.ForLedgerId = ForLedgerId;
            this.NewVoteClout = NewVoteClout;
            this.SubmitterUserId = SubmitterUserId;

            return this;
        }

        public static IncreaseVoteClout Create(ApplicationDbContext db, string SubmitterUserId, string UserId, Guid ForLedgerId, int NewVoteClout)
        {
            return db.LedgerEntries.Create<IncreaseVoteClout>().Initialize(SubmitterUserId, UserId, ForLedgerId, NewVoteClout);
        }

        public static IncreaseVoteClout CreateAndAdd(ApplicationDbContext db, Ledger Ledger, string SubmitterUserId, string UserId, Guid ForLedgerId, int NewVoteClout)
        {
            return db.LedgerEntries.Add<IncreaseVoteClout>(Ledger.Entry(Create(db, SubmitterUserId, UserId, ForLedgerId, NewVoteClout)));
        }

        public static IncreaseVoteClout CreateAndAddToGroup(ApplicationDbContext db, LedgerEntry GroupAction, string SubmitterUserId, string UserId, Guid ForLedgerId, int NewVoteClout)
        {
            if (GroupAction.Ledger == null)
                GroupAction.Ledger = db.Ledgers.Find(new object[] { GroupAction.LedgerId });

            var le = GroupAction.Ledger.Entry(Create(db, SubmitterUserId, UserId, ForLedgerId, NewVoteClout));
            le.ParentId = GroupAction.Id;

            return db.LedgerEntries.Add<IncreaseVoteClout>(le);
        }



        [NotMapped]
        public string UserId
        {
            get { return (string)GetParm("UserId"); }
            set { SetParm("UserId", value); }
        }

        [NotMapped]
        public Guid ForLedgerId
        {
            get { return (Guid)GetParm("ForLedgerId"); }
            set { SetParm("ForLedgerId", value); }
        }

        [NotMapped]
        public int NewVoteClout
        {
            get { return (int)GetParm("NewVoteClout"); }
            set { SetParm("NewVoteClout", value); }
        }

        public new LedgerEntry ExecuteAsPartOfGroup(LEDGER_ACTION_FLAG Flags = LEDGER_ACTION_FLAG.None)
        {
            _execute(Flags);
            return base.ExecuteAsPartOfGroup(Flags);
        }

        public new LedgerEntry Execute(LEDGER_ACTION_FLAG Flags = LEDGER_ACTION_FLAG.None)
        {
            if (ParentId == null)
            {
                _execute(Flags);
                return base.Execute(Flags);
            }

            throw (new Exception("Can't execute an action that is part of a group!"));
        }

        private void _execute(LEDGER_ACTION_FLAG Flags)
        {
            PrepareForExecute();

            var lm = db.LedgerMembers.Local.Where(lmx => lmx.UserId == UserId && lmx.LedgerId == ForLedgerId).FirstOrDefault();

            if (lm == null)
                lm = db.LedgerMembers.Where(lmx => lmx.UserId == UserId && lmx.LedgerId == ForLedgerId).FirstOrDefault();

            if (lm == null)
            {
                lm = db.LedgerMembers.Create();
                lm.Id = Guid.NewGuid();
                lm.UserId = UserId;
                lm.LedgerId = ForLedgerId;
                lm.TimeStamp = DateTime.UtcNow;

                db.LedgerMembers.Add(lm);
            }

            lm.VoteClout = NewVoteClout;

        }

    }

}
