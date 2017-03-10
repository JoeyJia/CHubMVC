using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity;
using System.Data.Entity;

namespace CHubDAL
{
    public class M_CALENDAR_DAL : BaseDAL
    {
        public M_CALENDAR_DAL()
            : base() { }

        public M_CALENDAR_DAL(CHubEntities db)
            : base(db) { }

        public List<M_CALENDAR> GetDateRange(DateTime from, int offsetDays)
        {
            return db.M_CALENDAR.Where(a =>  a.CAL_DATE > from).Take(offsetDays).OrderBy(a=>a.CAL_DATE).ToList();
        }

    }
}
