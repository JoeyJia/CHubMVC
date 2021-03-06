﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class MD_REQ_DETAIL
    {
        public decimal MD_REQ_NO { get; set; }
        public decimal REQ_LINE_NO { get; set; }
        public string PART_NO { get; set; }
        public string PART_DESC { get; set; }
        public string CHECK_EXIST { get; set; }
        public string CHECK_PRI_SUP { get; set; }
        public string CHECK_PRI_PB { get; set; }
        public string CHECK_PRI_BPA { get; set; }
        public string CHECK_COST { get; set; }
        public string GLOBAL_PARTNO { get; set; }
        public string COMM_PART { get; set; }
        public string PRODUCT_GROUP_ID { get; set; }
        public string REQ_STATUS { get; set; }
        public string APP_STATUS { get; set; }
        public string APP_COMMENTS { get; set; }
        public string NOTE { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public string PART_DESC_SHORT { get; set; }
        public string GLOBAL_DESC { get; set; }
    }
}
