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
    
    public partial class ITT_TRAN_TYPE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ITT_TRAN_TYPE()
        {
            this.ITT_SHIPPING_H = new HashSet<ITT_SHIPPING_H>();
            this.ITT_TRAN_LOAD = new HashSet<ITT_TRAN_LOAD>();
        }
    
        public string TRAN_TYPE { get; set; }
        public string TRAN_DESC { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ITT_SHIPPING_H> ITT_SHIPPING_H { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ITT_TRAN_LOAD> ITT_TRAN_LOAD { get; set; }
    }
}
