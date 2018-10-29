using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity.UnmanagedModel;
using CHubDAL;

namespace CHubBLL
{
    public class FINLOAD_BLL
    {
        private FINLOAD_DAL dal;
        public FINLOAD_BLL()
        {
            dal = new FINLOAD_DAL();
        }
        public string GetExpVatLoad_Batch()
        {
            return dal.GetExpVatLoad_Batch();
        }

        public void InsertIntoEXP_VAT_LOAD(EXP_VAT_LOAD evl, decimal LOAD_BATCH, string appUser)
        {
            dal.InsertIntoEXP_VAT_LOAD(evl, LOAD_BATCH, appUser);
        }
        public void ExecP_EXP_VAT_LOAD_POST(string LOAD_BATCH)
        {
            dal.ExecP_EXP_VAT_LOAD_POST(LOAD_BATCH);
        }
        public string GetNumOfEXP_VAT_D(string LOAD_BATCH)
        {
            return dal.GetNumOfEXP_VAT_D(LOAD_BATCH);
        }

        public string GetExpXrefLoad_Batch()
        {
            return dal.GetExpXrefLoad_Batch();
        }

        public void InsertIntoEXP_VAT_XREF_LOAD(EXP_VAT_XREF_LOAD item, decimal LOAD_BATCH, string appUser)
        {
            dal.InsertIntoEXP_VAT_XREF_LOAD(item, LOAD_BATCH, appUser);
        }

        public string GetNumOfEXP_VAT_XREF_LOAD(string LOAD_BATCH)
        {
            return dal.GetNumOfEXP_VAT_XREF_LOAD(LOAD_BATCH);
        }

        public string GetExpCollectionLoad_Batch()
        {
            return dal.GetExpCollectionLoad_Batch();
        }
        public void InsertIntoEXP_COLLECTION_LOAD(EXP_COLLECTION_LOAD item, decimal LOAD_BATCH, string appUser)
        {
            dal.InsertIntoEXP_COLLECTION_LOAD(item, LOAD_BATCH, appUser);
        }
        public string GetNumOfEXP_COLLECTION_LOAD(string LOAD_BATCH)
        {
            return dal.GetNumOfEXP_COLLECTION_LOAD(LOAD_BATCH);
        }
    }
}
