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
    
    public partial class ITT_CARRIER_CODES
    {
        public string FROM_SYSTEM { get; set; }
        public string CARRIER_CODE { get; set; }
        public string CARRIER_DESC { get; set; }
        public string TRAN_TYPE { get; set; }
    
        public virtual ITT_TRAN_TYPE ITT_TRAN_TYPE { get; set; }
    }
}
