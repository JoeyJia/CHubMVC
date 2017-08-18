using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubModel.ExtensionModel;
using CHubModel.WebArg;

namespace CHubDAL
{
    public class V_INQ_TRACKNUM_DAL : BaseDAL
    {
        public V_INQ_TRACKNUM_DAL()
            : base() { }

        public V_INQ_TRACKNUM_DAL(CHubEntities db)
            : base(db) { }


        public List<TrackNumLevel1> GetTrackNumLevel1(TrackNumQueryArg arg)
        {
            string sql = string.Format(@"select distinct  TRACK_TAB ,
         TRACK_NUM,
         SHIP_ID ,
         WH_ID ,
         SHPSTS_DESC ,
         CARCOD ,
         CARNAM_SHORT ,
         ADRNAM ,
         ADRLN1 ,
         ADRLN2 ,
         PHNNUM ,
         LAST_NAME  From V_INQ_TRACKNUM  
         where WH_ID='{0}' ", arg.WHID);

            if (!string.IsNullOrEmpty(arg.SalesOrdNum))
                sql += string.Format(" and SALES_ORDNUM ='{0}' ", arg.SalesOrdNum);
            if (!string.IsNullOrEmpty(arg.VCCpoNum))
                sql += string.Format(" and VC_CPONUM ='{0}' ", arg.VCCpoNum);
            if (!string.IsNullOrEmpty(arg.TrackNum))
                sql += string.Format(" and TRACK_NUM ='{0}' ", arg.TrackNum);
            if (!string.IsNullOrEmpty(arg.ShipID))
                sql += string.Format(" and SHIP_ID ='{0}' ", arg.ShipID);
            if (!string.IsNullOrEmpty(arg.PRTNum))
                sql += string.Format(" and PRTNUM ='{0}' ", arg.PRTNum);
            if (!string.IsNullOrEmpty(arg.CSTPRT))
                sql += string.Format(" and CSTPRT ='{0}' ", arg.CSTPRT);
            if (!string.IsNullOrEmpty(arg.LodNum))
                sql += string.Format(" and LODNUM ='{0}' ", arg.LodNum);
            if (!string.IsNullOrEmpty(arg.OrdNum))
                sql += string.Format(" and ORDNUM like '%{0}%' ", arg.OrdNum);
            if (!string.IsNullOrEmpty(arg.ADRNam))
                sql += string.Format(" and ADRNAM like '%{0}%' ", arg.ADRNam);
            if (!string.IsNullOrEmpty(arg.ADRLN1))
                sql += string.Format(" and ADRLN1 like '%{0}%' ", arg.ADRLN1);
            if (!string.IsNullOrEmpty(arg.ADRLN2))
                sql += string.Format(" and ADRLN2 like '%{0}%' ", arg.ADRLN2);

            var result = db.Database.SqlQuery<TrackNumLevel1>(sql);

            return result.OrderBy(a=>a.SHIP_ID).ToList();
        }

        public List<TrackNumLevel2> GetTrackNumLevel2(string shipID)
        {
            string sql = string.Format(@"select 
          LODNUM ,
          STGDTE ,
          LODDTE ,
          ORDNUM ,
          ORDLIN ,
          UNTQTY ,
          ORDTYP ,
          SALES_ORDNUM ,
          VC_CPONUM ,
          PRTNUM ,
          CSTPRT ,
          BTCUST ,
          VC_DLRPONUM ,
          MOD_USR_ID ,
          NOTE1 ,
          NOTE2 
          From V_INQ_TRACKNUM
          where SHIP_ID='{0}'", shipID);

            var result = db.Database.SqlQuery<TrackNumLevel2>(sql);

            return result.OrderBy(a=>a.LODNUM).ToList();

        }



    }
}
