using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHubDBEntity
{
    public partial class ITT_CUST_LOAD
    {
        public bool HasChanges(ITT_CUST_LOAD compare)
        {
            if (!string.IsNullOrEmpty(compare.TC_GROUP) && this.TC_GROUP != compare.TC_GROUP)
                return true;
            if (compare.DO_RELEASE_DATE != null && this.DO_RELEASE_DATE != compare.DO_RELEASE_DATE)
                return true;
            if (compare.BND_ARRIVAL_DATE != null && this.BND_ARRIVAL_DATE != compare.BND_ARRIVAL_DATE)
                return true;
            if (compare.BND_OUT_DATE != null && this.BND_OUT_DATE != compare.BND_OUT_DATE)
                return true;
            if (compare.NBND_ARRIVAL_DATE != null && this.NBND_ARRIVAL_DATE != compare.NBND_ARRIVAL_DATE)
                return true;

            return false;
        }
    }
}
