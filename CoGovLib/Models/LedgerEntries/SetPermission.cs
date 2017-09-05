using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity;

namespace CoGov.Models.LedgerEntryTypes
{
    public class SetPermission : LedgerEntry
    {
        public SetPermission() : base()
        {
            CoreInit();
        }

        public void CoreInit()
        {
            ActionTypeId = LEDGER_ACTION_TYPE.SetPermission;
        }

        public SetPermission Initialize(ApplicationDbContext db, string SubmitterUserId, Guid GrantedToId, Guid PermissionId, bool Propose, bool ExecuteWithoutVote, int? MinimumHoursToClose, bool ExecuteImmediatelyUponThreshold, bool AssignToOthers, bool Deny) 
        {
            CoreInit();

            string oid = GrantedToId.ToString();

            var u = db.Users.Local.Where(ux => ux.Id == oid).FirstOrDefault();

            if (u == null)
                u = db.Users.Where(ux => ux.Id == oid).FirstOrDefault();

            if (u != null)
                Description = "Set permission for User: " + u.NameFull;
            else
            {
                var l = db.Ledgers.Local.Where(lx => lx.Id == GrantedToId).FirstOrDefault();

                if (l == null)
                    l = db.Ledgers.Where(lx => lx.Id == GrantedToId).FirstOrDefault();

                if (l != null)
                    Description = "Set permission for Ledger: " + l.Name;
                else
                {
                    var c = db.Ledgers.Local.Where(cx => cx.Id == GrantedToId).FirstOrDefault();

                    if (c == null)
                        c = db.Ledgers.Where(cx => cx.Id == GrantedToId).FirstOrDefault();

                    if (c != null)
                        Description = "Set permission for Collective: " + c.Name;
                }

            }

            this.ObjectId = GrantedToId;
            this.PermissionActionTypeId = PermissionId;
            this.Propose = Propose;
            this.ExecuteWithoutVote = ExecuteWithoutVote;
            this.MinimumHoursToClose = MinimumHoursToClose;
            this.ExecuteImmediatelyUponThreshold = ExecuteImmediatelyUponThreshold;
            this.AssignToOthers = AssignToOthers;
            this.Deny = Deny;

            return this;
        }

        public static SetPermission Create(ApplicationDbContext db, string SubmitterUserId, Guid ObjectId, Guid ActionTypeId, bool Propose, bool ExecuteWithoutVote, int? MinimumHoursToClose, bool ExecuteImmediatelyUponThreshold, bool AssignToOthers, bool Deny)
        {
            return db.LedgerEntries.Create<SetPermission>().Initialize(db, SubmitterUserId, ObjectId, ActionTypeId, Propose, ExecuteWithoutVote, MinimumHoursToClose, ExecuteImmediatelyUponThreshold, AssignToOthers, Deny);
        }

        public static SetPermission CreateAndAdd(ApplicationDbContext db, Ledger Ledger, string SubmitterUserId, Guid ObjectId, Guid ActionTypeId, bool Propose, bool ExecuteWithoutVote, int? MinimumHoursToClose, bool ExecuteImmediatelyUponThreshold, bool AssignToOthers, bool Deny)
        {
            var rtn = db.LedgerEntries.Add<SetPermission>(Ledger.Entry(Create(db, SubmitterUserId, ObjectId, ActionTypeId, Propose, ExecuteWithoutVote, MinimumHoursToClose, ExecuteImmediatelyUponThreshold, AssignToOthers, Deny)));
            rtn.SubmitterUserId = SubmitterUserId;
            return rtn;
        }

        public static SetPermission CreateAndAddToGroup(ApplicationDbContext db, LedgerEntry GroupAction, string SubmitterUserId, Guid ObjectId, Guid ActionTypeId, bool Propose, bool ExecuteWithoutVote, int? MinimumHoursToClose, bool ExecuteImmediatelyUponThreshold, bool AssignToOthers, bool Deny)
        {
            if (GroupAction.Ledger == null)
                GroupAction.Ledger = db.Ledgers.Find(new object[] { GroupAction.LedgerId });

            var le = GroupAction.Ledger.Entry(Create(db, SubmitterUserId, ObjectId, ActionTypeId, Propose, ExecuteWithoutVote, MinimumHoursToClose, ExecuteImmediatelyUponThreshold, AssignToOthers, Deny));
            le.ParentId = GroupAction.Id;

            return db.LedgerEntries.Add<SetPermission>(le);
        }

        [NotMapped]
        public Guid ObjectId
        {
            get { return (Guid)GetParm("ObjectId"); }
            set { SetParm("ObjectId", value); }
        }

        [NotMapped]
        public Guid PermissionActionTypeId
        {
            get { return (Guid)GetParm("PermissionActionTypeId"); }
            set { SetParm("PermissionActionTypeId", value); }
        }

        [NotMapped]
        public bool Propose
        {
            get { return (bool)GetParm("Propose"); }
            set { SetParm("Propose", value); }
        }

        [NotMapped]
        public bool ExecuteWithoutVote
        {
            get { return (bool)GetParm("ExecuteWithoutVote"); }
            set { SetParm("ExecuteWithoutVote", value); }
        }

        [NotMapped]
        public int? MinimumHoursToClose
        {
            get { return (int?)GetParm("MinimumHoursToClose"); }
            set { SetParm("MinimumHoursToClose", value); }
        }

        [NotMapped]
        public bool ExecuteImmediatelyUponThreshold
        {
            get { return (bool)GetParm("ExecuteImmediatelyUponThreshold"); }
            set { SetParm("ExecuteImmediatelyUponThreshold", value); }
        }

        [NotMapped]
        public bool AssignToOthers
        {
            get { return (bool)GetParm("AssignToOthers"); }
            set { SetParm("AssignToOthers", value); }
        }

        [NotMapped]
        public bool Deny
        {
            get { return (bool)GetParm("Deny"); }
            set { SetParm("Deny", value); }
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

            var p = db.Permissions.Where(px => px.GrantedToId == ObjectId && px.PermissionId == PermissionActionTypeId).FirstOrDefault();

            if (p == null)
            {
                p = db.Permissions.Create();
                p.Id = Guid.NewGuid();
                p.GrantedToId = ObjectId;
                p.PermissionId = PermissionActionTypeId;
                p.TimeStamp = DateTime.UtcNow;

                db.Permissions.Add(p);
            }

            p.Propose = Propose;
            p.ExecuteWithoutVote = ExecuteWithoutVote;
            p.MinimumHoursToClose = MinimumHoursToClose;
            p.ExecuteImmediatelyUponThreshold = ExecuteImmediatelyUponThreshold;
            p.AssignToOthers = AssignToOthers;
            p.Deny = Deny;

        }

    }

}
