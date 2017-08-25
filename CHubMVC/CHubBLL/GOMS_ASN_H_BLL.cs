using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class GOMS_ASN_H_BLL
    {
        private readonly GOMS_ASN_H_DAL dal;

        public GOMS_ASN_H_BLL()
        {
            dal = new GOMS_ASN_H_DAL();
        }
        public GOMS_ASN_H_BLL(CHubEntities db)
        {
            dal = new GOMS_ASN_H_DAL(db);
        }

        public List<GOMS_ASN_H> GetDockData(string wareHouse, string from, string asn, int range)
        {
            return dal.GetDockData(wareHouse, from, asn, range);
        }

        public void SaveDockData(GOMS_ASN_H model)
        {
             dal.Update(model);
        }

    }
}
