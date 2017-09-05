using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CoGov.Models
{

    public class LedgerActionType
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public IList<LedgerActionTypeLabel> TypeIds { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid? ExecuteResult1ActionParameterTypeId { get; set; }

        public Guid? ExecuteResult2ActionParameterTypeId { get; set; }

        public Guid? ExecuteResult3ActionParameterTypeId { get; set; }

        public virtual ActionParameterType ExecuteResult1ActionParameterType { get; set; }

        public virtual ActionParameterType ExecuteResult2ActionParameterType { get; set; }

        public virtual ActionParameterType ExecuteResult3ActionParameterType { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }

        public virtual List<LedgerActionTypeLabel> Types { get; set; }

    }

}