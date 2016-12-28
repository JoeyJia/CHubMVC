using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubModel.ExtensionModel;
using CHubCommon;

namespace CHubBLL
{
    public class V_ALIAS_ADDR_DFLT_BLL
    {
        private readonly V_ALIAS_ADDR_DFLT_DAL dal;

        public V_ALIAS_ADDR_DFLT_BLL()
        {
            dal = new V_ALIAS_ADDR_DFLT_DAL();
        }
        public V_ALIAS_ADDR_DFLT_BLL(CHubEntities db)
        {
            dal = new V_ALIAS_ADDR_DFLT_DAL(db);
        }

        public List<ExVAliasAddr> GetAliasAddrDFLT(string shipName, string addr)
        {
            List<V_ALIAS_ADDR_DFLT>  list = dal.GetAliasAddrDFLT(shipName, addr);
            List<ExVAliasAddr> exList = new List<ExVAliasAddr>();

            ClassConvertTable cct = new ClassConvertTable();

            foreach (var item in list)
            {
                ExVAliasAddr ex = new ExVAliasAddr();
                ClassConvert.ConvertAction(item, ex, cct.AliasAddrDFLTConvert);
                exList.Add(ex);
            }

            return exList;
        }

        public V_ALIAS_ADDR_DFLT GetSpecifyAliasAddrDFLT(string aliasName, string sysID, string cusNo, int? bill2Location, int? ship2Location)
        {
            return dal.GetSpecifyAliasAddrDFLT(aliasName, sysID, cusNo, bill2Location, ship2Location);
        }

    }
}
