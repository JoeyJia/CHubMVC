using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
    public class RP_LABEL_TYPE2_DAL : BaseDAL
    {
        public RP_LABEL_TYPE2_DAL()
            : base()
        { }

        public RP_LABEL_TYPE2_DAL(CHubEntities db)
            : base(db)
        { }


        public List<string> GetLABEL_CODEList()
        {
            return db.RP_LABEL_TYPE2.Where(l => l.BOM_FLAG == "0" && l.ACTIVEIND == "Y").Select(i => i.LABEL_CODE).ToList();
        }


        public string GetLabel_DESC(string Label_TYPE)
        {
            return db.RP_LABEL_TYPE2.FirstOrDefault(r => r.LABEL_CODE == Label_TYPE).LABEL_DESC;
        }

        public List<string> GetLabel_Code()
        {
            return db.RP_LABEL_TYPE2.Where(a => a.BOM_FLAG == "1" && a.ACTIVEIND == "Y").Select(i => i.LABEL_CODE).ToList();
        }

        public string GetBTW(string LABEL_CODE)
        {
            return db.RP_LABEL_TYPE2.FirstOrDefault(r => r.LABEL_CODE == LABEL_CODE).BTW;
        }



        public List<APP_SITES> GetSite()
        {
            return db.APP_SITES.Where(s => s.ACTIVEIND == "Y").ToList();
        }

        public List<RP_PRINTER> ChangeSite(string SITE_NAME)
        {
            return db.RP_PRINTER.Where(r => r.SITE_NAME == SITE_NAME && r.ACTIVEIND == "Y").ToList();
        }


    }
}
