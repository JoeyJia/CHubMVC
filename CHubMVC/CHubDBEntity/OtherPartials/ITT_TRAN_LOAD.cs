using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity
{
    public partial class ITT_TRAN_LOAD
    {
        public bool HasChanges(ITT_TRAN_LOAD compare)
        {
            if (!string.IsNullOrEmpty(compare.TRAN_TYPE) && this.TRAN_TYPE != compare.TRAN_TYPE)
                return true;
            if (compare.DEPART_DATE != null && this.DEPART_DATE != compare.DEPART_DATE)
                return true;
            if (compare.ARRIVAL_DATE != null && this.ARRIVAL_DATE != compare.ARRIVAL_DATE)
                return true;
            if (!string.IsNullOrEmpty(compare.NOTE) && this.NOTE != compare.NOTE)
                return true;
            return false;
        }
        
    }
}
