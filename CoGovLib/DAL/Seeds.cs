using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoGov.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Migrations;
using Startbutton.Data.Models;
using System.Data.Entity;

namespace CoGov.DAL
{
    public static class Seeds
    {

        public static void CreateIndexes(ApplicationDbContext db)
        {
            using (DbContextTransaction scope = db.Database.BeginTransaction())
            {
                db.CreateIndex<LedgerEntry>(true, new string[] { "LedgerId", "Number" });
                db.CreateIndex<LedgerEntry>(false, new string[] { "LedgerId", "ParentId" });
                db.CreateIndex<CollectiveShareType>(true, new string[] { "CollectiveId", "Identifier" });
                db.CreateIndex<LedgerEntryParameter>(true, new string[] { "LedgerEntryId", "ActionParameterId" });

                //db.Database.ExecuteSqlCommand(@"CREATE UNIQUE INDEX LedgerEntries_Number ON LedgerEntries(Number)");
                //db.Database.ExecuteSqlCommand(@"CREATE INDEX LedgerEntries_LedgerId_ParentId ON LedgerEntries(LedgerId,ParentId)");
                //db.Database.ExecuteSqlCommand(@"CREATE UNIQUE INDEX CollectiveShareTypes_CollectiveID_Identifier ON CollectiveShareTypes(CollectiveID,Identifier)");
                //db.Database.ExecuteSqlCommand(@"CREATE UNIQUE INDEX LedgerEntryParameters_LedgerEntryId_ActionParameterId ON LedgerEntryParameters(LedgerEntryId,ActionParameterId)");
                scope.Commit();
            }
        }

        public static void AdminUser(ApplicationDbContext db)
        {
            #region Users
            using (DbContextTransaction scope = db.Database.BeginTransaction())
            {
                var rolestore = new RoleStore<IdentityRole>(db);
                var rolemanager = new RoleManager<IdentityRole>(rolestore);
                var userstore = new UserStore<ApplicationUser>(db);
                var usermanager = new UserManager<ApplicationUser>(userstore);

                rolemanager.Create(new IdentityRole { Name = "User" });
                rolemanager.Create(new IdentityRole { Name = "Administrator" });

                var user = new ApplicationUser { Id = "ec827607-3e14-4866-9909-65bd9c1cd93c", UserName = "rayzer", Email = "rayzer42@gmail.com", SecurityStamp = "14eb4831-097a-4e5c-a79a-3ad81c92637e", NameFirst = "Raymond", NameLast = "Powell", TimeStamp = DateTime.UtcNow };
                usermanager.Create(user, "ChangeItAsap!");
                usermanager.AddToRole(user.Id, "Administrator");

                user.Logins.Add(new IdentityUserLogin { LoginProvider = "Facebook", ProviderKey = "10155743376938572" });

                scope.Commit();
            }
            #endregion
        }

        //Some old AddOrUpdates calls are still here even though a seend method will only execute once
        public static void General(ApplicationDbContext db)
        {

            #region LedgerActionTypeSettings
            db.LedgerActionTypeSettings.Add(new LedgerActionTypeSettings()
            {
                Id = new Guid("{C4FF2A86-EF1F-478E-8CC1-C82D04106B8F}"),
                YeaThreshold = 100,
                VetoThreshold = 50,
                MinimumHoursToClose = 168,
                ExecuteImmediatelyUponThreshold = false,
                TimeStamp = DateTime.UtcNow
            });

            db.LedgerActionTypeSettings.Add(new LedgerActionTypeSettings()
            {
                Id = new Guid("{03929108-CEA7-48D6-9D59-F3954C5A3263}"),
                ActionTypeId = LEDGER_ACTION_TYPE.SetMinimumHoursToCloseOfParticularAction,
                MinimumHoursToClose = 1,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region ActionParameterTypes
            db.ActionParameterTypes.Add(new ActionParameterType()
            {
                Id = PARAMETER_TYPE.Bool,
                Name = "Bool",
                UnderlyingTypeEnum = PARAMETER_TYPE_UNDERLYING.Bool,
                TimeStamp = DateTime.UtcNow
            });

            db.ActionParameterTypes.Add(new ActionParameterType()
            {
                Id = PARAMETER_TYPE.Decimal,
                Name = "Decimal",
                UnderlyingTypeEnum = PARAMETER_TYPE_UNDERLYING.Decimal,
                TimeStamp = DateTime.UtcNow
            });

            db.ActionParameterTypes.Add(new ActionParameterType()
            {
                Id = PARAMETER_TYPE.Guid,
                Name = "Guid",
                UnderlyingTypeEnum = PARAMETER_TYPE_UNDERLYING.Guid,
                TimeStamp = DateTime.UtcNow
            });

            db.ActionParameterTypes.Add(new ActionParameterType()
            {
                Id = PARAMETER_TYPE.Integer,
                Name = "Integer",
                UnderlyingTypeEnum = PARAMETER_TYPE_UNDERLYING.Integer,
                TimeStamp = DateTime.UtcNow
            });

            db.ActionParameterTypes.Add(new ActionParameterType()
            {
                Id = PARAMETER_TYPE.MuitilineString,
                Name = "MuitilineString",
                UnderlyingTypeEnum = PARAMETER_TYPE_UNDERLYING.String,
                TimeStamp = DateTime.UtcNow
            });

            db.ActionParameterTypes.Add(new ActionParameterType()
            {
                Id = PARAMETER_TYPE.HTML,
                Name = "HTML",
                UnderlyingTypeEnum = PARAMETER_TYPE_UNDERLYING.String,
                TimeStamp = DateTime.UtcNow
            });

            db.ActionParameterTypes.Add(new ActionParameterType()
            {
                Id = PARAMETER_TYPE.String,
                Name = "String",
                UnderlyingTypeEnum = PARAMETER_TYPE_UNDERLYING.String,
                TimeStamp = DateTime.UtcNow
            });

            db.ActionParameterTypes.Add(new ActionParameterType()
            {
                Id = PARAMETER_TYPE.User,
                Name = "User",
                LookupProperty1 = "Id",
                LookupEntity1 = "ApplicationUser",
                DisplayProperty1 = "NameFull",
                UnderlyingTypeEnum = PARAMETER_TYPE_UNDERLYING.String,
                TimeStamp = DateTime.UtcNow
            });

            db.ActionParameterTypes.Add(new ActionParameterType()
            {
                Id = PARAMETER_TYPE.ShareType,
                Name = "ShareType",
                LookupProperty1 = "Id",
                LookupEntity1 = "Collective",
                DisplayProperty1 = "Name",
                LookupProperty2 = "Id",
                LookupEntity2 = "CollectiveShare",
                DisplayProperty2 = "Name",
                UnderlyingTypeEnum = PARAMETER_TYPE_UNDERLYING.Guid,
                TimeStamp = DateTime.UtcNow
            });

            db.ActionParameterTypes.Add(new ActionParameterType()
            {
                Id = PARAMETER_TYPE.ActionOrLabel,
                Name = "ActionsAndLabels",
                LookupEntity1 = "ActionsAndLabels",
                LookupProperty1 = "Id",
                DisplayProperty1 = "Name",
                UnderlyingTypeEnum = PARAMETER_TYPE_UNDERLYING.Guid,
                TimeStamp = DateTime.UtcNow
            });

            db.ActionParameterTypes.Add(new ActionParameterType()
            {
                Id = PARAMETER_TYPE.Ledger,
                Name = "Ledger",
                LookupEntity1 = "Ledger",
                LookupProperty1 = "Id",
                DisplayProperty1 = "Name",
                UnderlyingTypeEnum = PARAMETER_TYPE_UNDERLYING.Guid,
                TimeStamp = DateTime.UtcNow
            });

            db.ActionParameterTypes.Add(new ActionParameterType()
            {
                Id = PARAMETER_TYPE.UserGroup,
                Name = "UserGroup",
                LookupEntity1 = "UserGroup",
                LookupProperty1 = "Id",
                DisplayProperty1 = "Name",
                UnderlyingTypeEnum = PARAMETER_TYPE_UNDERLYING.Guid,
                TimeStamp = DateTime.UtcNow
            });

            db.ActionParameterTypes.Add(new ActionParameterType()
            {
                Id = PARAMETER_TYPE.UserOrGroupOrCollective,
                Name = "UserOrGroupOrCollective",
                LookupEntity1 = "UserOrGroupOrCollective",
                LookupProperty1 = "Id",
                DisplayProperty1 = "Name",
                UnderlyingTypeEnum = PARAMETER_TYPE_UNDERLYING.Guid,
                TimeStamp = DateTime.UtcNow
            });

            db.ActionParameterTypes.Add(new ActionParameterType()
            {
                Id = PARAMETER_TYPE.String1or2or3Caps,
                Name = "String1or2or3Caps",
                UnderlyingTypeEnum = PARAMETER_TYPE_UNDERLYING.String,
                TimeStamp = DateTime.UtcNow
            });

            #endregion

            #region LedgerActionTypes / Parameters

            #region AddUserToGroup
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.AddUserToGroup,
                Name = "AddUserToGroup",
                Description = "Add a User to a User Group",
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: User
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.AddUserToGroup,
                Sequence = 1000,
                Name = "User",
                DisplayCaption = "User",
                DisplayHint = "The User to Add",
                ParameterTypeId = PARAMETER_TYPE.User,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter2: Group
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.AddUserToGroup,
                Sequence = 2000,
                Name = "Group",
                DisplayCaption = "Group",
                DisplayHint = "The Group to add the User to",
                ParameterTypeId = PARAMETER_TYPE.UserGroup,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region Create Collective
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.CreateCollective,
                Name = "CreateCollective",
                Description = "The first action on every collectives ledger",
                TimeStamp = DateTime.UtcNow
            });

            #endregion

            #region CreateShareType
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.CreateShareType,
                Name = "CreateShareType",
                Description = "Create Share Type",
                ExecuteResult1ActionParameterTypeId = PARAMETER_TYPE.ShareType,
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: Name
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.CreateShareType,
                Sequence = 1000,
                Name = "Name",
                DisplayCaption = "Name",
                DisplayHint = "Share Type Name",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = "CollectiveShare",
                RelatedPropertyName = "Name",
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter2: Identifier
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.CreateShareType,
                Sequence = 2000,
                Name = "Identifier",
                DisplayCaption = "Identifier",
                DisplayHint = "1, 2, or 3 capital letters that will uniquely identify this Share Type",
                ParameterTypeId = PARAMETER_TYPE.String1or2or3Caps,
                RelatedEntityName = "CollectiveShare",
                RelatedPropertyName = "Identifier",
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter3: Has Pool
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.CreateShareType,
                Sequence = 3000,
                Name = "HasPool",
                DisplayCaption = "Has Pool",
                DisplayHint = "Does the Collective Create a semi-fixed Number of these Shares?",
                ParameterTypeId = PARAMETER_TYPE.Bool,
                RelatedEntityName = "CollectiveShare",
                RelatedPropertyName = "HasPool",
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter4: IsTransferable
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.CreateShareType,
                Sequence = 4000,
                Name = "IsTransferable",
                DisplayCaption = "Is Transferable",
                DisplayHint = "Are these shares transferrable between holders on an ad-hoc basis?",
                ParameterTypeId = PARAMETER_TYPE.Bool,
                RelatedEntityName = "CollectiveShare",
                RelatedPropertyName = "IsTransferable",
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter5: AssetBacked
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.CreateShareType,
                Sequence = 5000,
                Name = "AssetBacked",
                DisplayCaption = "Asset Backed",
                DisplayHint = "Asset Backed",
                ParameterTypeId = PARAMETER_TYPE.Bool,
                RelatedEntityName = "CollectiveShare",
                RelatedPropertyName = "AssetBacked",
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region AddMember
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.AddMember,
                Name = "AddMember",
                Description = "Add a New Member to the Collective",
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: Member First Name
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.AddMember,
                Sequence = 1000,
                Name = "FirstName",
                DisplayCaption = "First Name",
                DisplayHint = "The New Member's First Name",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = "Users",
                RelatedPropertyName = "NameFirst",
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter2: Member Middle Name
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.AddMember,
                Sequence = 2000,
                Name = "MiddleName",
                DisplayCaption = "Middle Name",
                DisplayHint = "The New Member's Middle Name",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = "Users",
                RelatedPropertyName = "NameMiddle",
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter3: Member Last Name
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.AddMember,
                Sequence = 3000,
                Name = "LastName",
                DisplayCaption = "Last Name",
                DisplayHint = "The New Member's Last Name",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = "Users",
                RelatedPropertyName = "NameLast",
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter4: Username
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.AddMember,
                Sequence = 4000,
                Name = "Username",
                DisplayCaption = "Username",
                DisplayHint = "The New Member's Username for Login Purposes",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = "Users",
                RelatedPropertyName = "Username",
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter5: Password
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.AddMember,
                Sequence = 5000,
                Name = "Password",
                DisplayCaption = "Password",
                DisplayHint = "The New Member's Password",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter6: Email
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.AddMember,
                Sequence = 6000,
                Name = "Email",
                DisplayCaption = "Email",
                DisplayHint = "The New Member's Email Address",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = "Users",
                RelatedPropertyName = "Email",
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region CreateUserGroup
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.CreateUserGroup,
                Name = "CreateUserGroup",
                Description = "Create a User Group",
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: Name
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.CreateUserGroup,
                Sequence = 1000,
                Name = "Name",
                DisplayCaption = "Name",
                DisplayHint = "The name of the User Group",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = "UserGroups",
                RelatedPropertyName = "Name",
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region DecreaseShares
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.DecreaseShares,
                Name = "DecreaseShares",
                Description = "Give a Member less Shares than they currently have",
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: Username
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.DecreaseShares,
                Sequence = 1000,
                Name = "Username",
                DisplayCaption = "Username",
                DisplayHint = "Username of the Member to Decrease",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter2: ShareType
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.DecreaseShares,
                Sequence = 2000,
                Name = "ShareType",
                DisplayCaption = "Share Type",
                DisplayHint = "Type of Share of which the amount is Decreasing",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter3: NewValue
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.DecreaseShares,
                Sequence = 3000,
                Name = "NewValue",
                DisplayCaption = "New Value",
                DisplayHint = "New Amount of Shares the Member should have",
                ParameterTypeId = PARAMETER_TYPE.Integer,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region DecreaseVoteClout
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.DecreaseVoteClout,
                Name = "DecreaseVoteClout",
                Description = "Give a Member less VoteClout than they currently have",
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: Username
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.DecreaseVoteClout,
                Sequence = 1000,
                Name = "Username",
                DisplayCaption = "Username",
                DisplayHint = "Username of the Member to Decrease",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter2: Ledger
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.DecreaseVoteClout,
                Sequence = 2000,
                Name = "Ledger",
                DisplayCaption = "Ledger",
                DisplayHint = "The identifier of the Ledger in which this user will lose VoteClout",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter3: NewValue
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.DecreaseVoteClout,
                Sequence = 3000,
                Name = "NewValue",
                DisplayCaption = "New Value",
                DisplayHint = "New Amount of Vote Clout the Member should have",
                ParameterTypeId = PARAMETER_TYPE.Integer,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region General
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.General,
                Name = "General",
                Description = "General",
                TimeStamp = DateTime.UtcNow
            });

            #endregion

            #region GiveCollectiveAccessToLedger
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.GiveCollectiveAccessToLedger,
                Name = "GiveCollectiveAccessToLedger",
                Description = "Give a collective access to view a Ledger",
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: CollectiveID
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.GiveCollectiveAccessToLedger,
                Sequence = 1000,
                Name = "Collective",
                DisplayCaption = "Collective",
                DisplayHint = "The unique identifier of the Collective to be granted access",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter2: Ledger
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.GiveCollectiveAccessToLedger,
                Sequence = 2000,
                Name = "Ledger",
                DisplayCaption = "Ledger",
                DisplayHint = "The identifier of the Ledger",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region IncreaseShares
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.IncreaseShares,
                Name = "IncreaseShares",
                Description = "Give a Member more Shares than they currently have",
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: User
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.IncreaseShares,
                Sequence = 1000,
                Name = "User",
                DisplayCaption = "User",
                DisplayHint = "Member to for which to Increase Share holding",
                ParameterTypeId = PARAMETER_TYPE.User,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter2: ShareType
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.IncreaseShares,
                Sequence = 2000,
                Name = "ShareType",
                DisplayCaption = "Share Type",
                DisplayHint = "Type of Share of which the amount is Increasing",
                ParameterTypeId = PARAMETER_TYPE.ShareType,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter3: NewNumberOfShares
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.IncreaseShares,
                Sequence = 3000,
                Name = "NewNumberOfShares",
                DisplayCaption = "Number of Shares",
                DisplayHint = "New Amount of Shares the Member should have",
                ParameterTypeId = PARAMETER_TYPE.Integer,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region IncreaseVoteClout
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.IncreaseVoteClout,
                Name = "IncreaseVoteClout",
                Description = "Give a Member greater VoteClout than they currently have",
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: User
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.IncreaseVoteClout,
                Sequence = 1000,
                Name = "User",
                DisplayCaption = "User",
                DisplayHint = "Member to whom to Increase Vote Clout",
                ParameterTypeId = PARAMETER_TYPE.User,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter2: Ledger
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.IncreaseVoteClout,
                Sequence = 2000,
                Name = "Ledger",
                DisplayCaption = "Ledger",
                DisplayHint = "The Ledger in which this Member will gain VoteClout",
                ParameterTypeId = PARAMETER_TYPE.Ledger,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter3: NewVoteClout
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.IncreaseVoteClout,
                Sequence = 3000,
                Name = "NewVoteClout",
                DisplayCaption = "New Vote Clout",
                DisplayHint = "New Amount of Vote Clout the Member will have",
                ParameterTypeId = PARAMETER_TYPE.Integer,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region SetCollectiveName
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.SetCollectiveName,
                Name = "SetCollectiveName",
                Description = "Set Collective Name",
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: NewName
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetCollectiveName,
                Sequence = 1000,
                Name = "NewName",
                DisplayCaption = "New Name",
                DisplayHint = "The new name of the Collective",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = "Collectives",
                RelatedPropertyName = "Name",
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region SetCollectiveDetails
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.SetCollectiveDetails,
                Name = "SetCollectiveDetails",
                Description = "Set Collective Details",
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: NewDetails
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetCollectiveDetails,
                Sequence = 1000,
                Name = "NewDetails",
                DisplayCaption = "New Details",
                DisplayHint = "The new details for the Collective",
                ParameterTypeId = PARAMETER_TYPE.HTML,
                RelatedEntityName = "Collectives",
                RelatedPropertyName = "Details",
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region SetCollectiveDescription
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.SetCollectiveDescription,
                Name = "SetCollectiveDescription",
                Description = "Set Collective Description",
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: NewDescription
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetCollectiveDescription,
                Sequence = 1000,
                Name = "NewDescription",
                DisplayCaption = "New Description",
                DisplayHint = "The new Description for the Collective",
                ParameterTypeId = PARAMETER_TYPE.MuitilineString,
                RelatedEntityName = "Collectives",
                RelatedPropertyName = "Description",
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region SetLedgerAccess
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.SetLedgerAccess,
                Name = "SetLedgerAccess",
                Description = "Give a Member access to view or propose an action to a Ledger",
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: Username
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetLedgerAccess,
                Sequence = 1000,
                Name = "Username",
                DisplayCaption = "Username",
                DisplayHint = "Username of the Member to which to give Access",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter2: Ledger
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetLedgerAccess,
                Sequence = 2000,
                Name = "Ledger",
                DisplayCaption = "Ledger",
                DisplayHint = "The identifier of the Ledger",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region SetMinimumHoursToCloseOfActionType
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.SetMinimumHoursToCloseOfActionType,
                Name = "SetMinimumHoursToCloseOfActionType",
                Description = "Set the Minimum number of Hours that can be Specified by the Initiator of a certain type of Action before Voting for that Action Closes",
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: ActionType
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetMinimumHoursToCloseOfActionType,
                Sequence = 1000,
                Name = "ActionType",
                DisplayCaption = "Action Type",
                DisplayHint = "Identifier of the Type of Action to be Changed",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter2: NewValue
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetMinimumHoursToCloseOfActionType,
                Sequence = 2000,
                Name = "NewValue",
                DisplayCaption = "New Value",
                DisplayHint = "New Minimum Number of Hours",
                ParameterTypeId = PARAMETER_TYPE.Decimal,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region SetMinimumHoursToCloseOfParticularAction
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.SetMinimumHoursToCloseOfParticularAction,
                Name = "SetMinimumHoursToCloseOfParticularAction",
                Description = "Set the Minimum number of Hours that can be Specified by the Initiator of a Particular Open Action before Voting for that Action Closes",
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: ActionType
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetMinimumHoursToCloseOfParticularAction,
                Sequence = 1000,
                Name = "ActionType",
                DisplayCaption = "Action Type",
                DisplayHint = "Identifier of the Type of Action to be Changed",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter2: NewValue
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetMinimumHoursToCloseOfParticularAction,
                Sequence = 2000,
                Name = "NewValue",
                DisplayCaption = "New Value",
                DisplayHint = "New Minimum Number of Hours",
                ParameterTypeId = PARAMETER_TYPE.Decimal,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region SetPermission
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.SetPermission,
                Name = "SetPermission",
                Description = "Change Permission for a member, member group, or another collective to Execute a Particular Action (or group of actions)",
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: AssignTo
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetPermission,
                Sequence = 1000,
                Name = "AssignTo",
                DisplayCaption = "Assign To",
                DisplayHint = "The member, member group, or collective to which you are Assigning Permission",
                ParameterTypeId = PARAMETER_TYPE.UserOrGroupOrCollective,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter2: Action
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetPermission,
                Sequence = 2000,
                Name = "Action",
                DisplayCaption = "Action",
                DisplayHint = "Action or Action Label to Allow",
                ParameterTypeId = PARAMETER_TYPE.ActionOrLabel,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter3: Propose
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetPermission,
                Sequence = 3000,
                Name = "Propose",
                DisplayCaption = "Propose",
                DisplayHint = "Can this member, member group, or collective even propose the Action or Action Group?",
                ParameterTypeId = PARAMETER_TYPE.Bool,
                RelatedEntityName = "Permission",
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter4: ExecuteWithoutVote
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetPermission,
                Sequence = 4000,
                Name = "ExecuteWithoutVote",
                DisplayCaption = "Execute Without Vote",
                DisplayHint = "Can this member, member group, or collective Action or Action Group at will, without a vote at all?",
                ParameterTypeId = PARAMETER_TYPE.Bool,
                RelatedEntityName = "Permission",
                RelatedPropertyName = "ExecuteWithoutVote",
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter5: MinimumHoursToClose
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetPermission,
                Sequence = 5000,
                Name = "MinimumHoursToClose",
                DisplayCaption = "Minimum Hours To Close",
                DisplayHint = "What is the minimum number of hours this member, member group, or collective must set as the voting period for this Action or Action Group?",
                ParameterTypeId = PARAMETER_TYPE.Integer,
                RelatedEntityName = "Permission",
                RelatedPropertyName = "MinimumHoursToClose",
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter6: ExecuteImmediatelyUponThreshold
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetPermission,
                Sequence = 6000,
                Name = "ExecuteImmediatelyUponThreshold",
                DisplayCaption = "Execute Immediately Upon Threshold",
                DisplayHint = "Can this member, member group, or collective specify that this Action or Action Group will execute immediately upon reaching the cote threshold and will get cancelled immediately upon veto threshold?",
                ParameterTypeId = PARAMETER_TYPE.Bool,
                RelatedEntityName = "Permission",
                RelatedPropertyName = "ExecuteImmediatelyUponThreshold",
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter7: AssignToOthers
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetPermission,
                Sequence = 7000,
                Name = "AssignToOthers",
                DisplayCaption = "Assign To Others",
                DisplayHint = "Can this member, member group, or collective assign this permission to others without a vote?",
                ParameterTypeId = PARAMETER_TYPE.Bool,
                RelatedEntityName = "Permission",
                RelatedPropertyName = "AssignToOthers",
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter8: Deny
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetPermission,
                Sequence = 8000,
                Name = "Deny",
                DisplayCaption = "Deny",
                DisplayHint = "Is this setting being used to deny permission to a perticular action that is part of a perviously permitted action group?",
                ParameterTypeId = PARAMETER_TYPE.Bool,
                RelatedEntityName = "Permission",
                RelatedPropertyName = "Deny",
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region SetShareQuantity
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.SetShareQuantity,
                Name = "SetShareQuantity",
                Description = "Set Share Quantity",
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: ShareType
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetShareQuantity,
                Sequence = 1000,
                Name = "ShareType",
                DisplayCaption = "Share Type",
                DisplayHint = "Identifier of Share Type to Change",
                ParameterTypeId = PARAMETER_TYPE.ShareType,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter2: NewTotalShares
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetShareQuantity,
                Sequence = 2000,
                Name = "NewTotalShares",
                DisplayCaption = "New Total Shares",
                DisplayHint = "New Maximum Amount of Shares that the Collective can issue",
                ParameterTypeId = PARAMETER_TYPE.Integer,
                RelatedEntityName = "CollectiveShare",
                RelatedPropertyName = "Quantity",
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region SetVetoThresholdOfActionType
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.SetVetoThresholdOfActionType,
                Name = "SetVetoThresholdOfActionType",
                Description = "Set the Percentage of Vetoes required for a certain type of Action to be blocked regardless of the Number of Yea Votes",
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: ActionType
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetVetoThresholdOfActionType,
                Sequence = 1000,
                Name = "ActionType",
                DisplayCaption = "Action Type",
                DisplayHint = "Identifier of the Type of Action to be Changed",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter2: NewValue
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetVetoThresholdOfActionType,
                Sequence = 2000,
                Name = "NewValue",
                DisplayCaption = "New Value",
                DisplayHint = "New Veto Vote Threshold Percentage",
                ParameterTypeId = PARAMETER_TYPE.Decimal,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region SetVetoThresholdOfParticularAction
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.SetVetoThresholdOfParticularAction,
                Name = "SetVetoThresholdOfParticularAction",
                Description = "Set the Percentage of Vetoes required for a particular Action to be blocked regardless of the Number of Yea Votes",
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: ActionType
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetVetoThresholdOfParticularAction,
                Sequence = 1000,
                Name = "ActionType",
                DisplayCaption = "Action Type",
                DisplayHint = "Identifier of the Action to be Changed",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter2: NewValue
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetVetoThresholdOfParticularAction,
                Sequence = 2000,
                Name = "NewValue",
                DisplayCaption = "New Value",
                DisplayHint = "New Veto Vote Threshold Percentage",
                ParameterTypeId = PARAMETER_TYPE.Decimal,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region SetYeaThresholdOfActionType
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.SetYeaThresholdOfActionType,
                Name = "SetYeaThresholdOfActionType",
                Description = "Set the Percentage of Yea Votes required for a certain type of Action to be Approved for Execution",
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: ActionType
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetYeaThresholdOfActionType,
                Sequence = 1000,
                Name = "ActionType",
                DisplayCaption = "Action Type",
                DisplayHint = "Identifier of the Type of Action to be Changed",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter2: NewValue
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetYeaThresholdOfActionType,
                Sequence = 2000,
                Name = "NewValue",
                DisplayCaption = "New Value",
                DisplayHint = "New Yea Vote Threshold Percentage",
                ParameterTypeId = PARAMETER_TYPE.Decimal,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region SetYeaThresholdOfParticularAction
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.SetYeaThresholdOfParticularAction,
                Name = "SetYeaThresholdOfParticularAction",
                Description = "Set the Percentage of Yea Votes required for a particular Action to be Approved for Execution",
                TimeStamp = DateTime.UtcNow
            });

            #region Parameter1: ActionType
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetYeaThresholdOfParticularAction,
                Sequence = 1000,
                Name = "ActionType",
                DisplayCaption = "Action Type",
                DisplayHint = "Identifier of the Action to be Changed",
                ParameterTypeId = PARAMETER_TYPE.String,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region Parameter2: NewValue
            db.LedgerActionTypeParameters.Add(new LedgerActionTypeParameter()
            {
                Id = Guid.NewGuid(),
                ActionTypeId = LEDGER_ACTION_TYPE.SetYeaThresholdOfParticularAction,
                Sequence = 2000,
                Name = "NewValue",
                DisplayCaption = "New Value",
                DisplayHint = "New Yea Vote Threshold Percentage",
                ParameterTypeId = PARAMETER_TYPE.Decimal,
                RelatedEntityName = null,
                RelatedPropertyName = null,
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #endregion

            #region SubmitAction
            db.LedgerActionTypes.Add(new LedgerActionType()
            {
                Id = LEDGER_ACTION_TYPE.SubmitAction,
                Name = "SubmitAction",
                Description = "Submit action to another ledger",
                TimeStamp = DateTime.UtcNow
            });

            #endregion            
            #endregion

            #region LedgerActionTypeLabels
            db.LedgerActionTypeLabels.Add(new LedgerActionTypeLabel()
            {
                Id = LEDGER_ACTION_TAG.All,
                Name = "All",
                Description = "All Action Types",
                Types = db.LedgerActionTypes.Local.ToList(),
                TimeStamp = DateTime.UtcNow
            });

            db.LedgerActionTypeLabels.Add(new LedgerActionTypeLabel()
            {
                Id = LEDGER_ACTION_TAG.NonSystemLedger,
                Name = "NonSystemLedger",
                Description = "Actions that can be Proposed in a Non-System Ledger",
                Types = new List<LedgerActionType>() {
                    db.LedgerActionTypes.Find(new object[] { LEDGER_ACTION_TYPE.SubmitAction }),
                    db.LedgerActionTypes.Find(new object[] { LEDGER_ACTION_TYPE.General }),
                },
                TimeStamp = DateTime.UtcNow
            });
            #endregion

            #region PrivilegeRoles
            db.PrivilegeRoles.AddOrUpdate(i => i.Id
                , new PrivilegeRole { Id = new Guid("EA4D1E52-31FF-4F17-BFC2-7088CC6CAE5C"), Name = "Member", ReferenceTable = "Collectives", TimeStamp = DateTime.UtcNow }
            );
            #endregion

            #region LedgerActionStatuses
            db.LedgerActionStatuses.Add(new LedgerActionStatus() { Id = ACTION_STATUS.Open, Name = "Open", TimeStamp = DateTime.UtcNow });
            db.LedgerActionStatuses.Add(new LedgerActionStatus() { Id = ACTION_STATUS.Executed, Name = "Executed", TimeStamp = DateTime.UtcNow });
            db.LedgerActionStatuses.Add(new LedgerActionStatus() { Id = ACTION_STATUS.Failed, Name = "Failed", TimeStamp = DateTime.UtcNow });
            #endregion

            Collective.CreateAndAdd(db, "CoGov.Tech", "ec827607-3e14-4866-9909-65bd9c1cd93c", "Equity", "EQT", 100000, true, true, 1000, 1);

        }

    }

}
