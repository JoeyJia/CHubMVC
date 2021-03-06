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
    
    public partial class APP_USERS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public APP_USERS()
        {
            this.APP_RECENT_PAGES = new HashSet<APP_RECENT_PAGES>();
            this.APP_USER_ALIAS_LINK = new HashSet<APP_USER_ALIAS_LINK>();
            this.APP_USER_ROLE_LINK = new HashSet<APP_USER_ROLE_LINK>();
            this.EW_USER_APPLY = new HashSet<EW_USER_APPLY>();
        }
    
        public string APP_USER { get; set; }
        public string FIRST_NAME { get; set; }
        public string MIDDLE_INITIAL { get; set; }
        public string LAST_NAME { get; set; }
        public string PHONE { get; set; }
        public string DESCRIPTION { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_BY { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public Nullable<System.DateTime> RECORD_DATE { get; set; }
        public string EMAIL_ADDR { get; set; }
        public string PWD { get; set; }
        public string STATUS { get; set; }
        public Nullable<System.DateTime> LAST_LOGIN { get; set; }
        public string DEF_WH_ID { get; set; }
        public string PRINTER_ID { get; set; }
        public string QA_OB_SIGNER { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<APP_RECENT_PAGES> APP_RECENT_PAGES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<APP_USER_ALIAS_LINK> APP_USER_ALIAS_LINK { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<APP_USER_ROLE_LINK> APP_USER_ROLE_LINK { get; set; }
        public virtual APP_USERS APP_USERS1 { get; set; }
        public virtual APP_USERS APP_USERS2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EW_USER_APPLY> EW_USER_APPLY { get; set; }
        public virtual RP_PRINTER RP_PRINTER { get; set; }
    }
}
