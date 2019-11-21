using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity.UnmanagedModel;

namespace CHubBLL
{
    public class IhubJob_BLL
    {
        private IhubJob_DAL dal;
        public IhubJob_BLL()
        {
            dal = new IhubJob_DAL();
        }
        public List<V_JOB_LINK_USER> GetJOB_DISPLAY(string App_User)
        {
            return dal.GetJOB_DISPLAY(App_User);
        }
        public V_JOB_LINK_USER GetJOB_DESC(string JOB_NAME, string App_User)
        {
            return dal.GetJOB_DESC(JOB_NAME, App_User);
        }
        public void ExecuteProc(V_JOB_LINK_USER item, List<string> paras)
        {
            dal.ExecuteProc(item, paras);
        }
        public void LogHistory(V_JOB_LINK_USER item, string msg)
        {
            dal.LogHistory(item, msg);
        }
    }
}
