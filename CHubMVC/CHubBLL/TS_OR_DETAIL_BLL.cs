﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDAL;
using CHubDBEntity;
using CHubModel.ExtensionModel;

namespace CHubBLL
{
    public class TS_OR_DETAIL_BLL
    {
        private readonly TS_OR_DETAIL_DAL dal;

        public TS_OR_DETAIL_BLL()
        {
            dal = new TS_OR_DETAIL_DAL();
        }
        public TS_OR_DETAIL_BLL(CHubEntities db)
        {
            dal = new TS_OR_DETAIL_DAL(db);
        }

        public bool AddDetail(TS_OR_DETAIL model)
        {
            return dal.AddDetail(model);
        }

    }
}
