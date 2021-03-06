﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel
{
    public class OrderLineItem
    {
        public decimal OrderLineNo { get; set; }

        public string CustomerPartNo { get; set; }

        public string PartNo { get; set; }

        public string PartNoPlaceHolder { get; set; }

        public string Description { get; set; }

        public string DescCN { get; set; }

        public decimal Qty { get; set; }

        public decimal? PriAVLCheck { get; set; }

        public string PriAVLCheckColor { get; set; }

        public decimal? AltAVLCheck { get; set; }

        public string AltAVLCheckColor { get; set; }

        public string LastCheckNo { get; set; }

        public decimal LastQty { get; set; }

        public string ItemBackColor { get; set; }

        public string WarningMsg { get; set; }

        public string WarningColor { get; set; }
    }
}
