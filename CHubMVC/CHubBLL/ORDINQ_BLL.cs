﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHubDBEntity.UnmanagedModel;
using CHubDAL;

namespace CHubBLL
{
    public class ORDINQ_BLL
    {
        private ORDINQ_DAL dal;
        public ORDINQ_BLL()
        {
            dal = new ORDINQ_DAL();
        }
        public List<V_GOMS_ORDER_H> GetORDER_HList(string CUSTOMER_PO_NO, string ORDER_NO)
        {
            return dal.GetORDER_HList(CUSTOMER_PO_NO, ORDER_NO);
        }
        public List<G_OECUST_SHIPPING_LOCAL> GetSHIPPING_LOCAL(string LOAD_FROM, string CUSTOMER_NO, decimal BILL_TO_LOCATION, decimal SHIP_TO_LOCATION, decimal DEST_LOCATION)
        {
            return dal.GetSHIPPING_LOCAL(LOAD_FROM, CUSTOMER_NO, BILL_TO_LOCATION, SHIP_TO_LOCATION, DEST_LOCATION);
        }
        public List<V_GOMS_ORDER_D> GetORDER_DList(string LOAD_FROM, string ORDER_NO)
        {
            return dal.GetORDER_DList(LOAD_FROM, ORDER_NO);
        }
        public List<V_GOMS_ORDER_R> GetORDER_RList(string LOAD_FROM, string ORDER_NO, string LINE_NO)
        {
            return dal.GetORDER_RList(LOAD_FROM, ORDER_NO, LINE_NO);
        }
        public List<V_SHIP_TRACK_PRO> GetTrackList(string LOAD_FROM, string ORDER_NO)
        {
            return dal.GetTrackList(LOAD_FROM, ORDER_NO);
        }
        public bool CheckPrintSecurity(string SECURE_ID, string APP_USER)
        {
            return dal.CheckPrintSecurity(SECURE_ID, APP_USER);
        }
        public V_OA_H_PRINT SearchV_OA_H_PRINT(string LOAD_FROM, string ORDER_NO)
        {
            return dal.SearchV_OA_H_PRINT(LOAD_FROM, ORDER_NO);
        }
        public OA_TYPE_MST SearchOA_TYPE_MST(string OA_TYPE)
        {
            return dal.SearchOA_TYPE_MST(OA_TYPE);
        }
        public List<V_OA_D_PRINT> SearchV_OA_D_PRINT(string LOAD_FROM, string ORDER_NO)
        {
            return dal.SearchV_OA_D_PRINT(LOAD_FROM, ORDER_NO);
        }
    }
}
