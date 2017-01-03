using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using CHubDBEntity;

namespace CHubDAL
{
    public class BaseDAL
    {
        public BaseDAL()
        {
            _db = new CHubEntities();
        }

        public BaseDAL(CHubEntities inDb)
        {
            db = inDb;
        }

        private CHubEntities _db;

        public CHubEntities db
        {
            get
            {
                if (_db == null)
                    throw new Exception("CHubEntities should be init first");
                else
                    return _db;
            }
            set
            { _db = value; }
        }

        /// <summary>
        /// Save All pending changes
        /// </summary>
        public void SaveChanges()
        {
            db.SaveChanges();
        }

        /// <summary>
        /// whether auto save for a specify change
        /// </summary>
        /// <param name="autoSave"></param>
        protected void AutoSave(bool autoSave)
        {
            if (autoSave)
                db.SaveChanges();
        }

        /// <summary>
        /// Function for Add a new model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="autoSave"></param>
        public void Add<T>(T model, bool autoSave = true) where T : class
        {
            db.Entry<T>(model).State = EntityState.Added;
            AutoSave(autoSave);
        }

        /// <summary>
        /// Function for update a exist model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="autoSave"></param>
        public void Update<T>(T model, bool autoSave = true) where T : class
        {
            db.Entry<T>(model).State = EntityState.Modified;
            AutoSave(autoSave);
        }

        /// <summary>
        /// for db.Database.SqlQuery function date CultureInfo check
        /// if CurrentCulture is zh-CN need to change db configuration
        /// otherwise will get wrong date format exception
        /// </summary>
        public void CheckCultureInfoForDate()
        {
            var cur = System.Globalization.CultureInfo.CurrentCulture;//zh-CN
            if (cur.Name == "zh-CN")
            {
                db.Database.ExecuteSqlCommand("alter session set nls_date_language = 'american'");
            }
        }


        public decimal GetOrderSqeNextVal()
        {
            List<decimal> nextVal = db.Database.SqlQuery<decimal>( "select ORDER_SEQ_NO.nextval from dual", string.Empty).ToList();
            decimal result = nextVal[0];
            return result; 
        }

        public decimal GetOrderSqeCurrVal()
        {
            try
            {
                List<decimal> nextVal = db.Database.SqlQuery<decimal>("select ORDER_SEQ_NO.currval from dual", string.Empty).ToList();
                decimal result = nextVal[0];
                return result;
            }
            catch
            {
                return 0.00M;
            }
        }

    }
}
