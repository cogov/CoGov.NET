using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity;

namespace CoGov.Models.LedgerEntryTypes
{
    public class SetShareQuantity : LedgerEntry
    {
        public SetShareQuantity() : base()
        {
            CoreInit();
        }

        public void CoreInit()
        {
            ActionTypeId = LEDGER_ACTION_TYPE.SetShareQuantity;
        }

        public SetShareQuantity Initialize(string SubmitterUserId, string ShareType, int NewTotalShares) 
        {
            CoreInit();
            Description = "Set Share Quantity of " + ShareType + " to " + NewTotalShares.ToString();
            this.ShareType = ShareType;
            this.NewTotalShares = NewTotalShares;
            this.SubmitterUserId = SubmitterUserId;

            return this;
        }

        public static SetShareQuantity Create(ApplicationDbContext db, string SubmitterUserId, string ShareType, int NewTotalShares)
        {
            return db.LedgerEntries.Create<SetShareQuantity>().Initialize(SubmitterUserId, ShareType, NewTotalShares);
        }

        public static SetShareQuantity CreateAndAdd(ApplicationDbContext db, Ledger Ledger, string SubmitterUserId, string ShareType, int NewTotalShares)
        {
            return db.LedgerEntries.Add<SetShareQuantity>(Ledger.Entry(Create(db, SubmitterUserId, ShareType, NewTotalShares)));
        }

        public static SetShareQuantity CreateAndAddToGroup(ApplicationDbContext db, LedgerEntry GroupAction, string SubmitterUserId, string ShareType, int NewTotalShares)
        {
            if (GroupAction.Ledger == null)
                GroupAction.Ledger = db.Ledgers.Find(new object[] { GroupAction.LedgerId });

            var le = GroupAction.Ledger.Entry(Create(db, SubmitterUserId, ShareType, NewTotalShares));
            le.ParentId = GroupAction.Id;

            return db.LedgerEntries.Add<SetShareQuantity>(le);
        }



        [NotMapped]
        public string ShareType
        {
            get { return (string)GetParm("ShareType"); }
            set { SetParm("ShareType", value); }
        }

        [NotMapped]
        public int NewTotalShares
        {
            get { return (int)GetParm("NewTotalShares"); }
            set { SetParm("NewTotalShares", value); }
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

            var cs = db.CollectiveShares.Local.Where(csx => csx.CollectiveId == Ledger.CollectiveId && csx.Identifier == ShareType).FirstOrDefault();

            if (cs == null)
                db.CollectiveShares.Where(csx => csx.CollectiveId == Ledger.CollectiveId && csx.Identifier == ShareType).FirstOrDefault();

            cs.Quantity = NewTotalShares;
        }

    }

}
