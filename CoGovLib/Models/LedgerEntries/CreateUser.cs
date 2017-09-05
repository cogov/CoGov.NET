using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Monty.Linq;

namespace CoGov.Models.LedgerEntryTypes
{
    public class CreateUser : LedgerEntry
    {
        public CreateUser() : base()
        {
            CoreInit();
        }

        public void CoreInit()
        {
            ActionTypeId = LEDGER_ACTION_TYPE.CreateUser;
        }

        public CreateUser Initialize(string SubmitterUserId, string FirstName, string MiddleName, string LastName, string Username, string Password, string Email) 
        {
            CoreInit();

            string Name;

            if (MiddleName == null)
                Name =  String.Format("{0} {1}", FirstName, LastName);
            else
                Name = String.Format("{0} {1}. {2}", FirstName, MiddleName.Substring(0, 1), LastName);

            Description = "Create User " + Name + ": " + Username + " (" + Email + ")";
            this.FirstName = FirstName;
            this.MiddleName = MiddleName;
            this.LastName = LastName;
            this.Username = Username;
            this.Password = Password;
            this.Email = Email;
            this.SubmitterUserId = SubmitterUserId;

            return this;
        }

        public static CreateUser Create(ApplicationDbContext db, string SubmitterUserId, string FirstName, string MiddleName, string LastName, string Username, string Password, string Email)
        {
            var u = db.Users.Local.Where(ux => ux.UserName == Username).FirstOrDefault();

            if (u == null)
                u = db.Users.Where(ux => ux.UserName == Username).FirstOrDefault();

            if (u != null)
            {
                FirstName = u.NameFirst;
                MiddleName = u.NameMiddle;
                LastName = u.NameLast;
                Email = u.Email;
            }

            return db.LedgerEntries.Create<CreateUser>().Initialize(SubmitterUserId, FirstName, MiddleName, LastName, Username, Password, Email);
        }

        public static CreateUser CreateAndAdd(ApplicationDbContext db, Ledger Ledger, string SubmitterUserId, string FirstName, string MiddleName, string LastName, string Username, string Password, string Email)
        {
            return db.LedgerEntries.Add<CreateUser>(Ledger.Entry(Create(db, SubmitterUserId, FirstName, MiddleName, LastName, Username, Password, Email)));
        }

        public static CreateUser CreateAndAddToGroup(ApplicationDbContext db, LedgerEntry GroupAction, string SubmitterUserId, string FirstName, string MiddleName, string LastName, string Username, string Password, string Email)
        {
            if (GroupAction.Ledger == null)
                GroupAction.Ledger = db.Ledgers.Find(new object[] { GroupAction.LedgerId });

            var le = GroupAction.Ledger.Entry(Create(db, SubmitterUserId, FirstName, MiddleName, LastName, Username, Password, Email));
            le.ParentId = GroupAction.Id;

            return db.LedgerEntries.Add<CreateUser>(le);
        }

        [NotMapped]
        public string UserId { get; set; }

        [NotMapped]
        public string FirstName
        {
            get { return (string)GetParm("FirstName"); }
            set { SetParm("FirstName", value); }
        }

        [NotMapped]
        public string MiddleName
        {
            get { return (string)GetParm("MiddleName"); }
            set { SetParm("MiddleName", value); }
        }

        [NotMapped]
        public string LastName
        {
            get { return (string)GetParm("LastName"); }
            set { SetParm("LastName", value); }
        }

        [NotMapped]
        public string Username
        {
            get { return (string)GetParm("Username"); }
            set { SetParm("Username", value); }
        }

        [NotMapped]
        public string Password
        {
            get { return (string)GetParm("Password"); }
            set { SetParm("Password", value); }
        }

        [NotMapped]
        public string Email
        {
            get { return (string)GetParm("Email"); }
            set { SetParm("Email", value); }
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

            var u = db.Users.Local.Where(ux => ux.UserName == Username).FirstOrDefault();

            if (u == null)
                u = db.Users.Where(ux => ux.UserName == Username).FirstOrDefault();

            if (u != null)
            {
                UserId = u.Id;
            }
            else
            {
                UserId = Guid.NewGuid().ToString();

                var user = new ApplicationUser { Id = UserId, UserName = Username, Email = Email, SecurityStamp = Guid.NewGuid().ToString(), NameFirst = FirstName, NameMiddle = MiddleName, NameLast = LastName, TimeStamp = DateTime.UtcNow };

                var userstore = new UserStore<ApplicationUser>(db);
                var usermanager = new UserManager<ApplicationUser>(userstore);

                usermanager.Create(user, Password);
                usermanager.AddToRole(user.Id, "User");
            }

            Ledger.Collective.AddUser(UserId, "Member");

        }

    }

}
