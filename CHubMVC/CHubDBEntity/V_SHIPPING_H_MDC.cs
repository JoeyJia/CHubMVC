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
    
    public partial class V_SHIPPING_H_MDC
    {
        public string FROM_SYSTEM { get; set; }
        public long SHIPMENT_NO { get; set; }
        public Nullable<System.DateTime> SHIPMENT_DATE { get; set; }
        public Nullable<int> BILL_OF_LADING { get; set; }
        public string SHIP_TO_ABBR { get; set; }
        public string CARRIER_CODE { get; set; }
        public string CARRIER_PRO_NO { get; set; }
        public Nullable<decimal> WEIGHT_SHIPPED { get; set; }
        public Nullable<int> NO_OF_CARTONS { get; set; }
        public Nullable<System.DateTime> REMOTE_CREATE_DATE { get; set; }
        public string TRACKING_NO { get; set; }
        public string TRAN_TYPE { get; set; }
    }
}