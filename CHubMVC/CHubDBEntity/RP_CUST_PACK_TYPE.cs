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
    
    public partial class RP_CUST_PACK_TYPE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RP_CUST_PACK_TYPE()
        {
            this.RP_ADR_MST = new HashSet<RP_ADR_MST>();
        }
    
        public string CUST_PACK_ID { get; set; }
        public string PACK_DESC { get; set; }
        public string ACTIVEIND { get; set; }
        public string AUTO_PRINT { get; set; }
        public string CUST_SHORT_NAME { get; set; }
        public string HEADER1 { get; set; }
        public string HEADER2 { get; set; }
        public string HEADER3 { get; set; }
        public string FOOTER1 { get; set; }
        public string FOOTER2 { get; set; }
        public string FOOTER3 { get; set; }
        public string NOTE1 { get; set; }
        public string NOTE2 { get; set; }
        public string NOTE3 { get; set; }
        public string NOTE4 { get; set; }
        public string NOTE5 { get; set; }
        public string LOGO { get; set; }
        public string COL01 { get; set; }
        public string COL02 { get; set; }
        public string COL03 { get; set; }
        public string COL04 { get; set; }
        public string COL05 { get; set; }
        public string COL06 { get; set; }
        public string COL07 { get; set; }
        public string COL08 { get; set; }
        public string COL09 { get; set; }
        public string COL10 { get; set; }
        public decimal PAPER_HORIZONTAL { get; set; }
        public decimal PAPER_VERTICAL { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RP_ADR_MST> RP_ADR_MST { get; set; }
        public virtual RP_CUST_PACK_TYPE RP_CUST_PACK_TYPE1 { get; set; }
        public virtual RP_CUST_PACK_TYPE RP_CUST_PACK_TYPE2 { get; set; }
    }
}
