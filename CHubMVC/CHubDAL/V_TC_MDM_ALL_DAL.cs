using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubCommon;

namespace CHubDAL
{
    public class V_TC_MDM_ALL_DAL : BaseDAL
    {
        public V_TC_MDM_ALL_DAL()
            : base() { }

        public V_TC_MDM_ALL_DAL(CHubEntities db)
            : base(db) { }

        public List<V_TC_MDM_ALL> GetTCMDMList(string partNo, string hsCode, string declrName, string element, int currentPage, int pageSize,out int totalCount)
        {
            IQueryable<V_TC_MDM_ALL> result = db.V_TC_MDM_ALL;
            if (!string.IsNullOrEmpty(partNo))
                result = result.Where(a => a.PART_NO.Contains(partNo));
            if (!string.IsNullOrEmpty(hsCode))
                result = result.Where(a => a.HSCODE.Contains(hsCode));
            if (!string.IsNullOrEmpty(declrName))
                result = result.Where(a => a.DECLARATION_NAME.Contains(declrName));
            if (!string.IsNullOrEmpty(element))
                result = result.Where(a => a.ELEMENT.Contains(element));

            totalCount = result.Count();

            return result.OrderBy(a=>a.PART_NO).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

    }
}
