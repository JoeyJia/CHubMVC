//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CHubDBEntity
{
    using System;
    using System.Collections.Generic;
    
    public partial class RP_ADR_MST
    {
        public string WH_ID { get; set; }
        public string ADRNAM { get; set; }
        public Nullable<System.DateTime> LOAD_DATE { get; set; }
        public string CUST_PACK_ID { get; set; }
    
        public virtual RP_CUST_PACK_TYPE RP_CUST_PACK_TYPE { get; set; }
    }
}
