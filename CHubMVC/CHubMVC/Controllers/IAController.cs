using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHubModel;
using CHubBLL;
using CHubCommon;
using CHubModel.WebArg;
using CHubDBEntity;
using CHubMVC.Models;
using CHubBLL.OtherProcess;
using System.Threading;

namespace CHubMVC.Controllers
{
    public class IAController : BaseController
    {
        [Authorize]
        public ActionResult IACode()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.iacode.ToString(), this.Request.Url.AbsoluteUri);

            IAModels vm = new IAModels();
            IA_CODE_TYPE_BLL bll = new IA_CODE_TYPE_BLL();
            vm.iacode = bll.GetIACodes();

            return View(vm);
        }


        [Authorize]
        [HttpPost]
        public ActionResult SaveIaCode(IACodeTypeArg iacode)
        {
            IA_CODE_TYPE_BLL bll = new IA_CODE_TYPE_BLL();
            IA_CODE_TYPE iac = new IA_CODE_TYPE();
            try
            {
                iac = bll.GetIACode(iacode.IA_CODE);
                iac.IA_CODE_DESC = iacode.IA_CODE_DESC;
                iac.DPMO_FLAG = iacode.DPMO_FLAG;
                iac.ACTIVEIND = iacode.ACTIVEIND;
                iac.AUTO_FLAG = iacode.AUTO_FLAG;
                bll.AddOrUpdateIACode(iac, "Update");
                return Json(new RequestResult(true, "The Data Has Saved"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA SaveIaCode");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddIaCode(IACodeTypeArg iacode)
        {
            IA_CODE_TYPE_BLL bll = new IA_CODE_TYPE_BLL();
            IA_CODE_TYPE iac = new IA_CODE_TYPE();
            if (bll.IsExist(iacode.IA_CODE))
            {
                return Json(new RequestResult(false, "The IA_CODE Already Existed"));
            }
            else
            {
                try
                {
                    iac.IA_CODE = iacode.IA_CODE;
                    iac.IA_CODE_DESC = iacode.IA_CODE_DESC;
                    iac.DPMO_FLAG = iacode.DPMO_FLAG;
                    iac.ACTIVEIND = iacode.ACTIVEIND;
                    iac.NOTE = "";
                    iac.CREATE_DATE = DateTime.Now;
                    iac.AUTO_FLAG = iacode.AUTO_FLAG;
                    bll.AddOrUpdateIACode(iac, "Add");
                    return Json(new RequestResult(true, "The Data Has Saved"));
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("IA AddIaCode");
                    return Json(new RequestResult(false, ex.Message));
                }
            }

        }

        [Authorize]
        public ActionResult IAMap()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.iamap.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetIaMap(IAModels vm)
        {
            IA_PART_AUTOMAP_BLL bll = new IA_PART_AUTOMAP_BLL();
            try
            {
                var result = bll.GetIAMap(vm.INPUT_PART, vm.PRTNUM);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA IAMap");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult SaveIaMap(string INPUT_PART, string PRTNUM, string MAP_GROUP, string NOTE)
        {
            IA_PART_AUTOMAP_BLL bll = new IA_PART_AUTOMAP_BLL();
            IA_PART_AUTOMAP iamap = new IA_PART_AUTOMAP();
            if (bll.CheckPRTNUM(PRTNUM)) //check
            {
                try
                {
                    iamap = bll.GetIaMap(INPUT_PART);
                    iamap.PRTNUM = PRTNUM;
                    iamap.MAP_GROUP = MAP_GROUP;
                    iamap.NOTE = NOTE;
                    iamap.RECORD_DATE = DateTime.Now;
                    bll.AddOrUpdateIaMap(iamap, "Update");
                    return Json(new RequestResult(true, "The Data Has Saved"));
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("IA SaveIaMap");
                    return Json(new RequestResult(false, ex.Message));
                }
            }
            else
            {
                return Json(new RequestResult(false, "The PRTNUM does not Exist"));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddIaMap(string INPUT_PART, string PRTNUM, string MAP_GROUP, string NOTE)
        {
            IA_PART_AUTOMAP_BLL bll = new IA_PART_AUTOMAP_BLL();
            IA_PART_AUTOMAP iamap = new IA_PART_AUTOMAP();
            if (bll.IsExistINPUT_PART(INPUT_PART))
                return Json(new RequestResult(false, "The INPUT_PART Has Existed"));
            else
            {
                if (bll.CheckPRTNUM(PRTNUM))
                {
                    iamap.INPUT_PART = INPUT_PART;
                    iamap.PRTNUM = PRTNUM;
                    iamap.MAP_GROUP = MAP_GROUP;
                    iamap.NOTE = NOTE;
                    iamap.CREATE_DATE = DateTime.Now;
                    iamap.USERID = Session[CHubConstValues.SessionUser].ToString();
                    bll.AddOrUpdateIaMap(iamap, "Add");
                    return Json(new RequestResult(true, "The Data Has Saved"));
                }
                else
                {
                    return Json(new RequestResult(false, "The PRTNUM does not Exist"));
                }
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteIaMap(string INPUT_PART)
        {
            IA_PART_AUTOMAP_BLL bll = new IA_PART_AUTOMAP_BLL();
            IA_PART_AUTOMAP iamap = bll.GetIaMap(INPUT_PART);
            try
            {
                bll.DeleteIaMap(iamap);
                return Json(new RequestResult(true, "The Data Has Deleted"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA DeleteIaMap");
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [Authorize]
        public ActionResult PrtAddt()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.prtaddt.ToString(), this.Request.Url.AbsoluteUri);
            IAModels vm = new IAModels();
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult PrtAddt(IAModels vm)
        {
            G_PART_ADDTIONAL_BLL bll = new G_PART_ADDTIONAL_BLL();
            ViewBag.PaperIDList = bll.GetPaperIDList();

            var result = bll.GetPAByPartNo(vm.PART_NO);
            vm.prtaddt = result;

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult SavePrtAddt(GPartAddtionalArg prtaddt)
        {
            G_PART_ADDTIONAL_BLL bll = new G_PART_ADDTIONAL_BLL();
            G_PART_ADDTIONAL gpa = new G_PART_ADDTIONAL();
            try
            {
                gpa = bll.GetPAByPartNo(prtaddt.PART_NO).First();
                gpa.PAPER_ID = prtaddt.PAPER_ID;
                gpa.NOTE = prtaddt.NOTE;
                gpa.MOQ_OVERRIDE = Convert.ToDecimal(prtaddt.MOQ_OVERRIDE);
                gpa.PACKING_MOQ = Convert.ToDecimal(prtaddt.PACKING_MOQ);
                gpa.QC_NOTE = prtaddt.QC_NOTE;
                gpa.MSG_ADDT1 = prtaddt.MSG_ADDT1;
                gpa.MSG_ADDT2 = prtaddt.MSG_ADDT2;
                gpa.MSG_ADDT3 = prtaddt.MSG_ADDT3;
                bll.SavePrtAddt(gpa);
                return Json(new RequestResult(true, "The Data Has Saved"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA SavePrtAddt");
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [Authorize]
        public ActionResult CustAddt()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.custaddt.ToString(), this.Request.Url.AbsoluteUri);
            IAModels vm = new IAModels();
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CustAddt(IAModels vm)
        {
            M_ADRNAM_MST_BLL bll = new M_ADRNAM_MST_BLL();
            ViewBag.CodeList = bll.GetLabelCode();

            var result = bll.GetCustAddt(vm.ADRNAM);
            vm.mam = result;

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult SaveCustAddt(MADRNAMMSTArg custaddt)
        {
            M_ADRNAM_MST_BLL bll = new M_ADRNAM_MST_BLL();
            M_ADRNAM_MST mam = new M_ADRNAM_MST();
            try
            {
                mam = bll.GetCustAddt(custaddt.ADRNAM).First();
                mam.NOTE = custaddt.NOTE;
                mam.QC_NOTE = custaddt.QC_NOTE;
                mam.IA_IGNORE = custaddt.IA_IGNORE;
                mam.LABEL_CODE = custaddt.LABEL_CODE;
                bll.SaveCustAddt(mam);
                return Json(new RequestResult(true, "The Data Has Saved"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA SaveCustAddt");
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [Authorize]
        public ActionResult IAScanTest()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.iascantest.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ConvertIAScan(string ScanStr)
        {
            IA_2DSCAN_LOG_BLL bll = new IA_2DSCAN_LOG_BLL();
            string convertStr = string.Empty;

            //Convert
            try
            {
                convertStr = bll.GetConvertStr(ScanStr);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA ConvertIAScan");
                return Json(new RequestResult(false, ex.Message));
            }

            //log
            IA_2DSCAN_LOG scanlog = new IA_2DSCAN_LOG();
            scanlog.INPUT_STR = ScanStr;
            scanlog.PART_IDENTIFIED = convertStr;
            scanlog.SCAN_DATE = DateTime.Now;
            scanlog.IA_AUDITOR = Session[CHubConstValues.SessionUser].ToString();
            bll.LogScanStr(scanlog);

            //Check
            if (bll.CheckPrintPartNo(convertStr) || bll.CheckInputStr(convertStr))
                return Json(new RequestResult(true, "", convertStr));
            else
                return Json(new RequestResult(false, "零件号不存在", convertStr));
        }



        [Authorize]
        public ActionResult IAScan(string LODNUM_DISPLAY)
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.iascan.ToString(), this.Request.Url.AbsoluteUri);

            ViewBag.LODNUM_DISPLAY = LODNUM_DISPLAY;

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetIACODEList()
        {
            IA_CODE_TYPE_BLL bll = new IA_CODE_TYPE_BLL();
            try
            {
                var result = bll.GetIACODEList();
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA GetIACODEList");
                return Json(new RequestResult(false, ex.Message));
            }
        }



        /// <summary>
        /// Step One
        /// </summary>
        /// <param name="LODNUM_DISPLAY"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult GetInfoFromHDR(string LODNUM_DISPLAY)
        {
            V_IA_LOD_HDR_BLL bll = new V_IA_LOD_HDR_BLL();
            try
            {
                var result = bll.GetInfoFromHDR(LODNUM_DISPLAY);
                if (result != null && result.Any())
                    return Json(new RequestResult(result));
                else
                    return Json(new RequestResult(false));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA GetInfoFromHDR");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// Step Two
        /// </summary>
        /// <param name="LODNUM_DISPLAY"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult GetInfoFromBASE(string LODNUM_DISPLAY)
        {
            V_IA_LOD_BASE_BLL bll = new V_IA_LOD_BASE_BLL();
            try
            {
                var result = bll.GetInfoFromBASE(LODNUM_DISPLAY);
                if (result != null && result.Count() > 0)
                {
                    bll.RunProc(LODNUM_DISPLAY, Session[CHubConstValues.SessionUser].ToString());
                    return Json(new RequestResult(true));
                }
                else
                    return Json(new RequestResult(false, "没找到箱号"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA GetInfoFromBASE");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetPartConvert(string LODNUM, string PRTNUM)
        {
            IA_LOD_DTL_BLL bll = new IA_LOD_DTL_BLL();
            IA_2DSCAN_LOG_BLL logbll = new IA_2DSCAN_LOG_BLL();
            IA_2DSCAN_LOG scanlog = new IA_2DSCAN_LOG();
            try
            {
                var result = bll.GetIaLodDtl(LODNUM, PRTNUM);
                if (result != null && result.Any())  //Exist Part_No=PRTNUM
                {
                    //Log
                    scanlog.INPUT_STR = PRTNUM;
                    scanlog.PART_IDENTIFIED = PRTNUM;
                    scanlog.SCAN_DATE = DateTime.Now;
                    scanlog.IA_AUDITOR = Session[CHubConstValues.SessionUser].ToString();
                    logbll.LogScanStr(scanlog);

                    return Json(new RequestResult(true, "", PRTNUM));
                }
                else
                {
                    string prtnum_new = bll.GetNewPRTNUM(PRTNUM); //Function 

                    scanlog.INPUT_STR = PRTNUM;
                    scanlog.PART_IDENTIFIED = prtnum_new;
                    scanlog.SCAN_DATE = DateTime.Now;
                    scanlog.IA_AUDITOR = Session[CHubConstValues.SessionUser].ToString();
                    logbll.LogScanStr(scanlog);

                    //CheckAgain
                    var result_new = bll.GetIaLodDtl(LODNUM, prtnum_new);
                    if (result_new != null && result_new.Any())
                        return Json(new RequestResult(true, "", prtnum_new)); //Exist Again Part_No = PRTNUM_NEW
                    else
                        return Json(new RequestResult(false, "箱内无此零件:" + prtnum_new, ""));
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA GetPartConvert");
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult GetInfoLODDTL(string LODNUM)
        {
            V_IA_LOD_DTL_BLL bll = new V_IA_LOD_DTL_BLL();
            try
            {
                var result = bll.GetInfoLODDTL(LODNUM);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA GetInfoLODDTL");
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult SaveIALODDTL(IALODDTLArg arg)
        {
            IA_LOD_DTL_BLL bll = new IA_LOD_DTL_BLL();
            IA_LOD_DTL iad = new IA_LOD_DTL();
            try
            {
                if (arg.DList != null && arg.DList.Count() > 0)
                {
                    foreach (var item in arg.DList)
                    {
                        iad = bll.GetIaLodDtl(arg.LODNUM, item.PRTNUM).First();
                        iad.IA_QTY = item.IA_QTY;
                        iad.IA_CODE1 = item.IA_CODE1;
                        iad.IA_CODE2 = item.IA_CODE2;
                        bll.SaveIALODDTL(iad);
                    }
                    return Json(new RequestResult(true, "Success to Save"));
                }
                else
                {
                    return Json(new RequestResult(false, "No Data Save"));
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA SaveIALODDTL");
                return Json(new RequestResult(false, ex.Message));
            }
        }


        /// <summary>
        /// F10 Complete
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult CompleteIALODDTL(IALODDTLArg arg)
        {
            IA_LOD_DTL_BLL bll = new IA_LOD_DTL_BLL();
            V_IA_LOD_HDR_BLL hbll = new V_IA_LOD_HDR_BLL();
            IA_LOD_DTL iad = new IA_LOD_DTL();
            IA_LOD_HDR iah = new IA_LOD_HDR();
            try
            {
                #region Save
                if (arg.DList != null && arg.DList.Count() > 0)
                {
                    foreach (var item in arg.DList)
                    {
                        iad = bll.GetIaLodDtl(arg.LODNUM, item.PRTNUM).First();
                        iad.IA_QTY = item.IA_QTY;
                        iad.IA_CODE1 = item.IA_CODE1;
                        iad.IA_CODE2 = item.IA_CODE2;
                        bll.SaveIALODDTL(iad);
                    }
                }
                #endregion

                #region Check
                List<CheckDTL> cdtl = new List<CheckDTL>();
                if (arg.DList != null && arg.DList.Count() > 0)
                {
                    foreach (var ia in arg.DList)
                    {
                        iad = bll.GetIaLodDtl(arg.LODNUM, ia.PRTNUM).First();
                        if (iad.NOTE != null && iad.IA_QTY == null)
                        {
                            cdtl.Add(new CheckDTL()
                            {
                                PRTNUM = iad.PRTNUM,
                                NOTE = iad.NOTE
                            });
                        }
                    }
                }
                #endregion

                if (cdtl.Count() > 0)
                    return Json(new RequestResult(false, "关注零件：" + cdtl[0].PRTNUM + "未检验！" + cdtl[0].NOTE));
                else
                {
                    var result = hbll.GetIALODHDR(arg.LODNUM);
                    return Json(new RequestResult(result.COMP_COMMENTS));
                }

                #region old
                //iah = hbll.GetIALODHDR(arg.LODNUM);
                //iah.IA_STATUS = "Comp";
                //iah.COMPLETE_DATE = DateTime.Now;
                //hbll.UpdateIALODHDR(iah);

                //if (arg.DList != null && arg.DList.Count() > 0)
                //{
                //    foreach (var item in arg.DList)
                //    {
                //        iad = bll.GetIaLodDtl(arg.LODNUM, item.PRTNUM).First();
                //        iad.IA_QTY = item.IA_QTY;
                //        iad.IA_CODE1 = item.IA_CODE1;
                //        iad.IA_CODE2 = item.IA_CODE2;
                //        bll.SaveIALODDTL(iad);
                //    }
                //    return Json(new RequestResult(true, "Success to Save"));
                //}
                //else
                //{
                //    return Json(new RequestResult(false, "No Data Save"));
                //}
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA SaveIALODDTL");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public class CheckDTL
        {
            public string PRTNUM { get; set; }
            public string NOTE { get; set; }
        }

        [Authorize]
        [HttpPost]
        public ActionResult CompleteComment(string LODNUM, string Comp_Comm)
        {
            V_IA_LOD_HDR_BLL bll = new V_IA_LOD_HDR_BLL();
            IA_LOD_HDR iah = new IA_LOD_HDR();
            try
            {
                iah = bll.GetIALODHDR(LODNUM);
                iah.IA_STATUS = "Comp";
                iah.COMP_COMMENTS = Comp_Comm;
                iah.COMPLETE_DATE = DateTime.Now;
                bll.UpdateIALODHDR(iah);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA CompleteComment");
                return Json(new RequestResult(false, ex.Message));
            }
        }



        [Authorize]
        public ActionResult IAInq()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.iainq.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetIAStatus()
        {
            V_IA_LOD_HDR_BLL bll = new V_IA_LOD_HDR_BLL();
            try
            {
                var result = bll.GetIAStatus();
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA GetIAStatus");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public ActionResult GetIAINQWH_ID()
        {
            APP_WH_BLL bll = new APP_WH_BLL();
            try
            {
                var result = bll.GetAppWHList().Select(a => a.WH_ID);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA GetIAINQWH_ID");
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult GetVIALODHDR(string WH_ID, string ADRNAM, string LODNUM_DISPLAY, string NEED_SIGN_YN, string IA_STATUS, int CREATE_DATE)
        {
            V_IA_LOD_HDR_BLL bll = new V_IA_LOD_HDR_BLL();
            try
            {
                var IAStatusList = bll.GetIAStatus();
                var result = bll.GetVIALODHDR(WH_ID, ADRNAM, LODNUM_DISPLAY, NEED_SIGN_YN, IA_STATUS, CREATE_DATE);
                string html = CreateTable(result, IAStatusList);
                return Json(new RequestResult(html));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA GetVIALODHDR");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public string CreateTable(List<V_IA_LOD_HDR> vilh, List<IA_STATUS_CODE> isc)
        {
            string html = "";
            if (vilh != null & vilh.Any())
            {
                foreach (var item in vilh)
                {
                    html += "<tr data-id=\"" + item.LODNUM + "\">";
                    html += "<td title=\"" + item.LODNUM + "\">" + item.LODNUM + "</td>";
                    html += "<td title=\"" + item.WH_ID + "\">" + item.WH_ID + "</td>";
                    html += "<td title=\"" + item.ADRNAM + "\">" + item.ADRNAM + "</td>";
                    if (item.STGDTE.HasValue)
                        html += "<td title=\"" + item.STGDTE.Value.ToString("yyyy-MM-dd HH:mm:ss") + "\">" + item.STGDTE.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</td>";
                    else
                        html += "<td></td>";
                    html += "<td><select class=\"form-control ia_status\">";
                    html += "<option value=\"\"></option>";
                    foreach (var ic in isc)
                    {
                        if (ic.IA_STATUS == item.IA_STATUS)
                            html += "<option value=\"" + ic.IA_STATUS + "\" title=\"" + ic.STATUS_DESC + "\" selected=\"selected\">" + ic.IA_STATUS + "</option>";
                        else
                            html += "<option value=\"" + ic.IA_STATUS + "\" title=\"" + ic.STATUS_DESC + "\">" + ic.IA_STATUS + "</option>";
                    }
                    html += "</select></td>";
                    html += "<td title=\"" + item.IA_AUDITOR + "\">" + item.IA_AUDITOR + "</td>";
                    html += "<td><input type=\"text\" class=\"form-control note\" value=\"" + item.NOTE + "\" title=\"" + item.NOTE + "\" /></td>";
                    if (item.CREATE_DATE.HasValue)
                        html += "<td title=\"" + item.CREATE_DATE.Value.ToString("yyyy-MM-dd HH:mm:ss") + "\">" + item.CREATE_DATE.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</td>";
                    else
                        html += "<td></td>";
                    if (item.COMPLETE_DATE.HasValue)
                        html += "<td title=\"" + item.COMPLETE_DATE.Value.ToString("yyyy-MM-dd HH:mm:ss") + "\">" + item.COMPLETE_DATE.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</td>";
                    else
                        html += "<td></td>";
                    html += "<td title=\"" + item.COMP_COMMENTS + "\">" + item.COMP_COMMENTS + "</td>";
                    html += "<td title=\"" + item.SIGNER + "\">" + item.SIGNER + "</td>";
                    html += "<td title=\"" + item.SIGN_COMMENTS + "\">" + item.SIGN_COMMENTS + "</td>";
                    html += "<td>" + "<input type=\"button\" class=\"btn btn-primary btn-sm Save\" data-id=\"" + item.LODNUM + "\" value=\"Save\" /><input type=\"button\" class=\"btn btn-primary btn-sm Lines\" data-id=\"" + item.LODNUM + "\" value=\"Lines\" /><br /><input type=\"button\" class=\"btn btn-primary btn-sm PrintReport\" data-id=\"" + item.LODNUM + "\" value=\"Print Report\" /><input type=\"button\" class=\"btn btn-primary btn-sm Sign\" data-id=\"" + item.LODNUM + "\" value=\"SIGN\" />" + "</td>";
                    html += "</tr>";
                }
            }
            return html;
        }

        [Authorize]
        [HttpPost]
        public ActionResult CheckIAINQUser(string LODNUM)
        {
            var user = Session[CHubConstValues.SessionUser].ToString();
            V_IA_LOD_HDR_BLL bll = new V_IA_LOD_HDR_BLL();
            try
            {
                if (bll.CheckUser(user))
                {
                    var result = bll.GetIALODHDR(LODNUM);
                    return Json(new RequestResult(result.SIGN_COMMENTS));
                }
                else
                    return Json(new RequestResult(false, "ERROR: You are not SIGNER!"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA CheckIAINQUser");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult SaveSignComm(string LODNUM, string SIGN_COMMENTS)
        {
            V_IA_LOD_HDR_BLL bll = new V_IA_LOD_HDR_BLL();
            try
            {
                var result = bll.GetIALODHDR(LODNUM);
                result.SIGNER = Session[CHubConstValues.SessionUser].ToString();
                result.SIGN_COMMENTS = SIGN_COMMENTS;
                result.SIGN_DATE = DateTime.Now;
                bll.UpdateIALODHDR(result);
                return Json(new RequestResult(result.LODNUM + "|" + result.SIGNER + "|" + result.SIGN_COMMENTS));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA SaveSignComm");
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult GetVIALODDTL(string LODNUM)
        {
            V_IA_LOD_DTL_BLL bll = new V_IA_LOD_DTL_BLL();
            try
            {
                var result = bll.GetVIALODDTL(LODNUM);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA GetVIALODDTL");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public ActionResult IAReportPrint(string LODNUM)
        {
            V_IA_REPORT_PRINT_BLL bll = new V_IA_REPORT_PRINT_BLL();
            try
            {
                var result = bll.IAReportPrint(LODNUM);
                if (result == null || result.Count == 0)
                    return Json(new RequestResult(false, "Get no page data"));

                string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                LabelPrintBLL lpBLL = new LabelPrintBLL(basePath);
                string fileName = lpBLL.BuildPdfForIAReport(result);
                string webPath = "/temp/" + fileName;

                return Json(new RequestResult(webPath));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA IAReportPrint");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult SaveIALODHDR(string LODNUM, string IA_STATUS, string NOTE)
        {
            V_IA_LOD_HDR_BLL bll = new V_IA_LOD_HDR_BLL();
            try
            {
                var result = bll.GetIALODHDR(LODNUM);
                result.IA_STATUS = IA_STATUS;
                result.NOTE = NOTE;
                bll.UpdateIALODHDR(result);
                return Json(new RequestResult(true, "Success to Save"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA SaveIALODHDR");
                return Json(new RequestResult(false, ex.Message));
            }
        }



        [Authorize]
        public ActionResult IAToday()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.iatoday.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }


        [Authorize]
        public ActionResult GetWHID()
        {
            APP_WH_BLL bll = new APP_WH_BLL();
            try
            {
                var result = bll.GetAppWHList().Select(a => a.WH_ID);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA GetWHID");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        public ActionResult GetWHIDDESC(string WH_ID)
        {
            APP_WH_BLL bll = new APP_WH_BLL();
            try
            {
                var result = bll.GetAppWHList().Where(a => a.WH_ID == WH_ID).Select(p => p.DESCRIPTION).FirstOrDefault();
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA GetWHIDDESC");
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult GetIATodoToday(string WH_ID, string Cust)
        {
            V_IA_TODO_TODAY_BLL bll = new V_IA_TODO_TODAY_BLL();
            try
            {
                bll.RunRefreshProc();
                Thread.Sleep(3000);
                var result = bll.GetIATodoToday(WH_ID, Cust);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA GetIATodoToday");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetMapPRTNUM(string ScanInput)
        {
            IA_PART_AUTOMAP_BLL bll = new IA_PART_AUTOMAP_BLL();
            try
            {
                var result = bll.GetIaMap(ScanInput);
                if (result != null)
                    return Json(new RequestResult(result.PRTNUM));
                else
                    return Json(new RequestResult(""));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA GetMapPRTNUM");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddOrUpdateMap(string ScanInput, string PRTNUM)
        {
            IA_PART_AUTOMAP_BLL bll = new IA_PART_AUTOMAP_BLL();
            try
            {
                if (bll.CheckPRTNUM(PRTNUM))
                {
                    var result = bll.GetIaMap(ScanInput);
                    if (result != null)
                    {
                        result.PRTNUM = PRTNUM;
                        result.RECORD_DATE = DateTime.Now;
                        bll.AddOrUpdateIaMap(result, "Update");
                    }
                    else
                    {
                        IA_PART_AUTOMAP iamap = new IA_PART_AUTOMAP();
                        iamap.INPUT_PART = ScanInput;
                        iamap.PRTNUM = PRTNUM;
                        iamap.MAP_GROUP = "";
                        iamap.NOTE = "";
                        iamap.CREATE_DATE = DateTime.Now;
                        iamap.USERID = Session[CHubConstValues.SessionUser].ToString();
                        bll.AddOrUpdateIaMap(iamap, "Add");
                    }
                    return Json(new RequestResult("The Data Has Saved"));
                }
                else
                {
                    return Json(new RequestResult(false, "No PRTNUM Existed"));
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA AddOrUpdateMap");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        public ActionResult IANOTEC()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetIANOTEC(string PART_NO, string PRINT_PART_NO)
        {
            IA_PRT_NOTE_CUST_BLL bll = new IA_PRT_NOTE_CUST_BLL();
            G_PART_DESCRIPTION_BLL gbll = new G_PART_DESCRIPTION_BLL();
            try
            {
                var gresult = gbll.GetGPartDesc(PART_NO, PRINT_PART_NO);
                if (gresult != null)
                {
                    var result = bll.GetIANOTEC(PART_NO, PRINT_PART_NO);
                    return Json(new { Success = true, Msg = (gresult.DESCRIPTION + gresult.DESC_CN), Part_No = gresult.PART_NO, Data = result });
                    //return Json(new RequestResult(true, (gresult.DESCRIPTION + gresult.DESC_CN), result));
                }
                else
                {
                    return Json(new { Success = true, Msg = "不存在此零件号", Part_No = "", Data = "" });
                    //return Json(new RequestResult(true, "不存在此零件号"));
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA GetIANOTEC");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult SaveIANOTEC(string PART_NO, string ADRNAM, string QC_NOTE, string ACTIVEIND)
        {
            IA_PRT_NOTE_CUST_BLL bll = new IA_PRT_NOTE_CUST_BLL();
            try
            {
                IA_PRT_NOTE_CUST result = bll.GetIANoteC(PART_NO, ADRNAM);
                result.QC_NOTE = QC_NOTE;
                result.ACTIVEIND = ACTIVEIND;
                result.RECORD_DATE = DateTime.Now;
                bll.SaveIANOTEC(result);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA SaveIANOTEC");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetIANOTECNEW(string PART_NO, string ADRNAM)
        {
            V_IA_PRT_NOTE_CUST_ADDNEW_BLL bll = new V_IA_PRT_NOTE_CUST_ADDNEW_BLL();
            try
            {
                var result = bll.GetIANOTECNEW(PART_NO, ADRNAM);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA GetIANOTECNEW");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult IANOTECSaveNew(List<IANOTECADDNEWArg> arg)
        {
            V_IA_PRT_NOTE_CUST_ADDNEW_BLL bll = new V_IA_PRT_NOTE_CUST_ADDNEW_BLL();
            try
            {
                string userID = Session[CHubConstValues.SessionUser].ToString();
                if (arg != null && arg.Count() > 0)
                {
                    foreach (var item in arg)
                    {
                        bll.IANOTECSaveNew(item.PART_NO, item.ADRNAM, item.QC_NOTE, userID);
                    }
                }
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA IANOTECSaveNew");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetDescByPartNo(string PART_NO)
        {
            G_PART_ADDTIONAL_BLL bll = new G_PART_ADDTIONAL_BLL();
            try
            {
                var result = bll.GetDescByPartNo(PART_NO);
                if (result != null)
                    return Json(new RequestResult(result.DESCRIPTION + result.DESC_CN));
                else
                    return Json(new RequestResult("不存在此零件号"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA GetDescByPartNo");
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetDescByPrintPartNo(string PRINT_PART_NO)
        {
            G_PART_ADDTIONAL_BLL bll = new G_PART_ADDTIONAL_BLL();
            try
            {
                var result = bll.GetDescByPrintPartNo(PRINT_PART_NO);
                if (result != null)
                    return Json(new RequestResult(true, result.PART_NO, result.DESCRIPTION + result.DESC_CN));
                else
                    return Json(new RequestResult("不存在此零件号"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("IA GetDescByPrintPartNo");
                return Json(new RequestResult(false, ex.Message));
            }
        }


    }
}