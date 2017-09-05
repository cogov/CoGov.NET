using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity;

namespace CoGov.Models.LedgerEntryTypes
{
    public class CreateShareType : LedgerEntry
    {
        public CreateShareType() : base()
        {
            CoreInit();
        }

        public void CoreInit()
        {
            ActionTypeId = LEDGER_ACTION_TYPE.CreateShareType;
        }

        public CreateShareType Initialize(string SubmitterUserId, string Name, string Identifier, bool HasPool, bool IsTransferable, bool AssetBacked) 
        {
            CoreInit();
            Description = "Create Share Type " + Identifier + ": " + Name;
            this.Name = Name;
            this.Identifier = Identifier;
            this.HasPool = HasPool;
            this.IsTransferable = IsTransferable;
            this.AssetBacked = AssetBacked;
            this.SubmitterUserId = SubmitterUserId;

            return this;
        }

        public static CreateShareType Create(ApplicationDbContext db, string SubmitterUserId, string Name, string Identifier, bool HasPool, bool IsTransferable, bool AssetBacked)
        {
            return db.LedgerEntries.Create<CreateShareType>().Initialize(SubmitterUserId, Name, Identifier, HasPool, IsTransferable, AssetBacked);
        }

        public static CreateShareType CreateAndAdd(ApplicationDbContext db, Ledger Ledger, string SubmitterUserId, string Name, string Identifier, bool HasPool, bool IsTransferable, bool AssetBacked)
        {
            var le = Ledger.Entry(Create(db, SubmitterUserId, Name, Identifier, HasPool, IsTransferable, AssetBacked));

            le.CheckErrors(Ledger);

            return db.LedgerEntries.Add<CreateShareType>(le);
        }

        public static CreateShareType CreateAndAddToGroup(ApplicationDbContext db, LedgerEntry GroupAction, string SubmitterUserId, string Name, string Identifier, bool HasPool, bool IsTransferable, bool AssetBacked)
        {
            if (GroupAction.Ledger == null)
                GroupAction.Ledger = db.Ledgers.Find(new object[] { GroupAction.LedgerId });

            var le = GroupAction.Ledger.Entry(Create(db, SubmitterUserId, Name, Identifier, HasPool, IsTransferable, AssetBacked));
            le.ParentId = GroupAction.Id;

            le.CheckErrors(GroupAction.Ledger);

            return db.LedgerEntries.Add<CreateShareType>(le);
        }

        public override void CheckErrors(Ledger Ledger)
        {
            Error = null;

            if (Ledger.db.CollectiveShares.Count(cs => cs.CollectiveId == Ledger.CollectiveId && cs.Identifier == Identifier) +
                (Ledger.db.CollectiveShares.Local.Count(cs => cs.CollectiveId == Ledger.CollectiveId && cs.Identifier == Identifier)) > 0)
            {
                Error = "Share Type '" + Identifier + "' already exists for Collective " + Ledger.Collective.Name;
                return;
            }
        }


        [NotMapped]
        public string Name
        {
            get { return (string)GetParm("Name"); }
            set { SetParm("Name", value); }
        }

        [NotMapped]
        public string Identifier
        {
            get { return (string)GetParm("Identifier"); }
            set { SetParm("Identifier", value); }
        }

        [NotMapped]
        public bool HasPool
        {
            get { return (bool)GetParm("HasPool"); }
            set { SetParm("HasPool", value); }
        }

        [NotMapped]
        public bool IsTransferable
        {
            get { return (bool)GetParm("IsTransferable"); }
            set { SetParm("IsTransferable", value); }
        }

        [NotMapped]
        public bool AssetBacked
        {
            get { return (bool)GetParm("AssetBacked"); }
            set { SetParm("AssetBacked", value); }
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

            if (Error == null)
            {
                var NewId = Guid.NewGuid();

                db.CollectiveShares.Add(new CollectiveShareType()
                {
                    Id = NewId,
                    CollectiveId = Ledger.CollectiveId,
                    Name = Name,
                    Identifier = Identifier,
                    Quantity = HasPool ? 0 : (int?)null,
                    IsTransferable = IsTransferable,
                    AssetBacked = AssetBacked,
                    TimeStamp = DateTime.UtcNow
                });

                ExecuteResult1 = NewId.ToString();
            }
        }

    }

}
