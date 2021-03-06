﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;

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
            string sql = string.Format(@"select * from V_TC_MDM_ALL where 1=1");

            //IQueryable<V_TC_MDM_ALL> 

            if (!string.IsNullOrEmpty(partNo))
                sql += string.Format(@" and PART_NO like '%{0}%'", partNo);
            //    result = result.Where(a => a.PART_NO.Contains(partNo));
            if (!string.IsNullOrEmpty(hsCode))
                sql += string.Format(@" and HSCODE like '%{0}%'", hsCode);
            //    result = result.Where(a => a.HSCODE.Contains(hsCode));
            if (!string.IsNullOrEmpty(declrName))
                sql += string.Format(@" and DECLARATION_NAME like '%{0}%'", declrName);
            //    result = result.Where(a => a.DECLARATION_NAME.Contains(declrName));
            if (!string.IsNullOrEmpty(element))
                sql += string.Format(@" and ELEMENT like '%{0}%'", element);
            //    result = result.Where(a => a.ELEMENT.Contains(element));
            var result = db.Database.SqlQuery<V_TC_MDM_ALL>(sql).ToList();  //db.V_TC_MDM_ALL;;
            totalCount = result.Count();

            return result.OrderBy(a=>a.PART_NO).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public V_TC_MDM_ALL GetSpecifyMDM(string partNo)
        {
            string sql = string.Format(@"select * from V_TC_MDM_ALL where PART_NO='{0}'", partNo);
            var result = db.Database.SqlQuery<V_TC_MDM_ALL>(sql);

            return result.FirstOrDefault(); //db.V_TC_MDM_ALL.FirstOrDefault(a => a.PART_NO == partNo);
        }

        public string GetGOOD_DESC(string HSCODE, string CIQ)
        {
            string sql = string.Format(@"select GOOD_DESC from TC_HSCODE_CIQ_MST where HSCODE = '{0}' and CIQ= '{1}'", HSCODE, CIQ);
            var result = db.Database.SqlQuery<string>(sql).ToList().FirstOrDefault();
            return result == null ? "" : result;
        }

        public string GetELEMENTCK(string PART_NO)
        {
            string sql = string.Format(@"select ELEMENT2 from TC_PART_ELEMENT2 where part_no='{0}'", PART_NO);
            var result = db.Database.SqlQuery<string>(sql).ToList().FirstOrDefault();
            return result == null ? "" : result;
        }

        public List<TC_HSCODE_CIQ_MST> GetCIQLists(string HSCODE)
        {
            string sql = string.Format(@"select * from TC_HSCODE_CIQ_MST where HSCODE='{0}' order by CIQ", HSCODE);
            var result = db.Database.SqlQuery<TC_HSCODE_CIQ_MST>(sql).ToList();
            return result;
        }
    }
}
