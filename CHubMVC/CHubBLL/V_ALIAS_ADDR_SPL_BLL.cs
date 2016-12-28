﻿using System;
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
    public class V_ALIAS_ADDR_SPL_BLL
    {
        private readonly V_ALIAS_ADDR_SPL_DAL dal;

        public V_ALIAS_ADDR_SPL_BLL()
        {
            dal = new V_ALIAS_ADDR_SPL_DAL();
        }
        public V_ALIAS_ADDR_SPL_BLL(CHubEntities db)
        {
            dal = new V_ALIAS_ADDR_SPL_DAL(db);
        }

        public List<ExVAliasAddr> GetAliasAddrSPL(string localDestName, string addr)
        {
            List<V_ALIAS_ADDR_SPL>  list = dal.GetAliasAddrSPL(localDestName, addr);
            List<ExVAliasAddr> exList = new List<ExVAliasAddr>();

            ClassConvertTable cct = new ClassConvertTable();
            
            foreach (var item in list)
            {
                ExVAliasAddr ex = new ExVAliasAddr();
                ClassConvert.ConvertAction(item, ex, cct.AliasAddrSPLConvert);
                exList.Add(ex);
            }

            return exList; 
        }

        public V_ALIAS_ADDR_SPL GetSpecifyAliasAddrSPL(string aliasName, string sysID, string cusNo, int? bill2Location, long? ship2Location, long? destLocation)
        {
            return dal.GetSpecifyAliasAddrSPL(aliasName, sysID, cusNo, bill2Location, ship2Location, destLocation);
        }

    }
}