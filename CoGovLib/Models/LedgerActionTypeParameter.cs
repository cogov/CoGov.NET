using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CoGov.Models
{

    public class LedgerActionTypeParameter
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public Guid ActionTypeId { get; set; }

        public Guid ParameterTypeId { get; set; }

        public int Sequence { get; set; }

        public string Name { get; set; }

        public string DisplayCaption { get; set; }

        public string DisplayHint { get; set; }

        public Guid? KeyPreviousParameter { get; set; }

        public string RelatedEntityName { get; set; }

        public string RelatedPropertyName { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }

        public virtual LedgerActionType ActionType { get; set; }

        public virtual ActionParameterType ParameterType { get; set; }

    }

}