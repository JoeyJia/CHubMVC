﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity.UnmanagedModel
{
    public class V_RP_PACK_D_BASE
    {
        public string SHIP_ID { get; set; }
        public string WH_ID { get; set; }
        public string LODNUM { get; set; }
        public string CUST_PACK_ID { get; set; }
        public string PRTNUM { get; set; }
        public decimal? UNTQTY { get; set; }
        public string CSTPRT { get; set; }
        public string ORDTYP { get; set; }
        public string VC_CPONUM { get; set; }
        public string DESCRIPTION { get; set; }
        public string DESC_CN { get; set; }
        public string GOMS_PART_NO { get; set; }
        public decimal? PART_WEIGHT { get; set; }
        public decimal? PART_LENGTH { get; set; }
        public decimal? PART_WIDTH { get; set; }
        public decimal? PART_HEIGHT { get; set; }
        public string LWH { get; set; }
        public string SUPERSEDING_PART_NO { get; set; }
        public string SUPERSEDING_PRINT_PART_NO { get; set; }

    }
}
