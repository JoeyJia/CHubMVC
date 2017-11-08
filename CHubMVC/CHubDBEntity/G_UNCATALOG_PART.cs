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
    using Newtonsoft.Json;
    
    public partial class G_UNCATALOG_PART
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public G_UNCATALOG_PART()
        {
            this.G_KITS = new HashSet<G_KITS>();
        }
    
        public string PART_NO { get; set; }
        public string DESCRIPTION { get; set; }
        public string DESC_CN { get; set; }
        public string SHORT_DESCRIPTION { get; set; }
        public string COUNTRY_OF_ORIGIN { get; set; }
        public string UNIT_MEAS { get; set; }
        public string PART_STATUS { get; set; }
        public Nullable<decimal> PART_HEIGHT { get; set; }
        public Nullable<decimal> PART_LENGTH { get; set; }
        public Nullable<decimal> PART_WIDTH { get; set; }
        public Nullable<decimal> PART_WEIGHT { get; set; }
        public string CURRENT_SALES_STATUS_CODE { get; set; }
        public string PRINT_PART_NO { get; set; }
        public Nullable<decimal> MIN_ORDER_QTY { get; set; }
        public decimal QTY_IN_CARTON { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public Nullable<System.DateTime> RECORD_DATE { get; set; }
        public string NOTE_TEXT { get; set; }
    
        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<G_KITS> G_KITS { get; set; }
    }
}
