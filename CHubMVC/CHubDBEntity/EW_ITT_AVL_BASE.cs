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
    
    public partial class EW_ITT_AVL_BASE
    {
        public Nullable<System.DateTime> RUN_DATE { get; set; }
        public Nullable<System.DateTime> MEASURE_DATE { get; set; }
        public Nullable<decimal> KPI_GAPS { get; set; }
        public Nullable<System.DateTime> S50_NBND_ARRIVAL_DATE { get; set; }
        public string FROM_SYSTEM { get; set; }
        public long SHIPMENT_NO { get; set; }
        public long SHIPMENT_LINE_NO { get; set; }
        public string SHIP_TO_ABBR { get; set; }
        public string CARRIER_CODE { get; set; }
        public string TRAN_TYPE { get; set; }
        public string PART_NO { get; set; }
        public Nullable<decimal> QTY_SHIPPED { get; set; }
        public string INVOICE_NO { get; set; }
        public string WAY_BILL_NO { get; set; }
        public string CUSTOM_PROCESS_ID { get; set; }
        public Nullable<byte> CLOSE_FLAG { get; set; }
        public Nullable<decimal> COMP_RATIO { get; set; }
    }
}
