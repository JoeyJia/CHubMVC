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
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public partial class DB_KPI_GROUP
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DB_KPI_GROUP()
        {
            this.DB_KPI = new HashSet<DB_KPI>();
        }
    
        public string KPI_GROUP { get; set; }
        public string KPI_GROUP_DESC { get; set; }
        public string ACTIVEIND { get; set; }
        public string GROUP_DESC_SHORT { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DB_KPI> DB_KPI { get; set; }
    }
}
