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
    
    public partial class TMP_ASN
    {
        public string ASN_NO { get; set; }
        public string BUY_FROM_COMPANY { get; set; }
        public string COMPANY_NAME { get; set; }
        public string MANIFEST_CODE { get; set; }
        public string WAREHOUSE { get; set; }
        public long ASN_LINE_NO { get; set; }
        public string PART_NO { get; set; }
        public string PRINT_PART_NO { get; set; }
        public string DESCRIPTION { get; set; }
        public string MSC_CODE { get; set; }
        public Nullable<decimal> QTY_SHIPPED { get; set; }
        public Nullable<decimal> QTY_RECEIVED { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public Nullable<System.DateTime> LAST_RECEIVING_DATE { get; set; }
    }
}