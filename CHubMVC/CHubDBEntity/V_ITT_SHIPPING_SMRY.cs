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
    
    public partial class V_ITT_SHIPPING_SMRY
    {
        public Nullable<System.DateTime> SHIPMENT_DATE { get; set; }
        public string FROM_SYSTEM { get; set; }
        public long SHIPMENT_NO { get; set; }
        public string SHIP_TO_ABBR { get; set; }
        public string CARRIER_CODE { get; set; }
        public string TRAN_TYPE { get; set; }
        public string INVOICE_NO { get; set; }
        public string WILL_BILL_NO { get; set; }
        public string TC_GROUP { get; set; }
        public Nullable<System.DateTime> S00_DEPART_DATE { get; set; }
        public Nullable<System.DateTime> S10_PORT_ARRIVAL_DATE { get; set; }
        public Nullable<System.DateTime> S20_DO_RELEASE_DATE { get; set; }
        public Nullable<System.DateTime> S30_BND_ARRIVAL_DATE { get; set; }
        public Nullable<System.DateTime> S40_BND_OUT_DATE { get; set; }
        public Nullable<System.DateTime> S50_NBND_ARRIVAL_DATE { get; set; }
        public decimal SHIP_LINES { get; set; }
    }
}
