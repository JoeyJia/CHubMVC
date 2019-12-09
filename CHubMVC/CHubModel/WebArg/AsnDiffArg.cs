using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubModel.WebArg
{
    public class AsnDiffArg
    {
        /// <summary>
        /// 仓库
        /// </summary>
        public string WareHouse { get; set; }
        /// <summary>
        /// ASNID
        /// </summary>
        public string AsnNo { get; set; }
        /// <summary>
        /// ASNDiff创建时间
        /// </summary>
        public DateTime? CreateDate_Start { get; set; }
        public DateTime? CreateDate_End { get; set; }
        /// <summary>
        /// 索赔状态 空为全部
        /// </summary>
        public string ResultType { get; set; }
        /// <summary>
        /// 是否关单 0-未关单 1-已关单 -1 全部
        /// </summary>
        public string IsClose { get; set; }
    }
}
