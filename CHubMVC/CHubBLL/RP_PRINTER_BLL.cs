using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;

namespace CHubBLL
{
    public class RP_PRINTER_BLL
    {
        private readonly RP_PRINTER_DAL dal;

        public RP_PRINTER_BLL()
        {
            dal = new RP_PRINTER_DAL();
        }
        public RP_PRINTER_BLL(CHubEntities db)
        {
            dal = new RP_PRINTER_DAL(db);
        }

        public List<RP_PRINTER> GetPrinterList()
        {
            return dal.GetPrinterList();
        }

    }
}
