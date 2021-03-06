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
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public partial class TS_OR_HEADER
    {
        public decimal ORDER_REQ_NO { get; set; }
        public decimal SHIPFROM_SEQ { get; set; }
        public string TO_SYSTEM { get; set; }
        public string CUSTOMER_NO { get; set; }
        public decimal BILL_TO_LOCATION { get; set; }
        public decimal SHIP_TO_LOCATION { get; set; }
        public decimal DEST_LOCATION { get; set; }
        public System.DateTime DUE_DATE { get; set; }
        public string ORDER_TYPE { get; set; }
        public string CUSTOMER_PO_NO { get; set; }
        public string SPL_IND { get; set; }
        public string SHIPCOMP_FLAG { get; set; }
        public string ORDER_STATUS { get; set; }
        public System.DateTime CREATION_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> RECORD_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public string ALIAS_NAME { get; set; }
        public string ORDER_NOTES { get; set; }

        [JsonIgnore]
        public virtual APP_CUST_ALIAS APP_CUST_ALIAS { get; set; }
        [JsonIgnore]
        public virtual APP_ORDER_STATUS APP_ORDER_STATUS { get; set; }
        [JsonIgnore]
        public virtual APP_ORDER_TYPE APP_ORDER_TYPE { get; set; }
        [JsonIgnore]
        public virtual APP_SHIP_SEQ APP_SHIP_SEQ { get; set; }
        [JsonIgnore]
        public virtual M_SYSTEM M_SYSTEM { get; set; }
    }
}
