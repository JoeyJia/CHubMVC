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
    
    public partial class ITT_PO
    {
        public string PUR_ORDER_ID { get; set; }
        public long PUR_ORDER_LINE_NO { get; set; }
        public long PUR_RELEASE_NO { get; set; }
        public string WAREHOUSE { get; set; }
        public string BUYER_CODE { get; set; }
        public string EC_PUR_ORDER_TYPE_CODE { get; set; }
        public string EC_PUR_ORDER_TYPE_DESC { get; set; }
        public string BUY_FROM_COMPANY { get; set; }
        public string COMPANY_NAME { get; set; }
        public string PART_NO { get; set; }
        public string PUR_ORDER_DESC { get; set; }
        public Nullable<System.DateTime> RELEASE_DATE { get; set; }
        public Nullable<decimal> RELEASE_QTY { get; set; }
        public Nullable<decimal> REMAINING_QTY { get; set; }
        public string UOM_CODE { get; set; }
        public Nullable<System.DateTime> REVISED_DUE_DATE { get; set; }
        public Nullable<System.DateTime> ETA_DATE { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public Nullable<System.DateTime> RECEIVING_DATE { get; set; }
        public Nullable<System.DateTime> LOAD_DATE { get; set; }
        public string BUY_FOR { get; set; }
        public string SUPPLIER_TYPE { get; set; }
        public string PO_STATUS { get; set; }
        public string CREATED_BY { get; set; }
    }
}
