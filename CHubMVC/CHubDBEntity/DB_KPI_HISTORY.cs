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
    
    public partial class DB_KPI_HISTORY
    {
        public System.DateTime KPI_DATE { get; set; }
        public string KPI_CODE { get; set; }
        public string KPI_SUB_CODE { get; set; }
        public decimal KPI_VALUE { get; set; }
        public Nullable<decimal> KPI_TARGET { get; set; }
        public Nullable<decimal> IND_Y { get; set; }
        public string KPI_OWNER { get; set; }
        public string NOTE { get; set; }
        public string OWNER_HIGHLIGHT { get; set; }
        public Nullable<System.DateTime> HIGHLIGHT_DATE { get; set; }
    
        public virtual DB_KPI DB_KPI { get; set; }
    }
}
