using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class IA_PART_AUTOMAP_BLL
    {
        private IA_PART_AUTOMAP_DAL dal;
        public IA_PART_AUTOMAP_BLL()
        {
            dal = new IA_PART_AUTOMAP_DAL();
        }

        public IA_PART_AUTOMAP_BLL(CHubEntities db)
        {
            dal = new IA_PART_AUTOMAP_DAL(db);
        }

        public List<IA_PART_AUTOMAP> GetIAMap(string INPUT_PART, string PRTNUM)
        {
            return dal.GetIAMap(INPUT_PART, PRTNUM);
        }

        public bool CheckPRTNUM(string PRTNUM)
        {
            return dal.CheckPRTNUM(PRTNUM);
        }

        public IA_PART_AUTOMAP GetIaMap(string INPUT_PART)
        {
            return dal.GetIaMap(INPUT_PART);
        }

        public void AddOrUpdateIaMap(IA_PART_AUTOMAP iamap, string Type)
        {
            dal.AddOrUpdateIaMap(iamap, Type);
        }


        public bool IsExistINPUT_PART(string INPUT_PART)
        {
            return dal.IsExistINPUT_PART(INPUT_PART);
        }

        public void DeleteIaMap(IA_PART_AUTOMAP iamap)
        {
            dal.DeleteIaMap(iamap);
        }

    }
}
