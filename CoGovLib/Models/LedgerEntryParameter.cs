using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.Reflection;
using CoGov.DAL;
using System.Linq;

namespace CoGov.Models
{

    public class LedgerEntryParameter : EntityObject
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public Guid LedgerEntryId { get; set; }

        public Guid ActionParameterId { get; set; }

        public Guid? ValueGuid { get; set; }

        public string ValueString { get; set; }

        public DateTime? ValueDateTime { get; set; }

        public bool? ValueBool { get; set; }

        public decimal? ValueDecimal { get; set; }

        public int? ValueInteger { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }

        public virtual LedgerEntry LedgerEntry { get; set; }
        public virtual LedgerActionTypeParameter ActionParameter { get; set; }

        public string ValueToString(string DateTimeFormat = null)
        {
            if (ActionParameter.ParameterType.UnderlyingTypeEnum == PARAMETER_TYPE_UNDERLYING.Bool)
                return ValueBool.ToString();
            else if (ActionParameter.ParameterType.UnderlyingTypeEnum == PARAMETER_TYPE_UNDERLYING.DateTime)
                return ((DateTime)ValueDateTime).ToString(DateTimeFormat);
            else if (ActionParameter.ParameterType.UnderlyingTypeEnum == PARAMETER_TYPE_UNDERLYING.Guid)
                return ValueGuid.ToString();
            else if (ActionParameter.ParameterType.UnderlyingTypeEnum == PARAMETER_TYPE_UNDERLYING.Integer)
                return ValueInteger.ToString();
            else if (ActionParameter.ParameterType.UnderlyingTypeEnum == PARAMETER_TYPE_UNDERLYING.Decimal)
                return ValueDecimal.ToString();

            return ValueString;
        }

        public string DisplayValue(object Format = null)
        {
            if (!db.LedgerEntryParameterCache.ContainsKey(Id))
                db.LedgerEntryParameterCache[Id] = (string)this.GetType().GetMethod("DisplayValue" + ActionParameter.ParameterType.Name, BindingFlags.NonPublic | BindingFlags.Instance).Invoke(this, new object[] { Format });

            return db.LedgerEntryParameterCache[Id];
        }

        protected string DisplayValueBool(object Format)
        {
            if (ValueBool == null)
                return "No";
            else if ((bool)ValueBool)
                return "Yes";

            return "No";
        }

        protected string DisplayValueDecimal(object Format)
        {
            if (ValueDecimal == null)
                return "(null)";

            if (Format != null)
                return ((int)ValueDecimal).ToString((string)Format);

            return ((int)ValueDecimal).ToString();
        }

        protected string DisplayValueGuid(object Format)
        {
            if (ValueGuid == null)
                return "(null)";

            return ValueGuid.ToString();
        }

        protected string DisplayValueInteger(object Format)
        {
            if (ValueInteger == null)
                return "(null)";

            if (Format != null)
                return ((int)ValueInteger).ToString((string)Format);

            return ((int)ValueInteger).ToString();
        }

        protected string DisplayValueString(object Format)
        {
            if (ValueString == null)
                return "(null)";

            return ValueString;
        }

        protected string DisplayValueUser(object Format)
        {
            if (ValueString == null)
                return "(null)";

            var u = db.Users.Find(new object[] { ValueString });

            if (Format == null)
                return u.UserName;
            else if (((string)Format).ToUpper() == "FIRST")
                return u.NameFirst;
            else if (((string)Format).ToUpper() == "LAST")
                return u.NameLast;
            else if (((string)Format).ToUpper() == "FULL")
                return u.NameFull;

            return u.UserName;

        }

        protected string DisplayValueShareType(object Format)
        {
            if (ValueGuid == null)
                return "(null)";

            var rec = db.CollectiveShares.Find(new object[] { ValueGuid });

            if (Format == null)
                return rec.Name;
            else if (((string)Format).ToUpper() == "ID")
                return rec.Identifier;

            return rec.Name;
        }

        protected string DisplayValueActionsAndLabels(object Format)
        {
            if (ValueGuid == null)
                return "(null)";

            var rec = db.LedgerActionTypes.Find(new object[] { ValueGuid });

            if (rec == null)
            {
                var rec2 = db.LedgerActionTypeLabels.Find(new object[] { ValueGuid });
                return "*" + rec2.Name + "*";
            }

            return rec.Name;
        }

        protected string DisplayValueLedger(object Format)
        {
            if (ValueGuid == null)
                return "(null)";

            var rec = db.Ledgers.Find(new object[] { ValueGuid });

            return rec.Name;
        }

        protected string DisplayValueUserGroup(object Format)
        {
            if (ValueGuid == null)
                return "(null)";

            var rec = db.UserGroups.Find(new object[] { ValueGuid });

            return rec.Name;
        }

        protected string DisplayValueUserOrGroupOrCollective(object Format)
        {
            if (ValueGuid == null)
                return "(null)";

            var u = db.Users.Find(new object[] { ValueGuid.ToString() });

            if (u != null)
            {

                if (Format == null)
                    return "Member: " + u.NameFull;
                else if (((string)Format).ToUpper() == "USERNAME")
                    return "Member: " + u.UserName;
                else
                    return "Member: " + u.NameFull;
            }

            var ug = db.UserGroups.Find(new object[] { ValueGuid });

            if (ug != null)
                return "Group: " + ug.Name;

            var c = db.Collectives.Find(new object[] { ValueGuid });

            return "Collective: " + c.Name;
        }

        protected string DisplayValueString1or2or3Caps(object Format)
        {
            if (ValueString == null)
                return "(null)";

            return ValueString;
        }

    }

}