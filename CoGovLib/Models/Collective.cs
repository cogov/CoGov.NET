using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.Linq;
using CoGov.Models;
using Startbutton.Data.Models;

namespace CoGov.Models
{

    public class Collective : CoGov.DAL.EntityObject
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [ScaffoldColumn(false)]
        public string CreatorUserId { get; set; }

        [Display(Name = "Collective Name")]
        public string Name { get; set; }

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

        [Display(Name="Allow Public Join", Description = "Allow people to join without an invite?")]
        public bool AllowPublicJoin { get; set; }

        [Display(Name = "Public Contact User", Description = "User whose contact details will be made available to the public")]
        public string PublicContactUserId { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }

        public virtual ApplicationUser CreatorUser { get; set; }

        public IList<Ledger> Ledgers { get; set; }

        private Guid? _SystemLedgerId;
        private Ledger _SystemLedger;

        [NotMapped]
        public Nullable<Guid> SystemLedgerId
        {
            get
            {
                LoadSystemLedger();
                return _SystemLedgerId;
            }
        }

        [NotMapped]
        public Ledger SystemLedger
        {
            get
            {
                LoadSystemLedger();
                return _SystemLedger;
            }
        }

        private void LoadSystemLedger()
        {
            if (_SystemLedgerId == null)
            {
                _SystemLedger = db.Ledgers.Where(l => l.CollectiveId == this.Id && l.Name == "System").FirstOrDefault();
                _SystemLedgerId = _SystemLedger.Id;
            }
        }

        public Collective() { }

        public static Collective Create(ApplicationDbContext db)
        {
            return db.Collectives.Create();
        }

        public static Collective CreateAndAdd(ApplicationDbContext context, string Name, string CreatorUserId, string ShareName, string ShareIdentifier, int? TotalShares, bool IsTransferable, bool AssetBacked, int? UserShares, int UserVoteClout, Guid? id = null)
        {
            var collective = context.Collectives.Create();

            if (id == null)
                id = Guid.NewGuid();

            var user = context.Users.Find(new object[] { CreatorUserId });

            collective.Id = (Guid)id;
            collective.TimeStamp = DateTime.UtcNow;
            collective.CreatorUserId = CreatorUserId;

            var cm = new CollectiveMember { Id = Guid.NewGuid(), CollectiveId = collective.Id, UserId = CreatorUserId, TimeStamp = DateTime.UtcNow };
            var l = new Ledger { Id = Guid.NewGuid(), CollectiveId = collective.Id, Name = "System", Description = "Ledger used for all core system actions", TimeStamp = DateTime.UtcNow };

            context.Collectives.Add(collective);
            context.CollectiveMembers.Add(cm);
            context.Ledgers.Add(l);

            var Actions = LedgerEntry.CreateAndAdd(context, l, null, LEDGER_ACTION_TYPE.General, "Initialize " + Name + " Collective")
                .SetStatus(ACTION_STATUS.Open);

            LedgerEntry.CreateAndAddToGroup(context, Actions, null, LEDGER_ACTION_TYPE.CreateCollective, "Create Collective");

            LedgerEntry.CreateAndAddToGroup(context, Actions, null, LEDGER_ACTION_TYPE.SetCollectiveName, "Set Collective Name")
                .SetParm("NewName", Name);

            LedgerEntry.CreateAndAddToGroup(context, Actions, null, LEDGER_ACTION_TYPE.AddMember, "Create User " + Name + ": " + user.UserName + " (" + user.Email + ")")
                .SetParm("Username", user.UserName);

            LedgerEntry.CreateAndAddToGroup(context, Actions, null, LEDGER_ACTION_TYPE.SetPermission, "Set permission for " + user.NameFull)
                .SetParm("AssignTo", Guid.Parse(user.Id))
                .SetParm("Action", LEDGER_ACTION_TAG.All)
                .SetParm("Propose", true)
                .SetParm("ExecuteWithoutVote", true)
                .SetParm("AssignToOthers", true);

            LedgerEntry.CreateAndAddToGroup(context, Actions, null, LEDGER_ACTION_TYPE.IncreaseVoteClout, "Increase Vote Clout for " + user.NameFull)
                .SetParm("User", user.Id)
                .SetParm("Ledger", l.Id)
                .SetParm("NewVoteClout", 1);

            if (ShareName != null)
            {
                var ShareTypeLedgerId = LedgerEntry.CreateAndAddToGroup(context, Actions, CreatorUserId, LEDGER_ACTION_TYPE.CreateShareType, "Create Share Type " + ShareIdentifier + ": " + ShareName)
                    .SetFlags(LEDGER_ACTION_FLAG.Privileged)
                    .SetParm("Name", ShareName)
                    .SetParm("Identifier", ShareIdentifier)
                    .SetParm("IsTransferable", IsTransferable)
                    .SetParm("AssetBacked", AssetBacked)
                    .Id;

                if (TotalShares != null)
                    LedgerEntry.CreateAndAddToGroup(context, Actions, CreatorUserId, LEDGER_ACTION_TYPE.SetShareQuantity, "Set Share Quantity of " + ShareIdentifier + " to " + TotalShares.ToString())
                        .SetFlags(LEDGER_ACTION_FLAG.Privileged)
                        .SetParmFromResults<Guid>("ShareType", ShareTypeLedgerId, 1)
                        .SetParm("NewTotalShares", TotalShares);

                if (UserShares != null)
                    LedgerEntry.CreateAndAddToGroup(context, Actions, CreatorUserId, LEDGER_ACTION_TYPE.IncreaseShares, "Increase " + ShareIdentifier + " Shares for " + user.UserName)
                        .SetFlags(LEDGER_ACTION_FLAG.Privileged)
                        .SetParm("User", user.Id)
                        .SetParmFromResults<Guid>("ShareType", ShareTypeLedgerId, 1)
                        .SetParm("NewNumberOfShares", (int)UserShares);

            }

            Actions.Execute((int)LEDGER_ACTION_FLAG.SystemAutomatic);

            return collective;

        }

        public void AddUser(string UserId, string RoleName)
        {

            var role = db.PrivilegeRoles.Local.Where(u => u.Name == RoleName && u.ReferenceTable == "Collectives").FirstOrDefault();

            if (role == null)
                role = db.PrivilegeRoles.Single(u => u.Name == RoleName && u.ReferenceTable == "Collectives");

            db.UserPrivilegeRoles.Add(new UserPrivilegeRole()
                {
                    Id = Guid.NewGuid(),
                    UserId = UserId.ToString(),
                    RoleId = role.Id,
                    ReferenceId = Id.ToString(),
                    TimeStamp = DateTime.UtcNow
                }
            );

        }

    }

}