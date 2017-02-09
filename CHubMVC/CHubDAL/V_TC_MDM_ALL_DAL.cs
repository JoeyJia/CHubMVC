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

        public List<V_TC_MDM_ALL> GetTCMDMList(string partNo, string hsCode, string declrName, string element)
        {
            IQueryable<V_TC_MDM_ALL> result = null;
            if (!string.IsNullOrEmpty(partNo))
                result = db.V_TC_MDM_ALL.Where(a => a.PART_NO==partNo);
            if (!string.IsNullOrEmpty(hsCode))
                result = db.V_TC_MDM_ALL.Where(a => a.HSCODE==hsCode);
            if (!string.IsNullOrEmpty(declrName))
                result = db.V_TC_MDM_ALL.Where(a => a.DECLARATION_NAME.Contains(declrName));
            if (!string.IsNullOrEmpty(element))
                result = db.V_TC_MDM_ALL.Where(a => a.ELEMENT.Contains(element));
            return result.ToList();
        }

    }
}
