using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;

namespace CHubBLL
{
    public class JD_ADDR_CONVERT_BLL
    {
        private JD_ADDR_CONVERT_DAL dal;

        public JD_ADDR_CONVERT_BLL()
        {
            dal = new JD_ADDR_CONVERT_DAL();
        }

        public JD_ADDR_CONVERT_BLL(CHubEntities db)
        {
            dal = new JD_ADDR_CONVERT_DAL(db);
        }

        public List<JD_ADDR_CONVERT> SearchAdrMap(string GOMS_ADDR, string CREATE_DATE)
        {
            return dal.SearchAdrMap(GOMS_ADDR, CREATE_DATE);
        }

        public JD_ADDR_CONVERT SearchAdrMap(string JID)
        {
            return dal.SearchAdrMap(JID);
        }

        public void UpdateAdrMap(JD_ADDR_CONVERT adrmap)
        {
            dal.UpdateAdrMap(adrmap);
        }

        public List<JD_4_CLASS_MST> GetArea(string type, string province = null, string city = null, string county = null)
        {
            return dal.GetArea(type, province, city, county);
        }

    }
}
