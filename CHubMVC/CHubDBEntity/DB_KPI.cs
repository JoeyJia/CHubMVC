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
    
    public partial class DB_KPI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DB_KPI()
        {
            this.DB_KPI_HISTORY = new HashSet<DB_KPI_HISTORY>();
        }
    
        public string KPI_GROUP { get; set; }
        public string KPI_CODE { get; set; }
        public string KPI_SUB_CODE { get; set; }
        public string KPI_DESC { get; set; }
        public Nullable<decimal> KPI_TARGET { get; set; }
        public Nullable<decimal> IND_Y { get; set; }
        public string KPI_OWNER { get; set; }
        public string NOTE { get; set; }
        public string ACTIVEIND { get; set; }
    
        public virtual DB_KPI_GROUP DB_KPI_GROUP { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DB_KPI_HISTORY> DB_KPI_HISTORY { get; set; }
        public virtual DB_KPI_CODE DB_KPI_CODE { get; set; }
    }
}
