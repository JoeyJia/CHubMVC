using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;

namespace CHubDAL
{
   public class RP_LABEL_TYPE2_DAL:BaseDAL
    {
        public RP_LABEL_TYPE2_DAL()
            : base() { }

        public RP_LABEL_TYPE2_DAL(CHubEntities db)
            : base(db) { }


        public List<string> GetLABEL_CODEList()
        {
            return db.RP_LABEL_TYPE2.Where(l=>l.BOM_FLAG=="0").Select(i => i.LABEL_CODE).ToList();
        }


        public string GetLabel_DESC(string Label_TYPE)
        {
            return db.RP_LABEL_TYPE2.FirstOrDefault(r => r.LABEL_CODE == Label_TYPE).LABEL_DESC;
        }


    }
}
