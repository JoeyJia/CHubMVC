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
    
    public partial class APP_SECURE_PROCESS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public APP_SECURE_PROCESS()
        {
            this.APP_SECURE_PROC_ASSIGN = new HashSet<APP_SECURE_PROC_ASSIGN>();
        }
    
        public string SECURE_ID { get; set; }
        public string SECURE_DESC { get; set; }
        public string OWNER { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<APP_SECURE_PROC_ASSIGN> APP_SECURE_PROC_ASSIGN { get; set; }
    }
}
