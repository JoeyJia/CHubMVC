using System;
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
    public class EW_USER_APPLY_BLL
    {
        private readonly EW_USER_APPLY_DAL dal;

        public EW_USER_APPLY_BLL()
        {
            dal = new EW_USER_APPLY_DAL();
        }
        public EW_USER_APPLY_BLL(CHubEntities db)
        {
            dal = new EW_USER_APPLY_DAL(db);
        }


        public void add(EW_USER_APPLY model)
        {
            dal.Add(model);
        }

        public void SaveApply(List<string> idList,string group, string appUser)
        {
            List<EW_USER_APPLY> applies = dal.GetUserApplyByGroup(group, appUser);
            foreach (var item in applies)
            {
                if (idList.Contains(item.MESSAGE_ID))
                {
                    item.APPLY = CHubConstValues.IndY;
                    item.APPLY_DATE = DateTime.Now;
                    dal.Update(item, false);
                    idList.Remove(item.MESSAGE_ID);
                }
                else {
                    item.APPLY = CHubConstValues.IndN;
                    item.APPLY_DATE = DateTime.Now;
                    dal.Update(item, false);
                }    
            }
            if (idList.Count != 0)
            {
                foreach (var id in idList)
                {
                    EW_USER_APPLY apply = new EW_USER_APPLY();
                    apply.MESSAGE_ID = id;
                    apply.APP_USER = appUser;
                    apply.APPLY = CHubConstValues.IndY;
                    apply.APPLY_DATE = DateTime.Now;
                    apply.SAMPLE_DATE = DateTime.Now;
                    dal.Add(apply, false);
                }
            }

            dal.SaveChanges();


        }

        public List<string> GetApplyUsersMail(string messageID)
        {
            return dal.GetApplyUsersMail(messageID);
        }

    }
}
