using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity;

namespace CoGov.Models.LedgerEntryTypes
{
    public class SetCollectiveName : LedgerEntry
    {
        public SetCollectiveName() : base()
        {
            Initialize(null);
        }

        public SetCollectiveName Initialize(string Newame) 
        {
            ActionTypeId = LEDGER_ACTION_TYPE.SetCollectiveName;
            Description = "Set Collective Name";
            this.NewName = Newame;

            return this;
        }

        public static SetCollectiveName Create(ApplicationDbContext db, string SubmitterUserId, string NewName)
        {
            var rtn = db.LedgerEntries.Create<SetCollectiveName>().Initialize(NewName);
            rtn.SubmitterUserId = SubmitterUserId;
            return rtn;
        }

        public static SetCollectiveName CreateAndAdd(ApplicationDbContext db, Ledger Ledger, string SubmitterUserId, string NewName)
        {
            return db.LedgerEntries.Add<SetCollectiveName>(Ledger.Entry(Create(db, SubmitterUserId, NewName)));
        }

        public static SetCollectiveName CreateAndAddToGroup(ApplicationDbContext db, LedgerEntry GroupAction, string SubmitterUserId, string NewName)
        {
            if (GroupAction.Ledger == null)
                GroupAction.Ledger = db.Ledgers.Find(new object[] { GroupAction.LedgerId });

            var le = GroupAction.Ledger.Entry(Create(db, SubmitterUserId, NewName));
            le.ParentId = GroupAction.Id;

            return db.LedgerEntries.Add<SetCollectiveName>(le);
        }

        [NotMapped]
        public string NewName
        {
            get { return (string)GetParm("NewName"); }
            set { SetParm("NewName", value); }
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

        private void _execute(LEDGER_ACTION_FLAG Flags = LEDGER_ACTION_FLAG.None)
        {
            PrepareForExecute();
            Ledger.Collective.Name = NewName;

        }

    }

}
