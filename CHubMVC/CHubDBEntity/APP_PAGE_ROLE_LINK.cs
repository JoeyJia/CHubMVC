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
    
    public partial class APP_PAGE_ROLE_LINK
    {
        public string ROLE_NAME { get; set; }
        public string PAGE_NAME { get; set; }
        public string ACTIVEIND { get; set; }
        public Nullable<System.DateTime> CREATION_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
        public string UPDATED_BY { get; set; }
    
        public virtual APP_ROLES APP_ROLES { get; set; }
        public virtual APP_PAGES APP_PAGES { get; set; }
    }
}
