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
    
    public partial class EW_USER_APPLY
    {
        public string APP_USER { get; set; }
        public string MESSAGE_ID { get; set; }
        public string APPLY { get; set; }
        public Nullable<System.DateTime> APPLY_DATE { get; set; }
        public System.DateTime SAMPLE_DATE { get; set; }
    
        public virtual APP_USERS APP_USERS { get; set; }
        public virtual EW_MESSAGE EW_MESSAGE { get; set; }
    }
}
