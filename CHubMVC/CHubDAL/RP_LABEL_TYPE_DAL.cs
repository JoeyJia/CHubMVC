using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using CHubModel.ExtensionModel;

namespace CHubDAL
{
    public class RP_LABEL_TYPE_DAL : BaseDAL
    {
        public RP_LABEL_TYPE_DAL()
            : base() { }

        public RP_LABEL_TYPE_DAL(CHubEntities db)
            : base(db) { }

        public List<ExRPLabelType> GetLabelTypeExList()
        {
            var result = (
                from a in db.RP_LABEL_TYPE
                select new ExRPLabelType() {
                     LABEL_CODE = a.LABEL_CODE,
                     LABEL_DESC = a.LABEL_DESC
                }
                );

            return result.ToList();
        }

    }
}
