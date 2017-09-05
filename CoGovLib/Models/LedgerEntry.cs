using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using CoGov.DAL;
using System.Data.Entity;
using System.Linq;
using Startbutton.Data.ExtentionMethods;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Reflection;
using System.Threading;

namespace CoGov.Models
{

    public class LedgerEntry : EntityObject
    {
        #region Columns

        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [ScaffoldColumn(false)]
        public Guid LedgerId { get; set; }

        public long Number { get; set; }

        public Guid? ParentId { get; set; }

        public Guid ActionTypeId { get; set; }

        [ScaffoldColumn(false)]
        public string SubmitterUserId { get; set; }

        public string Description { get; set; }

        public string Summary { get; set; }

        [UIHint("WYSIWYG")]
        [AllowHtml]
        public string Details { get; set; }

        public string Url { get; set; }

        public decimal? YeaThreshold { get; set; }

        public decimal? VetoThreshold { get; set; }

        public int? HoursToClose { get; set; }

        public Guid? StatusId { get; set; }

        public DateTime? VoteCloses { get; set; }

        public int? VoteCountYea { get; set; }

        public int? VoteCountNay { get; set; }

        public int? VoteCountVeto { get; set; }

        public int? VoteCountAbstain { get; set; }

        public DateTime? ExecutionAttempt { get; set; }

        public string ExecutionError { get; set; }

        public DateTime? ActionExecuted { get; set; }

        [ScaffoldColumn(false)]
        public int Flags { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }

        [ScaffoldColumn(false)]
        public IList<Vote> Votes { get; set; }

        [ScaffoldColumn(false)]
        public IList<Comment> Comments { get; set; }

        public string ExecuteResult1 { get; set; }

        public string ExecuteResult2 { get; set; }

        public string ExecuteResult3 { get; set; }

        public LedgerEntry SetFlags(LEDGER_ACTION_FLAG Flags)
        {
            return SetFlags((int)Flags);
        }

        public LedgerEntry SetFlags(int Flags)
        {
            this.Flags = this.Flags | Flags;
            return this;
        }

        public LedgerEntry SetStatus(Guid Status)
        {
            this.StatusId = Status;
            return this;
        }

        #endregion

        #region Virtual Columns

        public virtual Ledger Ledger { get; set; }

        public virtual LedgerEntry Parent { get; set; }

        public virtual LedgerActionType ActionType { get; set; }

        public virtual ApplicationUser SubmitterUser { get; set; }

        public virtual LedgerActionStatus Status { get; set; }

        public virtual IList<LedgerEntryParameter> Parameters { get; set; }

        #endregion

        public LedgerEntry()
        {
            Id = Guid.NewGuid();

            // Make sure this is a unique Timestamp for proper ordering
            //if (db.Cache["LastLedgerEntryTimeStamp"] != null)
            //    while (((DateTime)db.Cache["LastLedgerEntryTimeStamp"].Item).ToOADate() < DateTime.UtcNow.AddMilliseconds(-1).ToOADate())
                    Thread.Sleep(1);

            TimeStamp = DateTime.UtcNow;
            //db.Cache["LastLedgerEntryTimeStamp"] = new Startbutton.Data.DAL.DbCacheEntry() { Item = TimeStamp };
        }

        #region Other Public Properties
        [NotMapped]
        public List<string> Errors = new List<string>();

        [NotMapped]
        public string StatusText
        {
            get { return Status == null ? "" : Status.Name; }
        }

        [NotMapped]
        public string YeaThresholdStatusString
        {
            get
            {
                if (VoteCountYea == null || YeaThreshold == null)
                    return "";

                return ((decimal)VoteCountYea / (decimal)YeaThreshold).ToString("##0.00") + "% / " + YeaThreshold.ToString() + "%";
            }
        }

        [NotMapped]
        public string DisplayDescription
        {
            get
            {
                return Description == null ? ActionType.Name : Description;
            }
        }

        public bool HasFlag(LEDGER_ACTION_FLAG Flag)
        {
            return (Flags & (int)Flag) > 0;
        }

        #endregion

        #region Working Attributes/Properties/Classes

        private class ParmFromResult
        {
            public MethodInfo ParseMethodInfo;
            public Guid PreviousLedgerEntryId;
            public int ResultNumber;
        }

        [NotMapped]
        protected Dictionary<string, LedgerActionTypeParameter> Parm
        {
            get
            {
                var parms = db.FromCache<LedgerActionTypeParameter>("LedgerActionTypeParameters_" + ActionTypeId.ToString(), new TimeSpan(1, 0, 0), db.LocalOrDatabase<LedgerActionTypeParameter>(latp => latp.ActionTypeId == ActionTypeId));
                return parms.ToDictionary(r => r.Name, r => r);
            }
        }

        Dictionary<string, ParmFromResult> ParmFromResults = new Dictionary<string, ParmFromResult>();

        #endregion

        #region Public Methods

        #region Create Methods
        public static LedgerEntry Create(ApplicationDbContext db, string SubmitterUserId, Guid ActionTypeId, string Description)
        {
            var rtn = db.LedgerEntries.Create();
            rtn.SubmitterUserId = SubmitterUserId;
            rtn.ActionTypeId = ActionTypeId;
            rtn.Description = Description;
            return rtn;
        }

        public static LedgerEntry CreateAndAdd(ApplicationDbContext db, Ledger Ledger, string SubmitterUserId, Guid ActionTypeId, string Description)
        {
            return db.LedgerEntries.Add(Ledger.Entry(Create(db, SubmitterUserId, ActionTypeId, Description)));
        }

        public static LedgerEntry CreateAndAddToGroup(ApplicationDbContext db, LedgerEntry GroupAction, string SubmitterUserId, Guid ActionTypeId, string Description)
        {
            if (GroupAction.Ledger == null)
                GroupAction.Ledger = db.Ledgers.Find(new object[] { GroupAction.LedgerId });

            var le = GroupAction.Ledger.Entry(Create(db, SubmitterUserId, ActionTypeId, Description));
            le.ParentId = GroupAction.Id;

            return db.LedgerEntries.Add(le);
        }

        #endregion

        public List<string> Validate(Ledger Ledger)
        {

            //db.Entry(this).Reference("ActionType").Load();

            if (ActionType == null)
                ActionType = db.LedgerActionTypes.Find(new object[] { ActionTypeId });

            var method = this.GetType().GetMethod("Validate" + ActionType.Name);

            if (method != null)
                method.Invoke(this, new object[] { Flags });

            return Errors;
        }

        #region Get/Set Parms
        public object GetParm(string ParameterName)
        {
            return GetParm(Parm[ParameterName].Id);
        }

        public object GetParm(Guid ActionParameterId)
        {
            var rec = db.LedgerEntryParameters.Local.Where(lep => lep.LedgerEntryId == Id && lep.ActionParameterId == ActionParameterId).FirstOrDefault();

            if (rec == null)
                rec = db.LedgerEntryParameters.Include("ActionParameter").Include("ActionParameter.ParameterType").Where(lep => lep.LedgerEntryId == Id && lep.ActionParameterId == ActionParameterId).FirstOrDefault();

            if (rec == null)
                return null;

            if (rec.ActionParameter == null)
                rec.ActionParameter = db.LedgerActionTypeParameters.Find(new object[] { rec.ActionParameterId });

            if (rec.ActionParameter.ParameterType == null)
                rec.ActionParameter.ParameterType = db.ActionParameterTypes.Find(new object[] { rec.ActionParameter.ParameterTypeId });

            if (rec.ActionParameter.ParameterType.UnderlyingTypeEnum == PARAMETER_TYPE_UNDERLYING.Bool)
                return rec.ValueBool;
            else if (rec.ActionParameter.ParameterType.UnderlyingTypeEnum == PARAMETER_TYPE_UNDERLYING.DateTime)
                return rec.ValueDateTime;
            else if (rec.ActionParameter.ParameterType.UnderlyingTypeEnum == PARAMETER_TYPE_UNDERLYING.Guid)
                return rec.ValueGuid;
            else if (rec.ActionParameter.ParameterType.UnderlyingTypeEnum == PARAMETER_TYPE_UNDERLYING.String)
                return rec.ValueString;
            else if (rec.ActionParameter.ParameterType.UnderlyingTypeEnum == PARAMETER_TYPE_UNDERLYING.Integer)
                return (int)rec.ValueInteger;
            else if (rec.ActionParameter.ParameterType.UnderlyingTypeEnum == PARAMETER_TYPE_UNDERLYING.Decimal)
                return (decimal)rec.ValueDecimal;

            return null;

        }

        public bool GetParmBool(string ParameterName)
        {
            return GetParmBool(Parm[ParameterName].Id);
        }

        public bool GetParmBool(Guid ActionParameterId)
        {
            var rtn = (bool?)GetParm(ActionParameterId);

            if (rtn == null)
                return false;

            return (bool)rtn;
        }

        public LedgerEntry SetParm(string ParameterName, object Value)
        {
            return SetParm(Parm[ParameterName].Id, Value);
        }

        public LedgerEntry SetParmFromResults<T>(string ParmName, Guid PreviousLedgerEntryId, int ResultNumber)
        {
            var pmi = typeof(T).GetMethod("Parse");

            if (pmi == null)
                throw (new Exception("Type has no Parse method!"));

            ParmFromResults[ParmName] = new ParmFromResult() { ParseMethodInfo = pmi, PreviousLedgerEntryId = PreviousLedgerEntryId, ResultNumber = ResultNumber };
            return this;
        }

        public LedgerEntry SetParm(Guid ActionParameterId, object Value)
        {

            var rec = db.LedgerEntryParameters.Local.Where(lep => lep.LedgerEntryId == Id && lep.ActionParameterId == ActionParameterId).FirstOrDefault();

            if (rec == null)
                rec = db.LedgerEntryParameters.Include("ActionParameter").Include("ActionParameter.ParameterType").Where(lep => lep.LedgerEntryId == Id && lep.ActionParameterId == ActionParameterId).FirstOrDefault();

            if (rec == null)
            {
                rec = new LedgerEntryParameter() {
                    Id = Guid.NewGuid(),
                    LedgerEntryId = Id,
                    ActionParameterId = ActionParameterId,
                    TimeStamp = DateTime.UtcNow
                };

                db.LedgerEntryParameters.Add(rec);
            }

            if (rec.ActionParameter == null)
                rec.ActionParameter = db.LedgerActionTypeParameters.Find(new object[] { ActionParameterId });

            if (rec.ActionParameter.ParameterType == null)
                rec.ActionParameter.ParameterType = db.ActionParameterTypes.Find(new object[] { rec.ActionParameter.ParameterTypeId });

            if (rec.ActionParameter.ParameterType.UnderlyingTypeEnum == PARAMETER_TYPE_UNDERLYING.Bool)
                rec.ValueBool = (bool)Value;
            else if (rec.ActionParameter.ParameterType.UnderlyingTypeEnum == PARAMETER_TYPE_UNDERLYING.DateTime)
                rec.ValueDateTime = (DateTime)Value;
            else if (rec.ActionParameter.ParameterType.UnderlyingTypeEnum == PARAMETER_TYPE_UNDERLYING.Guid)
                rec.ValueGuid = (Guid)Value;
            else if (rec.ActionParameter.ParameterType.UnderlyingTypeEnum == PARAMETER_TYPE_UNDERLYING.String)
                rec.ValueString = (string)Value;
            else if (rec.ActionParameter.ParameterType.UnderlyingTypeEnum == PARAMETER_TYPE_UNDERLYING.Integer)
                rec.ValueInteger = (int)Value;
            else if (rec.ActionParameter.ParameterType.UnderlyingTypeEnum == PARAMETER_TYPE_UNDERLYING.Decimal)
                rec.ValueDecimal = (decimal)Value;

            return this;

        }
        #endregion

        public LedgerEntry ExecuteAsPartOfGroup(int Flags = 0)
        {
            return _execute(Flags);
        }

        public LedgerEntry Execute(int Flags = 0)
        {
            if (ParentId == null)
                return _execute(Flags);

            throw(new Exception("Can't execute an action that is part of a group!"));
        }

        #endregion

        #region Private Methods

        protected virtual void _doExecute()
        { }

        private void _resolveReferences()
        {
            foreach (var pbr in ParmFromResults)
            {
                var le = db.LedgerEntries.Find(new object[] { pbr.Value.PreviousLedgerEntryId });

                string rtn = null;

                if (pbr.Value.ResultNumber == 1)
                    rtn = le.ExecuteResult1;
                else if (pbr.Value.ResultNumber == 2)
                    rtn = le.ExecuteResult2;
                else if (pbr.Value.ResultNumber == 3)
                    rtn = le.ExecuteResult3;

                SetParm(pbr.Key, pbr.Value.ParseMethodInfo.Invoke(null, new object[] { rtn }));

            }
        }

        private LedgerEntry _execute(int Flags)
        {

            _resolveReferences();

            if (Ledger == null)
            {
                Ledger = db.Ledgers.Find(new object[] { LedgerId });
                Ledger.Collective = db.Collectives.Find(new object[] { Ledger.CollectiveId });
            }

            ExecutionAttempt = DateTime.UtcNow;

            if (Errors.Count > 0)
                ExecutionError = String.Join("|", Errors);

            if (Validate(Ledger).Count == 0)
            {
                this.GetType().GetMethod("Execute" + ActionType.Name, BindingFlags.NonPublic | BindingFlags.Instance).Invoke(this, new object[] { Flags });

                this.Flags = this.Flags | (int)Flags;
                ActionExecuted = DateTime.UtcNow;
                StatusId = ACTION_STATUS.Executed;
            }

            return this;
        }

        #region CheckError Methods
        #endregion

        #region Execute Methods
        protected void ExecuteCreateCollective(int Flags)
        { }

        protected void ExecuteCreateShareType(int Flags)
        {
            var NewId = Guid.NewGuid();

            db.CollectiveShares.Add(new CollectiveShareType()
            {
                Id = NewId,
                CollectiveId = Ledger.CollectiveId,
                Name = (string)GetParm("Name"),
                Identifier = (string)GetParm("Identifier"),
                IsTransferable = GetParmBool("IsTransferable"),
                AssetBacked = GetParmBool("AssetBacked"),
                TimeStamp = DateTime.UtcNow
            });

            ExecuteResult1 = NewId.ToString();

        }

        protected void ExecuteAddMember(int Flags)
        {
            string Username = (string)GetParm("Username");
            string Password = (string)GetParm("Password");
            string Email = (string)GetParm("Email");
            string FirstName = (string)GetParm("FirstName");
            string MiddleName = (string)GetParm("MiddleName");
            string LastName = (string)GetParm("LastName");

            var u = db.Users.Local.Where(ux => ux.UserName == Username).FirstOrDefault();

            if (u == null)
                u = db.Users.Where(ux => ux.UserName == Username).FirstOrDefault();

            string UserId;

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

        protected void ExecuteGeneral(int Flags)
        {
            List<LedgerEntry> All = new List<LedgerEntry>();
            All.AddRange(db.LedgerEntries.Local.Where(le => le.ParentId == this.Id).ToList());
            All.AddRange(db.LedgerEntries.Where(le => le.ParentId == this.Id).ToList());

            foreach (var le in All.OrderBy(a => a.TimeStamp))
            {
                if (le.ActionExecuted == null)
                    le.ExecuteAsPartOfGroup(Flags);

                if (le.ExecutionError != null)
                    break;
            }

        }

        protected void ExecuteIncreaseVoteClout(int Flags)
        {

            string UserId = (string)GetParm("User");
            Guid ForLedgerId = (Guid)GetParm("Ledger");
            int NewVoteClout = (int)GetParm("NewVoteClout");

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

        protected void ExecuteSetCollectiveName(int Flags)
        {
            Ledger.Collective.Name = (string)GetParm("NewName");
        }

        protected void ExecuteSetPermission(int Flags)
        {
            Guid ObjectId = (Guid)GetParm("AssignTo");
            Guid ActionId = (Guid)GetParm("Action");

            var p = db.Permissions.Where(px => px.GrantedToId == ObjectId && px.PermissionId == ActionId).FirstOrDefault();

            if (p == null)
            {
                p = db.Permissions.Create();
                p.Id = Guid.NewGuid();
                p.GrantedToId = ObjectId;
                p.PermissionId = ActionId;
                p.TimeStamp = DateTime.UtcNow;

                db.Permissions.Add(p);
            }

            p.Propose = (bool?)GetParm("Propose");
            p.ExecuteWithoutVote = (bool?)GetParm("ExecuteWithoutVote");
            p.MinimumHoursToClose = (int?)GetParm("MinimumHoursToClose");
            p.ExecuteImmediatelyUponThreshold = (bool?)GetParm("ExecuteImmediatelyUponThreshold");
            p.AssignToOthers = (bool?)GetParm("AssignToOthers");
            p.Deny = (bool?)GetParm("Deny");

        }

        protected void ExecuteSetShareQuantity(int Flags)
        {
            var cs = db.CollectiveShares.Local.Where(csx => csx.CollectiveId == Ledger.CollectiveId && csx.Id == (Guid)GetParm("ShareType")).FirstOrDefault();

            if (cs == null)
                db.CollectiveShares.Where(csx => csx.CollectiveId == Ledger.CollectiveId && csx.Id == (Guid)GetParm("ShareType")).FirstOrDefault();

            cs.Quantity = (int)GetParm("NewTotalShares");

        }

        protected void ExecuteIncreaseShares(int Flags)
        {
            string UserId = (string)GetParm("User");
            Guid ShareTypeId = (Guid)GetParm("ShareType");

            var ms = db.MemberShares.Local.Where(msx => msx.UserId == UserId && msx.TypeId == ShareTypeId).FirstOrDefault();

            if (ms == null)
                ms = db.MemberShares.Where(msx => msx.UserId == UserId && msx.TypeId == ShareTypeId).FirstOrDefault();

            if (ms == null)
            {
                ms = db.MemberShares.Create();
                ms.Id = Guid.NewGuid();
                ms.UserId = (string)GetParm("User");
                ms.TypeId = (Guid)GetParm("ShareType");
                ms.TimeStamp = DateTime.UtcNow;

                db.MemberShares.Add(ms);
            }

            ms.Held = (int)GetParm("NewNumberOfShares");

        }

        #endregion

        #endregion

    }

    public static class LedgerEntryExt
    { 
        public static T Add<T>(this IDbSet<LedgerEntry> LedgerEntries, T Entry) where T : LedgerEntry
        {
            return (T)LedgerEntries.Add(Entry);
        }

    }

}