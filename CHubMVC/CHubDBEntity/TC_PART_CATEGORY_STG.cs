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
    
    public partial class TC_PART_CATEGORY_STG
    {
        public string PART_NO { get; set; }
        public string TC_CATEGORY_BY_MAN { get; set; }
    
        public virtual TC_PART_CATEGORY TC_PART_CATEGORY { get; set; }
        public virtual M_PART M_PART { get; set; }
    }
}
