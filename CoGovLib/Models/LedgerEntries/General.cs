using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

namespace CoGov.Models.LedgerEntryTypes
{
    public class General : LedgerEntry
    {

        public static LedgerEntryTypes.General Create(ApplicationDbContext db, string SubmitterUserId, string Description)
        {
            return (LedgerEntryTypes.General)Create(db, SubmitterUserId, LEDGER_ACTION_TYPE.General, Description);
        }

        public static LedgerEntryTypes.General CreateAndAdd(ApplicationDbContext db, Ledger Ledger, string SubmitterUserId, string Description)
        {
            return (LedgerEntryTypes.General)CreateAndAdd(db, Ledger, SubmitterUserId, LEDGER_ACTION_TYPE.General, Description);
        }

        public static LedgerEntryTypes.General CreateAndAddToGroup(ApplicationDbContext db, LedgerEntry GroupAction, string SubmitterUserId, string Description)
        {
            return (LedgerEntryTypes.General)CreateAndAddToGroup(db, GroupAction, SubmitterUserId, LEDGER_ACTION_TYPE.General, Description);
        }

        public void _doExecute(LEDGER_ACTION_FLAG Flags = LEDGER_ACTION_FLAG.None)
        {

        }

    }

}
