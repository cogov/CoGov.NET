using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity;

namespace CoGov.Models.LedgerEntryTypes
{
    public class IncreaseShares : LedgerEntry
    {
        public IncreaseShares() : base()
        {
            CoreInit();
        }

        public void CoreInit()
        {
            ActionTypeId = LEDGER_ACTION_TYPE.IncreaseShares;
        }

        public IncreaseShares Initialize(string SubmitterUserId, string UserId, LedgerEntryParameter<Guid> ShareTypeId, int NewNumberOfShares)
        {
            CoreInit();
            Description = "Increase Vote Clout for " + UserId;
            this.UserId = UserId;
            this.ShareTypeId = ShareTypeId;
            this.NewNumberOfShares = NewNumberOfShares;
            this.SubmitterUserId = SubmitterUserId;
            TimeStamp = DateTime.UtcNow;

            return this;
        }

        public static IncreaseShares Create(ApplicationDbContext db, string SubmitterUserId, string UserId, LedgerEntryParameter<Guid> ShareTypeId, int NewNumberOfShares)
        {
            return db.LedgerEntries.Create<IncreaseShares>().Initialize(SubmitterUserId, UserId, ShareTypeId, NewNumberOfShares);
        }

        public static IncreaseShares CreateAndAdd(ApplicationDbContext db, Ledger Ledger, string SubmitterUserId, string UserId, LedgerEntryParameter<Guid> ShareTypeId, int NewNumberOfShares)
        {
            return db.LedgerEntries.Add<IncreaseShares>(Ledger.Entry(Create(db, SubmitterUserId, UserId, ShareTypeId, NewNumberOfShares)));
        }

        public static IncreaseShares CreateAndAddToGroup(ApplicationDbContext db, LedgerEntry GroupAction, string SubmitterUserId, string UserId, LedgerEntryParameter<Guid> ShareTypeId, int NewNumberOfShares)
        {
            if (GroupAction.Ledger == null)
                GroupAction.Ledger = db.Ledgers.Find(new object[] { GroupAction.LedgerId });

            var le = GroupAction.Ledger.Entry(Create(db, SubmitterUserId, UserId, ShareTypeId, NewNumberOfShares));
            le.ParentId = GroupAction.Id;

            return db.LedgerEntries.Add<IncreaseShares>(le);
        }



        [NotMapped]
        public string UserId
        {
            get { return (string)GetParm("UserId"); }
            set { SetParm("UserId", value); }
        }

        LedgerEntryParameter<Guid> _shareTypeId;

        [NotMapped]
        public LedgerEntryParameter<Guid> ShareTypeId
        {
            get
            {
                return _shareTypeId;
            }
            set
            {
                _shareTypeId = value;
                SetParm("ShareTypeId", _shareTypeId.Value.ToString());
            }
        }

        [NotMapped]
        public int NewNumberOfShares
        {
            get { return int.Parse(Parm3); }
            set { Parm3 = value.ToString(); }
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

            ShareTypeId.db = db;
            Parm2 = ShareTypeId.Value.ToString();

            var ms = db.MemberShares.Local.Where(msx => msx.UserId == UserId && msx.TypeId == ShareTypeId.Value).FirstOrDefault();

            if (ms == null)
                ms = db.MemberShares.Where(msx => msx.UserId == UserId && msx.TypeId == ShareTypeId.Value).FirstOrDefault();

            if (ms == null)
            {
                ms = db.MemberShares.Create();
                ms.Id = Guid.NewGuid();
                ms.UserId = UserId;
                ms.TypeId = ShareTypeId.Value;
                ms.TimeStamp = DateTime.UtcNow;

                db.MemberShares.Add(ms);
            }

            ms.Held = NewNumberOfShares;

        }

    }

}
