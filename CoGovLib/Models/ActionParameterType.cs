using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace CoGov.Models
{

    public class ActionParameterType
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string LookupEntity1 { get; set; }

        public string LookupProperty1 { get; set; }

        public string DisplayProperty1 { get; set; }

        public string LookupEntity2 { get; set; }

        public string LookupProperty2 { get; set; }

        public string DisplayProperty2 { get; set; }

        public int UnderlyingType { get; set; }

        [ScaffoldColumn(false)]
        public DateTime TimeStamp { get; set; }

        [NotMapped]
        public PARAMETER_TYPE_UNDERLYING UnderlyingTypeEnum
        {
            get
            {
                return (PARAMETER_TYPE_UNDERLYING)UnderlyingType;
            }
            set
            {
                UnderlyingType = (int)value;
            }
        }

    }

}