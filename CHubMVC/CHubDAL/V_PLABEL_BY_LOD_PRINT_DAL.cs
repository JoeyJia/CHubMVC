using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubCommon;

namespace CHubDAL
{
    public class V_PLABEL_BY_LOD_PRINT_DAL:BaseDAL
    {
        public V_PLABEL_BY_LOD_PRINT_DAL() : base()
        {

        }

        public V_PLABEL_BY_LOD_PRINT_DAL(CHubEntities db) : base(db)
        {

        }

        public List<V_PLABEL_BY_LOD_PRINT> QueryBySearch(string LabelTYPE, string ShipmentNo, string BoxNumber, string PartNumber_RP, string PartNumber_GOMS)
        {
            string sql = @"select *
                    from V_PLABEL_BY_LOD_PRINT where 1=1";
            if (!string.IsNullOrEmpty(LabelTYPE))
            {
                sql += string.Format(" and LABEL_CODE = '{0}'", LabelTYPE);
            }
            if (!string.IsNullOrEmpty(ShipmentNo))
            {
                sql += string.Format(" and SHIP_ID = '{0}'", ShipmentNo);
            }
            if (!string.IsNullOrEmpty(BoxNumber))
            {
                sql += string.Format(" and LODNUM = '{0}'", BoxNumber);
            }
            if (!string.IsNullOrEmpty(PartNumber_RP))
            {
                sql += string.Format(" and PRTNUM = '{0}'", PartNumber_RP);
            }
            if (!string.IsNullOrEmpty(PartNumber_GOMS))
            {
                sql += string.Format(" and PART_NO = '{0}'", PartNumber_GOMS);
            }
            this.CheckCultureInfoForDate();
            var result = db.Database.SqlQuery<V_PLABEL_BY_LOD_PRINT>(sql).ToList();

            return result;
        }


        public List<V_PLABEL_BY_LOD_PRINT> BatchGetLabelPrintData(List<string> VID, string LabelTYPE, string ShipmentNo, string BoxNumber, string PartNumber_RP, string PartNumber_GOMS)
        {
            string sql = string.Format(@"select * from V_PLABEL_BY_LOD_PRINT 
                where VID in ({0}) and LABEL_CODE = '{1}'", VID.ToSqlInStr(),LabelTYPE);
            if (!string.IsNullOrEmpty(ShipmentNo))
                sql += string.Format(@" and SHIP_ID='{0}'", ShipmentNo);
            if (!string.IsNullOrEmpty(BoxNumber))
                sql += string.Format(@" and LODNUM='{0}'", BoxNumber);
            if (!string.IsNullOrEmpty(PartNumber_RP))
                sql += string.Format(@" and PRINT_PART_NO = '{0}'", PartNumber_RP);
            if (!string.IsNullOrEmpty(PartNumber_GOMS))
                sql += string.Format(@" and PRTNUM = '{0}'", PartNumber_GOMS);

            this.CheckCultureInfoForDate();
            var result = db.Database.SqlQuery<V_PLABEL_BY_LOD_PRINT>(sql);
            return result.ToList();
        }


        public string GetADRNAMEByShip_ID(string Ship_ID)
        {
            if (!string.IsNullOrEmpty(Ship_ID))
                return db.V_PLABEL_BY_LOD_PRINT.FirstOrDefault(v => v.SHIP_ID == Ship_ID).ADRNAM;
            else
                return "";
        }

        public string GetADRNAMEByLODNUM(string LODNUM)
        {
            if (!string.IsNullOrEmpty(LODNUM))
                return db.V_PLABEL_BY_LOD_PRINT.FirstOrDefault(v => v.LODNUM == LODNUM).ADRNAM;
            else
                return "";
        }


    }
}
