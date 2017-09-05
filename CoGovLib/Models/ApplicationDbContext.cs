using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System;
using Startbutton.Data.DAL;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CoGov.Models
{
    public class ApplicationDbContext : StartbuttonDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base(null, "DefaultConnection", throwIfV1Schema: false)
        {
            InitialzeLedgerEntryParameterCache();
        }

        public ApplicationDbContext(DbCache Cache) : base(Cache, "DefaultConnection", throwIfV1Schema: false)
        {
            InitialzeLedgerEntryParameterCache();
        }

        public static ApplicationDbContext Create(DbCache Cache)
        {
            return new ApplicationDbContext(Cache);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        private void InitialzeLedgerEntryParameterCache()
        {
            if (Cache == null)
                LedgerEntryParameterCache = new Dictionary<Guid, string>();
            else
            {
                if (!Cache.ContainsKey("LedgerEntryParameterCache"))
                    Cache["LedgerEntryParameterCache"] = new DbCacheEntry() { Item = new Dictionary<Guid, string>() };

                LedgerEntryParameterCache = (Dictionary<Guid, string>)Cache["LedgerEntryParameterCache"].Item;
            }
        }

        public string UserId;
        public Dictionary<Guid, string> LedgerEntryParameterCache;

        public IDbSet<ActionParameterType> ActionParameterTypes { get; set; }
        public IDbSet<Collective> Collectives { get; set; }
        public IDbSet<CollectiveMember> CollectiveMembers { get; set; }
        public IDbSet<CollectiveShareType> CollectiveShares { get; set; }
        public IDbSet<Comment> Comments { get; set; }
        public IDbSet<Ledger> Ledgers { get; set; }
        public IDbSet<LedgerActionTypeParameter> LedgerActionTypeParameters { get; set; }
        public IDbSet<LedgerActionStatus> LedgerActionStatuses { get; set; }
        public IDbSet<LedgerActionTypeLabel> LedgerActionTypeLabels { get; set; }
        public IDbSet<LedgerActionType> LedgerActionTypes { get; set; }
        public IDbSet<LedgerActionTypeSettings> LedgerActionTypeSettings { get; set; }
        public IDbSet<LedgerEntry> LedgerEntries { get; set; }
        public IDbSet<LedgerEntryParameter> LedgerEntryParameters { get; set; }
        public IDbSet<LedgerMember> LedgerMembers { get; set; }
        public IDbSet<MemberShare> MemberShares { get; set; }
        public IDbSet<Permission> Permissions { get; set; }
        public IDbSet<UserGroup> UserGroups { get; set; }
        public IDbSet<Vote> Votes { get; set; }

        [NotMapped]
        public List<LedgerActionOrLedgerActionLabel> ActionsAndLabels
        {
            get
            {
                return FromCache("ActionsAndLabels", new TimeSpan(365, 0, 0, 0), _getActionsAndLabels());
            }
        }

        private IQueryable<LedgerActionOrLedgerActionLabel> _getActionsAndLabels()
        {
            return LedgerActionTypes.Select(lat => new LedgerActionOrLedgerActionLabel() { Id = lat.Id, Name = lat.Name });
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Ledger>()
                .HasRequired<Collective>(l => l.Collective)
                .WithMany(c => c.Ledgers);

            modelBuilder.Entity<LedgerActionType>()
                .HasMany<LedgerActionTypeLabel>(t => t.Types)
                .WithMany(tt => tt.Types);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany<UserGroup>(t => t.Groups)
                .WithMany(tt => tt.Users);

        }



        public new int SaveChanges()
        {

            int rtn;

            if (ChangeTracker.Entries<LedgerEntry>().Count(ct => ct.State == EntityState.Added && ct.Entity.Number == 0) > 0)
            {
                var entries = ChangeTracker.Entries<LedgerEntry>().Where(ct => ct.State == EntityState.Added && ct.Entity.Number == 0).OrderBy(ct => ct.Entity.LedgerId).OrderBy(ct => ct.Entity.TimeStamp).ToList();

                Guid? currentLedgerId = null;

                DbContextTransaction scope = Database.BeginTransaction();

                long? Number = null;

                foreach (var le in entries)
                {
                    if (currentLedgerId != le.Entity.LedgerId)
                    {
                        currentLedgerId = le.Entity.LedgerId;

                        if (currentLedgerId != null)
                        {
                            scope.Commit();
                            scope.Dispose();
                            scope = Database.BeginTransaction();
                        }

                        Number = Database.SqlQuery<long?>("SELECT MAX(Number) FROM LedgerEntries WITH (TABLOCKX, HOLDLOCK) WHERE LedgerId='" + le.Entity.LedgerId.ToString() + "'", new object[0]).First<long?>();

                        if (Number == null)
                            Number = 100000;
                    }

                    le.Entity.Number = (int)++Number;

                }

                scope.Commit();
                rtn = base.SaveChangesWithAudit(UserId);
                scope.Dispose();

            }
            else
                rtn = base.SaveChangesWithAudit(UserId);

            return rtn;
        }

        //public T CreateLedgerEntry<T>() where T:LedgerEntry
        //{
        //    return (T)typeof(T).GetMethod("Create").Invoke(null, new object[] { this });
        //}

    }

}
