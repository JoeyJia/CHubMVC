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
    
    public partial class RP_WAYBILL_TYPE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RP_WAYBILL_TYPE()
        {
            this.RP_CAR_MST = new HashSet<RP_CAR_MST>();
        }
    
        public string WAYBILL_ID { get; set; }
        public string WAYBILL_DESC { get; set; }
        public string ACTIVEIND { get; set; }
        public string PRINT_DETAIL { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RP_CAR_MST> RP_CAR_MST { get; set; }
    }
}
