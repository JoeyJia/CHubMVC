using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubModel.WebArg;
using CHubDBEntity.UnmanagedModel;
using System.Data;

namespace CHubBLL
{
    public class XX_ASN_DIFF_BLL
    {
        private XX_ASN_DIFF_DAL dal;
        public XX_ASN_DIFF_BLL()
        {
            dal = new XX_ASN_DIFF_DAL();
        }

        /// <summary>
        /// 根据条件查询差异订单
        /// </summary>
        /// <param name="AsnDiffSearch"></param>
        /// <returns></returns>
        public List<XX_ASN_DIFF> GetAsnDiffListBySearch(AsnDiffArg AsnDiffSearch)
        {
            return dal.GetAsnDiffListBySearch(AsnDiffSearch);
        }
        /// <summary>
        /// 根据差异id获取明细数据
        /// </summary>
        /// <param name="asndiffid"></param>
        /// <returns></returns>
        public XX_ASN_DIFF GetAsnDiffById(long asndiffid)
        {
            return dal.GetAsnDiffById(asndiffid);
        }
        /// <summary>
        /// 更新订单备注 备注累加
        /// </summary>
        /// <param name="asnid"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool UpdateXXAsnDiffRemark(string asnid, string remark)
        {
            return dal.UpdateXXAsnDiffRemark(asnid, remark);
        }
        /// <summary>
        /// 更新差异数据 空为成功 否则返回失败原因
        /// </summary>
        /// <param name="asndiff"></param>
        /// <returns></returns>
        public string UpdateXXAsnDiff(XX_ASN_DIFF asndiff)
        {
            return dal.UpdateXXAsnDiff(asndiff);
            
        }

        /// <summary>
        /// 根据asn单号获取差异数据
        /// </summary>
        /// <param name="warehouse"></param>
        /// <param name="AsnID"></param>
        /// <returns></returns>
        public List<V_DIS_ASN_BASE> GetAsnDiffInfo(string warehouse, string AsnID)
        {
            return dal.GetAsnDiffInfo(warehouse, AsnID);
        }
        public void SaveAsnDiff(string warehouse, string asnid, out string SaveMsg)
        {
            dal.SaveAsnDiff(warehouse, asnid, out SaveMsg);
        }
    }
}