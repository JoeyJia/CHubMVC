﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CHubDBEntity
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CHubEntities : DbContext
    {
        public CHubEntities()
            : base("name=CHubEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<APP_CUST_ALIAS> APP_CUST_ALIAS { get; set; }
        public virtual DbSet<APP_CUST_ALIAS_LINK> APP_CUST_ALIAS_LINK { get; set; }
        public virtual DbSet<APP_DURATION> APP_DURATION { get; set; }
        public virtual DbSet<APP_NOTICE> APP_NOTICE { get; set; }
        public virtual DbSet<APP_ORDER_TYPE> APP_ORDER_TYPE { get; set; }
        public virtual DbSet<APP_PAGE_ROLE_LINK> APP_PAGE_ROLE_LINK { get; set; }
        public virtual DbSet<APP_PAGES> APP_PAGES { get; set; }
        public virtual DbSet<APP_RECENT_PAGES> APP_RECENT_PAGES { get; set; }
        public virtual DbSet<APP_ROLES> APP_ROLES { get; set; }
        public virtual DbSet<APP_SITES> APP_SITES { get; set; }
        public virtual DbSet<APP_SPACE_PAGE_LINK> APP_SPACE_PAGE_LINK { get; set; }
        public virtual DbSet<APP_USER_ALIAS_LINK> APP_USER_ALIAS_LINK { get; set; }
        public virtual DbSet<APP_USER_ROLE_LINK> APP_USER_ROLE_LINK { get; set; }
        public virtual DbSet<APP_USERS> APP_USERS { get; set; }
        public virtual DbSet<APP_WELCOME> APP_WELCOME { get; set; }
        public virtual DbSet<APP_WH> APP_WH { get; set; }
        public virtual DbSet<APP_WORKSPACE> APP_WORKSPACE { get; set; }
        public virtual DbSet<M_PARAMETER> M_PARAMETER { get; set; }
        public virtual DbSet<M_SYSTEM> M_SYSTEM { get; set; }
        public virtual DbSet<G_PART_DESCRIPTION> G_PART_DESCRIPTION { get; set; }
        public virtual DbSet<G_PART_SUPERSESSION_ALL> G_PART_SUPERSESSION_ALL { get; set; }
        public virtual DbSet<P_ADDR_DFLT> P_ADDR_DFLT { get; set; }
        public virtual DbSet<P_ADDR_SPL> P_ADDR_SPL { get; set; }
        public virtual DbSet<P_CATALOG_CUSTOMER_PART> P_CATALOG_CUSTOMER_PART { get; set; }
        public virtual DbSet<R_ADDR_DFLT> R_ADDR_DFLT { get; set; }
        public virtual DbSet<R_ADDR_SPL> R_ADDR_SPL { get; set; }
        public virtual DbSet<R_CATALOG_CUSTOMER_PART> R_CATALOG_CUSTOMER_PART { get; set; }
        public virtual DbSet<V_ALIAS_ADDR_DFLT> V_ALIAS_ADDR_DFLT { get; set; }
        public virtual DbSet<V_ALIAS_ADDR_SPL> V_ALIAS_ADDR_SPL { get; set; }
        public virtual DbSet<V_CHK_DURATION_MISSING> V_CHK_DURATION_MISSING { get; set; }
        public virtual DbSet<V_NAV_ALL> V_NAV_ALL { get; set; }
        public virtual DbSet<V_USER_PAGE_LINK> V_USER_PAGE_LINK { get; set; }
        public virtual DbSet<V_USER_NAV_ALL> V_USER_NAV_ALL { get; set; }
        public virtual DbSet<APP_ORDER_STATUS> APP_ORDER_STATUS { get; set; }
        public virtual DbSet<APP_SHIP_SEQ> APP_SHIP_SEQ { get; set; }
        public virtual DbSet<TS_OR_HEADER> TS_OR_HEADER { get; set; }
        public virtual DbSet<TS_OR_DETAIL> TS_OR_DETAIL { get; set; }
        public virtual DbSet<TS_OR_DETAIL_STAGE> TS_OR_DETAIL_STAGE { get; set; }
        public virtual DbSet<TS_OR_HEADER_STAGE> TS_OR_HEADER_STAGE { get; set; }
        public virtual DbSet<V_O_DOWNLOAD_DTL> V_O_DOWNLOAD_DTL { get; set; }
        public virtual DbSet<V_O_DOWNLOAD_HDR> V_O_DOWNLOAD_HDR { get; set; }
        public virtual DbSet<M_APPS> M_APPS { get; set; }
        public virtual DbSet<TC_PART_CATEGORY> TC_PART_CATEGORY { get; set; }
        public virtual DbSet<TC_PART_CATEGORY_STG> TC_PART_CATEGORY_STG { get; set; }
        public virtual DbSet<TC_PART_HS> TC_PART_HS { get; set; }
        public virtual DbSet<TC_PART_HS_AUDIT> TC_PART_HS_AUDIT { get; set; }
        public virtual DbSet<TC_PART_HS_INI> TC_PART_HS_INI { get; set; }
        public virtual DbSet<TC_PART_HS_STG> TC_PART_HS_STG { get; set; }
        public virtual DbSet<V_TC_CATEGORY_BY_SYS> V_TC_CATEGORY_BY_SYS { get; set; }
        public virtual DbSet<V_TC_IMPORT_WITH_HS> V_TC_IMPORT_WITH_HS { get; set; }
        public virtual DbSet<V_TC_MDM_ALL> V_TC_MDM_ALL { get; set; }
        public virtual DbSet<V_TC_PART_HS> V_TC_PART_HS { get; set; }
        public virtual DbSet<M_PART> M_PART { get; set; }
        public virtual DbSet<ITT_SHIPPING_D_SNAP> ITT_SHIPPING_D_SNAP { get; set; }
        public virtual DbSet<ITT_SHIPPING_H> ITT_SHIPPING_H { get; set; }
        public virtual DbSet<ITT_SHIPPING_H_SNAP> ITT_SHIPPING_H_SNAP { get; set; }
        public virtual DbSet<ITT_TRAN_TYPE> ITT_TRAN_TYPE { get; set; }
        public virtual DbSet<ITT_CUST_LOAD> ITT_CUST_LOAD { get; set; }
        public virtual DbSet<ITT_SHIPPING_D> ITT_SHIPPING_D { get; set; }
        public virtual DbSet<V_ITT_SHIPPING_SMRY> V_ITT_SHIPPING_SMRY { get; set; }
        public virtual DbSet<M_CALENDAR> M_CALENDAR { get; set; }
        public virtual DbSet<V_SHIPPING_ALL_BASE> V_SHIPPING_ALL_BASE { get; set; }
        public virtual DbSet<ITT_TRAN_LOAD> ITT_TRAN_LOAD { get; set; }
        public virtual DbSet<ITT_EASY_QUERY_LOG> ITT_EASY_QUERY_LOG { get; set; }
        public virtual DbSet<M_INV> M_INV { get; set; }
        public virtual DbSet<ITT_EASY_WATCHING> ITT_EASY_WATCHING { get; set; }
        public virtual DbSet<V_OPEN_QTY_ASN_PDC> V_OPEN_QTY_ASN_PDC { get; set; }
        public virtual DbSet<V_OPEN_QTY_ASN_RDC> V_OPEN_QTY_ASN_RDC { get; set; }
        public virtual DbSet<V_OPEN_QTY_PO_PDC> V_OPEN_QTY_PO_PDC { get; set; }
        public virtual DbSet<V_OPEN_QTY_PO_RDC> V_OPEN_QTY_PO_RDC { get; set; }
        public virtual DbSet<V_ITT_SHIPPING_ALLIN1> V_ITT_SHIPPING_ALLIN1 { get; set; }
        public virtual DbSet<ITT_PO> ITT_PO { get; set; }
        public virtual DbSet<ITT_SO> ITT_SO { get; set; }
        public virtual DbSet<DB_KPI> DB_KPI { get; set; }
        public virtual DbSet<DB_KPI_GROUP> DB_KPI_GROUP { get; set; }
        public virtual DbSet<DB_KPI_HISTORY> DB_KPI_HISTORY { get; set; }
        public virtual DbSet<DB_KPI_CODE> DB_KPI_CODE { get; set; }
        public virtual DbSet<EW_MESSAGE> EW_MESSAGE { get; set; }
        public virtual DbSet<EW_MESSAGE_ATTACH> EW_MESSAGE_ATTACH { get; set; }
        public virtual DbSet<EW_SCRIPT> EW_SCRIPT { get; set; }
        public virtual DbSet<EW_LOG> EW_LOG { get; set; }
        public virtual DbSet<EW_MESSAGE_GROUP> EW_MESSAGE_GROUP { get; set; }
        public virtual DbSet<EW_SCHEDULE> EW_SCHEDULE { get; set; }
        public virtual DbSet<EW_SCHEDULE_TASK> EW_SCHEDULE_TASK { get; set; }
        public virtual DbSet<EW_USER_APPLY> EW_USER_APPLY { get; set; }
        public virtual DbSet<RP_ADR_MST> RP_ADR_MST { get; set; }
        public virtual DbSet<RP_CAR_MST> RP_CAR_MST { get; set; }
        public virtual DbSet<RP_CUST_PACK_TYPE> RP_CUST_PACK_TYPE { get; set; }
        public virtual DbSet<RP_PRINTER> RP_PRINTER { get; set; }
        public virtual DbSet<RP_STATION> RP_STATION { get; set; }
        public virtual DbSet<RP_WAYBILL_BASICINFO> RP_WAYBILL_BASICINFO { get; set; }
        public virtual DbSet<RP_WAYBILL_TYPE> RP_WAYBILL_TYPE { get; set; }
        public virtual DbSet<RP_SHIP_TRACK> RP_SHIP_TRACK { get; set; }
        public virtual DbSet<RP_AUTOPACK_LOG> RP_AUTOPACK_LOG { get; set; }
        public virtual DbSet<RP_ORDTYP_MST> RP_ORDTYP_MST { get; set; }
    }
}
