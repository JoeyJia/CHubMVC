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
    
    public partial class ITT_SHIPPING_D_SNAP
    {
        public string FROM_SYSTEM { get; set; }
        public long SHIPMENT_NO { get; set; }
        public long SHIPMENT_LINE_NO { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public Nullable<decimal> S40_BND_OUT_DAYS { get; set; }
        public Nullable<decimal> S50_NBND_ARRIVAL_DAYS { get; set; }
        public string CUSTOM_PROCESS_ID { get; set; }
        public Nullable<decimal> S00_DEPART_DAYS { get; set; }
        public Nullable<decimal> S10_PORT_ARRIVAL_DAYS { get; set; }
        public Nullable<decimal> S20_DO_RELEASE_DAYS { get; set; }
        public Nullable<decimal> S30_BND_ARRIVAL_DAYS { get; set; }
    
        public virtual ITT_CUSTOM_PROCESS ITT_CUSTOM_PROCESS { get; set; }
    }
}
