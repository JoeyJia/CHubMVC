using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHubCommon;
using CHubDBEntity;
using CHubDBEntity.UnmanagedModel;
using CHubModel;
using CHubBLL;
using System.Text;
using CHubModel.WebArg;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace CHubMVC.Controllers
{
    public class MDController : BaseController
    {
        /// <summary>
        /// MDJVITEM
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        public ActionResult MDJvItem()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.mdjvitem.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        /// <summary>
        /// MDJVITEM SEARCH
        /// </summary>
        /// <param name="Part_NO"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MDJvItemSearch(string Part_NO)
        {
            V_MD_ENTITY_ITEM_BLL meiBLL = new V_MD_ENTITY_ITEM_BLL();
            string mainHTML = string.Empty;
            try
            {
                var result = meiBLL.MDJvItemSearch(Part_NO);
                mainHTML = GetMDJvItemHTML(result);
                return Json(new RequestResult(mainHTML));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD MDJvItemSearch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// MDJVITEM　HTML
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public string GetMDJvItemHTML(List<V_MD_ENTITY_ITEM> result)
        {
            StringBuilder sb = new StringBuilder();
            if (result != null && result.Any())
            {
                foreach (var item in result)
                {
                    sb.Append("     <tr>");
                    sb.Append("         <td title=\"" + item.PRODUCT_GROUP_ID + "\">").Append(item.PRODUCT_GROUP_ID).Append("</td>");
                    sb.Append("         <td title=\"" + item.GROUP_DESC + "\">").Append(item.GROUP_DESC).Append("</td>");
                    sb.Append("         <td title=\"" + item.PART_NO + "\">").Append(item.PART_NO).Append("</td>");
                    sb.Append("         <td title=\"" + item.PART_DESC + "\">").Append(item.PART_DESC).Append("</td>");
                    sb.Append("         <td title=\"" + item.PART_STATUS + "\">").Append(item.PART_STATUS).Append("</td>");
                    sb.Append("         <td title=\"" + item.NOTE + "\">").Append(item.NOTE).Append("</td>");
                    sb.Append("         <td title=\"" + item.PROD_LINE + "\">").Append(item.PROD_LINE).Append("</td>");
                    sb.Append("         <td title=\"" + item.PART_TYPE + "\">").Append(item.PART_TYPE).Append("</td>");
                    sb.Append("         <td title=\"" + item.DESC_CN + "\">").Append(item.DESC_CN).Append("</td>");
                    sb.Append("     </tr>");
                }
            }
            return sb.ToString();
        }

        [Authorize]
        /// <summary>
        /// MDREQINQ
        /// </summary>
        /// <returns></returns>
        public ActionResult MDReqInq()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.mdreqinq.ToString(), this.Request.Url.AbsoluteUri);
            ViewBag.AppUser = appUser;
            return View();
        }

        [HttpPost]
        public ActionResult GetAPP_STATUS()
        {
            V_MD_REQ_ALL1ONE_BLL vBLL = new V_MD_REQ_ALL1ONE_BLL();
            try
            {
                var result = vBLL.GetAPP_STATUS();
                var app_statusList = result.Select(i => i.APP_STATUS).ToList();
                return Json(new RequestResult(app_statusList));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD GetAPP_STATUS", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult GetAPP_STATUS_DESC(string APP_STATUS)
        {
            V_MD_REQ_ALL1ONE_BLL vBLL = new V_MD_REQ_ALL1ONE_BLL();
            try
            {
                var result = vBLL.GetAPP_STATUS();
                var app_status_desc = result.Where(r => r.APP_STATUS == APP_STATUS).Select(a => a.APP_STATUS_DESC).ToList().First();
                return Json(new RequestResult(app_status_desc));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD GetAPP_STATUS_DESC", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [HttpPost]
        public ActionResult MDReqInqSearch(string MD_REQ_NO, string PART_NO, string REQ_DATE, string APP_STATUS, string REQ_BY, string WWID, string CHECK_EXIST, string COMM_PART)
        {
            V_MD_REQ_ALL1ONE_BLL mraBLL = new V_MD_REQ_ALL1ONE_BLL();
            string mainHTML = string.Empty;
            try
            {
                var result = mraBLL.MDReqInqSearch(MD_REQ_NO, PART_NO, REQ_DATE, APP_STATUS, REQ_BY, WWID, CHECK_EXIST, COMM_PART);
                var existList = result.Select(r => r.CHECK_EXIST).Distinct().ToList();
                mainHTML = GetMDReqInqHTML(result);
                return Json(new { Success = true, Data = mainHTML, ExistList = existList });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD MDReqInqSearch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// Change GLOBAL_PARTNO
        /// </summary>
        /// <param name="PART_NO"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPartDesc(string PART_NO)
        {
            V_MD_REQ_ALL1ONE_BLL mraBLL = new V_MD_REQ_ALL1ONE_BLL();
            try
            {
                var result = mraBLL.GetPartDesc(PART_NO);
                if (result != null && result.Any())
                    return Json(new { Success = true, DESC = result.First().DESCRIPTION, SHORT_DESC = result.First().PART_SHORT_DESC });
                else
                    return Json(new { Success = true, DESC = "", SHORT_DESC = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD GetPartDesc", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// SaveBtn
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MDReqInqSave(MDReqDetailArg arg)
        {
            V_MD_REQ_ALL1ONE_BLL mraBLL = new V_MD_REQ_ALL1ONE_BLL();
            MD_REQ_DETAIL mrd = new MD_REQ_DETAIL();
            try
            {
                //权限控制
                var appUser = Session[CHubConstValues.SessionUser].ToString();
                if (mraBLL.IsOperated("MD_REQ_CHANGE", appUser))
                {
                    //查询数据
                    mrd = mraBLL.GetMDReqDetail(arg.MD_REQ_NO.ToString(), arg.REQ_LINE_NO.ToString()).First();
                    //更新
                    mrd.PART_DESC = arg.PART_DESC;
                    mrd.NOTE = arg.NOTE;
                    mrd.GLOBAL_PARTNO = arg.GLOBAL_PARTNO;
                    mrd.GLOBAL_DESC = arg.GLOBAL_DESC;
                    mrd.PART_DESC_SHORT = arg.PART_DESC_SHORT;
                    mrd.PRODUCT_GROUP_ID = arg.PRODUCT_GROUP_ID;
                    mrd.COMM_PART = arg.COMM_PART;

                    mraBLL.UpdateMDReqInq(mrd);
                    return Json(new RequestResult(true));
                }
                else
                    return Json(new RequestResult(false, "You cannot operate"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD MDReqInqSave", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        /// <summary>
        /// Change Status
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MDReqInqStatusChange(MDReqDetailArg arg)
        {
            V_MD_REQ_ALL1ONE_BLL mraBLL = new V_MD_REQ_ALL1ONE_BLL();
            try
            {
                //权限控制
                var appUser = Session[CHubConstValues.SessionUser].ToString();
                if (mraBLL.IsOperated("MD_REQ_APPROVE", appUser))
                {
                    if (arg.detaillist != null && arg.detaillist.Any())
                    {
                        foreach (var item in arg.detaillist)
                        {
                            mraBLL.UpdateMDReqInqStatus(item, arg.APP_STATUS, arg.APP_COMMENTS);
                        }
                        return Json(new RequestResult(true));
                    }
                    else
                        return Json(new RequestResult(false, "No select data!"));
                }
                else
                    return Json(new RequestResult(false, "You cannot operate!"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD MDReqInqStatusChange", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public string GetMDReqInqHTML(List<V_MD_REQ_ALL1ONE> result)
        {
            StringBuilder sb = new StringBuilder();
            if (result != null && result.Any())
            {
                foreach (var item in result)
                {
                    string CanEdit = string.Empty;
                    if (item.APP_STATUS == "COMP")
                        CanEdit = "disabled=\"disabled\"";

                    sb.Append("     <tr>");
                    sb.Append("         <td>").Append("<input type=\"checkbox\" class=\"selectCheck\" data-mdreqno=\"" + item.MD_REQ_NO + "\" data-reqlineno=\"" + item.REQ_LINE_NO + "\" " + CanEdit + " />").Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"button\" class=\"btn btn-primary btn-sm btnSave\" data-mdreqno=\"" + item.MD_REQ_NO + "\" data-reqlineno=\"" + item.REQ_LINE_NO + "\" value=\"Save\" />").Append("</td>");
                    //sb.Append("         <td>").Append(item.REQ_STATUS).Append("</td>");
                    sb.Append("         <td>").Append(item.MD_REQ_NO).Append("</td>");
                    sb.Append("         <td>").Append(item.REQ_LINE_NO).Append("</td>");
                    sb.Append("         <td>").Append(item.PART_NO).Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control PART_DESC\" value=\"" + item.PART_DESC + "\" title=\"" + item.PART_DESC + "\" " + CanEdit + " />").Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control NOTE\" value=\"" + item.NOTE + "\" title=\"" + item.NOTE + "\" " + CanEdit + " />").Append("</td>");
                    sb.Append("         <td>").Append(item.SR_COMMENTS).Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control GLOBAL_PARTNO\" value=\"" + item.GLOBAL_PARTNO + "\" title=\"" + item.GLOBAL_PARTNO + "\" " + CanEdit + " />").Append("</td>");
                    sb.Append("         <td>").Append(item.GLOBAL_DESC).Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control PART_DESC_SHORT\" value=\"" + item.PART_DESC_SHORT + "\" title=\"" + item.PART_DESC_SHORT + "\" " + CanEdit + " />").Append("</td>");
                    //sb.Append("         <td>").Append(item.APP_STATUS).Append("</td>");//Approval
                    GetColorOfAPP_STATUS(sb, item.APP_STATUS);//Approval
                    sb.Append("         <td>").Append(item.APP_COMMENTS).Append("</td>");
                    sb.Append("         <td>").Append(GetProductGroupID(item.PRODUCT_GROUP_ID, CanEdit)).Append("</td>");
                    sb.Append("         <td>").Append(GetCommPart(item.COMM_PART, CanEdit)).Append("</td>");
                    sb.Append("         <td>").Append(item.REQ_BY).Append("</td>");
                    sb.Append("         <td>").Append(item.REQ_DATE.Value.ToString("yyyy-MM-dd")).Append("</td>");
                    sb.Append("         <td>").Append(item.CHECK_EXIST == "OK" ? "<span style=\"color:green\">" + item.CHECK_EXIST + "</span>" : "<span style=\"color:red\">" + item.CHECK_EXIST + "</span>").Append("</td>");
                    sb.Append("         <td>").Append(item.CHECK_PRI_SUP == "OK" ? "<span style=\"color:green\">" + item.CHECK_PRI_SUP + "</span>" : "<span style=\"color:red\">" + item.CHECK_PRI_SUP + "</span>").Append("</td>");
                    sb.Append("         <td>").Append(item.CHECK_PRI_PB == "OK" ? "<span style=\"color:green\">" + item.CHECK_PRI_PB + "</span>" : "<span style=\"color:red\">" + item.CHECK_PRI_PB + "</span>").Append("</td>");
                    sb.Append("         <td>").Append(item.CHECK_PRI_BPA == "OK" ? "<span style=\"color:green\">" + item.CHECK_PRI_BPA + "</span>" : "<span style=\"color:red\">" + item.CHECK_PRI_BPA + "</span>").Append("</td>");
                    sb.Append("         <td>").Append(item.CHECK_COST == "OK" ? "<span style=\"color:green\">" + item.CHECK_COST + "</span>" : "<span style=\"color:red\">" + item.CHECK_COST + "</span>").Append("</td>");
                    sb.Append("         <td>").Append(item.REQ_DESC).Append("</td>");
                    //sb.Append("         <td>").Append(item.GLOBAL_PART_DESC).Append("</td>");

                    sb.Append("     </tr>");
                }
            }


            return sb.ToString();
        }

        public void GetColorOfAPP_STATUS(StringBuilder sb, string APP_STATUS)
        {
            switch (APP_STATUS)
            {
                case "APPROVED":
                    sb.Append("<td style=\"color:green;\">").Append(APP_STATUS).Append("</td>");
                    break;
                case "REJECTED":
                    sb.Append("<td style=\"color:red;\">").Append(APP_STATUS).Append("</td>");
                    break;
                default:
                    sb.Append("<td style=\"color:blue;\">").Append(APP_STATUS).Append("</td>");
                    break;
            }
        }


        public string GetProductGroupID(string PRODUCT_GROUP_ID, string CanEdit)
        {
            var result = new V_MD_REQ_ALL1ONE_BLL().GetProductGroupID();
            StringBuilder sb = new StringBuilder();
            sb.Append("     <select class=\"form-control input-sm PRODUCT_GROUP_ID\" " + CanEdit + ">");
            if (string.IsNullOrEmpty(PRODUCT_GROUP_ID))
                sb.Append("     <option value=\"\" selected></option>");

            foreach (var item in result)
            {
                string selected = string.Empty;
                if (item.PRODUCT_GROUP_ID == PRODUCT_GROUP_ID)
                    selected = "selected";

                sb.Append("     <option value=\"" + item.PRODUCT_GROUP_ID + "\" title=\"" + item.GROUP_DESC + "\" " + selected + ">").Append(item.PRODUCT_GROUP_ID).Append("</option>");
            }
            sb.Append("     </select>");

            return sb.ToString();
        }

        public string GetCommPart(string COMM_PART, string CanEdit)
        {
            StringBuilder sb = new StringBuilder();
            string[] cp = new string[] { "Y", "N", "NA", "TBD" };
            sb.Append("     <select class=\"form-control input-sm COMM_PART\">");
            foreach (var item in cp)
            {
                string selected = string.Empty;
                if (item == COMM_PART)
                    selected = "selected";

                sb.Append("     <option value=\"" + item + "\" " + selected + ">").Append(item).Append("</option>");
            }
            sb.Append("     </select>");

            return sb.ToString();
        }

        [Authorize]
        public ActionResult MDReq()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.mdreq.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [HttpPost]
        public ActionResult CallFunction(string str, decimal lineno = 0)
        {
            if (!string.IsNullOrEmpty(str))
            {
                MD_REQ_BLL mrBLL = new MD_REQ_BLL();
                List<MDReqArg> mrList = new List<MDReqArg>();

                string[] pdList = str.Split(new char[] { '\r', '\n' }, StringSplitOptions.None);
                pdList = pdList.Where(s => !string.IsNullOrEmpty(s)).ToArray();

                string partno = string.Empty; string description = string.Empty; string note = string.Empty;

                List<string> samePartno = new List<string>();

                foreach (var pd in pdList)
                {
                    MDReqArg mr = new MDReqArg();
                    lineno++;
                    if (pd.IndexOf("\t") > 0)
                    {
                        partno = pd.Split('\t')[0];
                        description = pd.Split('\t')[1];
                        if (pd.Split('\t').ToList().Count == 3)
                            note = pd.Split('\t')[2];
                    }
                    else
                        partno = pd;

                    if (samePartno.Contains(partno))    //copy相同的partno
                        continue;
                    samePartno.Add(partno);

                    partno = partno.Length > 25 ? partno.Substring(0, 25) : partno;
                    if (!string.IsNullOrEmpty(description))
                    {
                        description = description.Length > 35 ? description.Substring(0, 35) : description;
                    }

                    mr.REQ_LINE_NO = lineno;
                    mr.PART_NO = mrBLL.GetPART_NO(partno);
                    mr.PART_DESC = !string.IsNullOrEmpty(description) ? description : mrBLL.GetPART_DESC(partno);
                    mr.NOTE = note;//新增NOTE
                    mr.CHECK_EXIST = mrBLL.GetCHECK_EXIST(partno);
                    mr.GLOBAL_PARTNO = mrBLL.GetGLOBAL_PARTNO(partno);
                    mr.GLOBAL_PARTDESC = mrBLL.GetGLOBAL_PARTDESC(mr.GLOBAL_PARTNO);
                    mr.SHORT_DESC = mrBLL.GetSHORT_DESC(mr.GLOBAL_PARTNO);
                    mr.CHECK_PRI_SUP = mrBLL.GetCHECK_PRI_SUP(partno);
                    mr.CHECK_PRI_PB = mrBLL.GetCHECK_PRI_PB(partno);
                    mr.CHECK_PRI_BPA = mrBLL.GetCHECK_PRI_BPA(partno);
                    mr.CHECK_COST = mrBLL.GetCHECK_COST(partno);
                    mr.PRODUCT_GROUP_ID = mrBLL.GetPRODUCT_GROUP_ID(partno);

                    mrList.Add(mr);
                }

                var mainHTML = GetMDReqHTML(mrList);
                return Json(new RequestResult(mainHTML));
            }
            else
                return Json(new RequestResult(false, "No Data!"));

        }

        public string GetMDReqHTML(List<MDReqArg> mrlist)
        {
            StringBuilder sb = new StringBuilder();

            if (mrlist != null && mrlist.Any())
            {
                foreach (var item in mrlist)
                {
                    sb.Append("     <tr>");
                    sb.Append("         <td title=\"" + item.REQ_LINE_NO + "\">").Append(item.REQ_LINE_NO).Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm txtPART_NO\" value=\"" + item.PART_NO + "\" title=\"" + item.PART_NO + "\" maxlength=\"25\" />").Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm txtPART_DESC\" value=\"" + item.PART_DESC + "\" title=\"" + item.PART_DESC + "\" maxlength=\"35\" />").Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm txtNOTE\" value=\"" + item.NOTE + "\" title=\"" + item.NOTE + "\" />").Append("</td>");
                    sb.Append("         <td>").Append(item.CHECK_EXIST.IndexOf("OK") >= 0 ? "<span style=\"color:green;\">" + item.CHECK_EXIST + "</span>" : "<span style=\"color:red;\">" + item.CHECK_EXIST + "</span>").Append("</td>");
                    sb.Append("         <td>").Append(item.GLOBAL_PARTNO).Append("</td>");
                    sb.Append("         <td>").Append(item.GLOBAL_PARTDESC).Append("</td>");
                    sb.Append("         <td>").Append(item.SHORT_DESC).Append("</td>");
                    sb.Append("         <td>").Append(item.CHECK_PRI_SUP == "OK" ? "<span style=\"color:green;\">" + item.CHECK_PRI_SUP + "</span>" : "<span style=\"color:red;\">" + item.CHECK_PRI_SUP + "</span>").Append("</td>");
                    sb.Append("         <td>").Append(item.CHECK_PRI_PB.IndexOf("OK") >= 0 ? "<span style=\"color:green;\">" + item.CHECK_PRI_PB + "</span>" : "<span style=\"color:red;\">" + item.CHECK_PRI_PB + "</span>").Append("</td>");
                    sb.Append("         <td>").Append(item.CHECK_PRI_BPA == "OK" ? "<span style=\"color:green;\">" + item.CHECK_PRI_BPA + "</span>" : "<span style=\"color:red;\">" + item.CHECK_PRI_BPA + "</span>").Append("</td>");
                    sb.Append("         <td>").Append(item.CHECK_COST == "OK" ? "<span style=\"color:green;\">" + item.CHECK_COST + "</span>" : "<span style=\"color:red;\">" + item.CHECK_COST + "</span>").Append("</td>");
                    sb.Append("         <td>").Append(GetProductGroupID(item.PRODUCT_GROUP_ID, "")).Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"button\" class=\"btn btn-primary btn-sm btnDelete\" value=\"delete\" />").Append("</td>");
                    sb.Append("     </tr>");
                }
            }
            return sb.ToString();
        }

        [HttpPost]
        public ActionResult MDReqNewLine(int Length)
        {
            try
            {
                string mainHTML = GetNewLineHTML(Length);
                return Json(new RequestResult(mainHTML));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD MDReqNewLine", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public string GetNewLineHTML(int Length)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("     <tr>");
            if (Length == 0)
                sb.Append("         <td>").Append(1).Append("</td>");
            else
                sb.Append("         <td>").Append(Length + 1).Append("</td>");
            sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm txtPART_NO\" value=\"\" title=\"\" maxlength=\"25\" />").Append("</td>");
            sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm txtPART_DESC\" value=\"\" title=\"\" maxlength=\"35\" />").Append("</td>");
            sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm txtNOTE\" value=\"\" title=\"\" />").Append("</td>");
            sb.Append("         <td>").Append("<span></span>").Append("</td>");
            sb.Append("         <td>").Append("").Append("</td>");
            sb.Append("         <td>").Append("").Append("</td>");
            sb.Append("         <td>").Append("").Append("</td>");
            sb.Append("         <td>").Append("<span></span>").Append("</td>");
            sb.Append("         <td>").Append("<span></span>").Append("</td>");
            sb.Append("         <td>").Append("<span></span>").Append("</td>");
            sb.Append("         <td>").Append("<span></span>").Append("</td>");
            sb.Append("         <td>").Append(GetProductGroupID("", "")).Append("</td>");
            sb.Append("         <td>").Append("<input type=\"button\" class=\"btn btn-primary btn-sm btnDelete\" value=\"delete\" />").Append("</td>");
            sb.Append("     </tr>");
            return sb.ToString();
        }

        /// <summary>
        /// 获取系统号
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult GetRequestNo(List<MDReqArg> MDReqList)
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            bool check = true;
            //只取EXIST和PRI_PB都不包含OK得去判断；EXIST包含OK不用去判断PART_DESC
            foreach (var item in MDReqList)
            {
                if (item.CHECK_EXIST.Contains("OK"))
                {
                    if (item.CHECK_PRI_PB.Contains("OK"))
                        continue;
                    else
                    {
                        if (string.IsNullOrEmpty(item.PRODUCT_GROUP_ID))
                        {
                            check = false;
                            break;
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(item.PART_NO) || string.IsNullOrEmpty(item.PART_DESC) || string.IsNullOrEmpty(item.PRODUCT_GROUP_ID))
                    {
                        check = false;
                        break;
                    }
                }
                #region old
                //if (item.CHECK_EXIST.Contains("OK") && item.CHECK_PRI_PB.Contains("OK"))
                //    continue;
                //else
                //{
                //    if (string.IsNullOrEmpty(item.PART_NO) || string.IsNullOrEmpty(item.PART_DESC) || string.IsNullOrEmpty(item.PRODUCT_GROUP_ID))
                //    {
                //        check = false;
                //        break;
                //    }
                //}
                #endregion
            }
            if (!check)
                return Json(new RequestResult(false, "Part NO or DESCRIPTION or PROD GROUP cannot be null"));

            //EXIST和PRI_PB都包含OK不需要去Convert
            var NewList = MDReqList.Where(a => a.CHECK_EXIST.Contains("OK") && a.CHECK_PRI_PB.Contains("OK"));
            if (NewList.Count() == MDReqList.Count())
                return Json(new RequestResult(false, "No Data need to Convert"));

            MD_REQ_BLL mrBLL = new MD_REQ_BLL();
            try
            {
                //CHECK_EXIST都为OK，只要执行Proc就行，不需要弹出窗
                var count = MDReqList.Where(m => m.CHECK_EXIST.Contains("OK")).Count();
                if (count == MDReqList.Count())
                {
                    foreach (var item in MDReqList)
                    {
                        if (!(item.CHECK_PRI_PB.Contains("OK")))
                            mrBLL.ExecP_MD_SR_NEW(item.PART_NO, item.PRODUCT_GROUP_ID, item.NOTE, appUser);
                    }
                    return Json(new RequestResult(true, "EXISTOK"));
                }

                var result = mrBLL.GetRequestNo();
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD GetRequestNo", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult MDReqSave(string MD_REQ_NO, string REQ_DESC, List<MDReqArg> MDReqList)
        {
            MD_REQ_BLL mrBLL = new MD_REQ_BLL();
            MD_REQ_HEADER mrHeader = new MD_REQ_HEADER();
            MD_REQ_DETAIL mrDetail = new MD_REQ_DETAIL();

            try
            {
                string appUser = Session[CHubConstValues.SessionUser].ToString();

                //表MD_REQ_HEADER 保存
                mrHeader.MD_REQ_NO = Convert.ToDecimal(MD_REQ_NO);
                mrHeader.REQ_DESC = REQ_DESC;
                mrHeader.REQ_BY = Session[CHubConstValues.SessionUser].ToString();
                mrBLL.SaveMD_REQ_HEADER(mrHeader);

                //EXIST和PRI_PB都为OK不添加
                var NewList = MDReqList.Where(a => a.CHECK_EXIST.Contains("OK") && a.CHECK_PRI_PB.Contains("OK"));
                if (NewList.Count() == MDReqList.Count())
                    return Json(new RequestResult(false, "No Data need to Convert"));

                foreach (var item in MDReqList)
                {
                    //CHECK_EXIST不包含OK添加到MD_REQ_DETAIL表中
                    if (!(item.CHECK_EXIST.Contains("OK")))
                    {
                        mrDetail.MD_REQ_NO = Convert.ToDecimal(MD_REQ_NO);
                        mrDetail.REQ_LINE_NO = item.REQ_LINE_NO;
                        mrDetail.PART_NO = item.PART_NO;
                        mrDetail.PART_DESC = item.PART_DESC;
                        mrDetail.NOTE = item.NOTE;
                        mrDetail.CHECK_EXIST = item.CHECK_EXIST;
                        mrDetail.CHECK_PRI_SUP = item.CHECK_PRI_SUP;
                        mrDetail.CHECK_PRI_PB = item.CHECK_PRI_PB;
                        mrDetail.CHECK_PRI_BPA = item.CHECK_PRI_BPA;
                        mrDetail.CHECK_COST = item.CHECK_COST;
                        mrDetail.GLOBAL_PARTNO = item.GLOBAL_PARTNO;
                        mrDetail.PRODUCT_GROUP_ID = item.PRODUCT_GROUP_ID;
                        mrDetail.PART_DESC_SHORT = item.SHORT_DESC;
                        mrDetail.GLOBAL_DESC = item.GLOBAL_PARTDESC;
                        mrBLL.SaveMD_REQ_DETAIL(mrDetail);//保存
                    }

                    //当CHECK_PRI_PB不包含OK 且 CHECK_EXIST不包含OK，包含OK的已在上一步完成
                    if (!(item.CHECK_PRI_PB.Contains("OK")) && !(item.CHECK_EXIST.Contains("OK")))
                        mrBLL.ExecP_MD_SR_NEW(item.PART_NO, item.PRODUCT_GROUP_ID, item.NOTE, appUser);
                }
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD MDReqSave", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult MDReq_PARTNOChange(string PART_NO)
        {
            MD_REQ_BLL mrBLL = new MD_REQ_BLL();
            MDReqArg mrArg = new MDReqArg();
            try
            {
                mrArg.PART_NO = mrBLL.GetPART_NO(PART_NO);
                mrArg.PART_DESC = mrBLL.GetPART_DESC(PART_NO);
                mrArg.CHECK_EXIST = mrBLL.GetCHECK_EXIST(PART_NO);
                mrArg.GLOBAL_PARTNO = mrBLL.GetGLOBAL_PARTNO(PART_NO);
                mrArg.GLOBAL_PARTDESC = mrBLL.GetGLOBAL_PARTDESC(mrArg.GLOBAL_PARTNO);
                mrArg.SHORT_DESC = mrBLL.GetSHORT_DESC(mrArg.GLOBAL_PARTNO);
                mrArg.CHECK_PRI_SUP = mrBLL.GetCHECK_PRI_SUP(PART_NO);
                mrArg.CHECK_PRI_PB = mrBLL.GetCHECK_PRI_PB(PART_NO);
                mrArg.CHECK_PRI_BPA = mrBLL.GetCHECK_PRI_BPA(PART_NO);
                mrArg.CHECK_COST = mrBLL.GetCHECK_COST(PART_NO);
                mrArg.PRODUCT_GROUP_ID = mrBLL.GetPRODUCT_GROUP_ID(PART_NO);
                return Json(new RequestResult(mrArg));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD MDReq_PARTNOChange", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult MDReqDownload(List<MDReqArg> MDReqList)
        {
            string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            string fullname = basePath + filename;
            NPOIExcelHelper npoiExcel = new NPOIExcelHelper(fullname);
            DataTable dt = new DataTable();
            dt.Columns.Add("REQ_LINE_NO");
            dt.Columns.Add("PART_NO");
            dt.Columns.Add("PART_DESC");
            dt.Columns.Add("NOTE");
            dt.Columns.Add("CHECK_EXIST");
            dt.Columns.Add("GLOBAL_PARTNO");
            dt.Columns.Add("GLOBAL_PARTDESC");
            dt.Columns.Add("SHORT_DESC");
            dt.Columns.Add("CHECK_PRI_SUP");
            dt.Columns.Add("CHECK_PRI_PB");
            dt.Columns.Add("CHECK_PRI_BPA");
            dt.Columns.Add("CHECK_COST");
            dt.Columns.Add("PRODUCT_GROUP_ID");
            if (MDReqList != null && MDReqList.Any())
            {
                foreach (var item in MDReqList)
                {
                    DataRow dr = dt.NewRow();
                    dr["REQ_LINE_NO"] = item.REQ_LINE_NO;
                    dr["PART_NO"] = item.PART_NO;
                    dr["PART_DESC"] = item.PART_DESC;
                    dr["NOTE"] = item.NOTE;
                    dr["CHECK_EXIST"] = item.CHECK_EXIST;
                    dr["GLOBAL_PARTNO"] = item.GLOBAL_PARTNO;
                    dr["GLOBAL_PARTDESC"] = item.GLOBAL_PARTDESC;
                    dr["SHORT_DESC"] = item.SHORT_DESC;
                    dr["CHECK_PRI_SUP"] = item.CHECK_PRI_SUP;
                    dr["CHECK_PRI_PB"] = item.CHECK_PRI_PB;
                    dr["CHECK_PRI_BPA"] = item.CHECK_PRI_BPA;
                    dr["CHECK_COST"] = item.CHECK_COST;
                    dr["PRODUCT_GROUP_ID"] = item.PRODUCT_GROUP_ID;
                    dt.Rows.Add(dr);
                }
            }
            npoiExcel.DataTableToExcel(dt, "Sheet1");
            return Json(new RequestResult(fullname));
        }

        public ActionResult DownLoad(string fullname)
        {
            return File(fullname, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(fullname));
        }

        [Authorize]
        public ActionResult MDSR()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.mdreq.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetSR_STATUS()
        {
            V_MD_SR_ALL_BLL bll = new V_MD_SR_ALL_BLL();
            try
            {
                var result = bll.GetSR_STATUS();
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD GetSR_STATUS", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetSR_STATUS_DESC(string SR_STATUS)
        {
            V_MD_SR_ALL_BLL bll = new V_MD_SR_ALL_BLL();
            try
            {
                var result = bll.GetSR_STATUS_DESC(SR_STATUS);
                if (result != null)
                    return Json(new RequestResult(result.SR_STATUS_DESC));
                else
                    return Json(new RequestResult(""));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD GetSR_STATUS_DESC", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult MDSRSearch(string PART_NO, string COMPANY_CODE, string SR_STATUS, string REQ_DATE, string IS_COMMON)
        {
            V_MD_SR_ALL_BLL bll = new V_MD_SR_ALL_BLL();
            try
            {
                var result = bll.MDSRSearch(PART_NO, COMPANY_CODE, SR_STATUS, REQ_DATE, IS_COMMON);
                var mainHtml = GetMDSRHTML(result);
                return Json(new RequestResult(mainHtml));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD MDSRSearch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult CheckCOMPANY_CODE(string COMPANY_CODE)
        {
            V_MD_SR_ALL_BLL msaBLL = new V_MD_SR_ALL_BLL();
            try
            {
                var result = msaBLL.CheckCOMPANY_CODE(COMPANY_CODE);
                if (result != null)
                    return Json(new RequestResult(result.COMPANY_NAME));
                else
                    return Json(new RequestResult(false, "The COMPANY_CODE is not Exist"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD CheckCOMPANY_CODE", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult MDSRSave(MDReqSRArg arg)
        {
            V_MD_SR_ALL_BLL bll = new V_MD_SR_ALL_BLL();
            try
            {
                //权限控制
                if (IsOperate("MD_SR_MAINT"))
                {
                    bll.MDSRSave(arg);//保存到表MD_REQ_SR
                    return Json(new RequestResult(true));
                }
                else
                    return Json(new RequestResult(false, "You cannot operate"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD MDSRSave", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult MDSRSaveCheck(List<MDReqSRArg> srList)
        {
            V_MD_SR_ALL_BLL bll = new V_MD_SR_ALL_BLL();
            try
            {
                foreach (var item in srList)
                {
                    bll.MDSRSave(item);
                }
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD MDSRSaveCheck", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult RunP_MD_SR_UPD_Status()
        {
            V_MD_SR_ALL_BLL vBLL = new V_MD_SR_ALL_BLL();
            try
            {
                vBLL.RunP_MD_SR_UPD_Status();
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD RunP_MD_SR_UPD_Status", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult MDSRIsShowModal(string status)
        {
            V_MD_SR_ALL_BLL bll = new V_MD_SR_ALL_BLL();
            try
            {
                if (status == "CONFIRM")
                {
                    if (IsOperate("MD_SR_CONFIRM"))
                        return Json(new RequestResult(true));
                    else
                        return Json(new RequestResult(false, "You cannot operate"));
                }
                else
                {
                    if (IsOperate("MD_SR_MAINT"))
                        return Json(new RequestResult(true));
                    else
                        return Json(new RequestResult(false, "You cannot operate"));
                }                
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD MDSRIsShowModal", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult MDSRChangeStatus(List<MDReqSRArg> MDSRList, string SR_STATUS, string SR_COMMENTS)
        {
            V_MD_SR_ALL_BLL bll = new V_MD_SR_ALL_BLL();
            try
            {
                var SR_REQ_NO = MDSRList.Select(a => a.SR_REQ_NO.ToString()).ToList();
                bll.MDSRChangeStatus(SR_REQ_NO, SR_STATUS, SR_COMMENTS);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD MDSRChangeStatus", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult MDSRUploadTemplate()
        {
            V_MD_SR_ALL_BLL vBLL = new V_MD_SR_ALL_BLL();
            int num = 0;
            try
            {
                //权限控制
                if (IsOperate("MD_SR_MAINT"))
                {
                    HttpPostedFileBase hpf = Request.Files[0];
                    string tempGuid = Guid.NewGuid().ToString();
                    string folderPath = Server.MapPath(CHubConstValues.ChubTempFolder);
                    FileInfo folder = new FileInfo(folderPath);
                    if (!Directory.Exists(folder.FullName))
                        Directory.CreateDirectory(folder.FullName);

                    string fileFullName = folder.FullName + tempGuid + ".xlsx";
                    hpf.SaveAs(fileFullName);

                    NPOIExcelHelper npoiHelper = new NPOIExcelHelper(fileFullName);
                    DataTable dt = npoiHelper.ExcelToDataTable();
                    System.IO.File.Delete(fileFullName);
                    if (dt == null && dt.Rows.Count == 0)
                        return Content("No data in excel");

                    List<MDSRLOADArg> lists = ClassConvert.DataTableToList<MDSRLOADArg>(dt);
                    if (lists == null || lists.Count == 0)
                        return Content("Wrong excel strut");

                    var SR_LOAD_SEQ = vBLL.GetSR_LOAD_SEQ();
                    string appUser = Session[CHubConstValues.SessionUser].ToString();
                    foreach (var item in lists)
                    {
                        num++;
                        vBLL.InsertMD_SR_LOAD(SR_LOAD_SEQ, item, appUser);
                    }
                    return Json(new RequestResult(true, "本次成功导入记录数量：" + num + "条"));
                }
                else
                    return Json(new RequestResult(false, "You cannot operate"));

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD MDSRUploadTemplate", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult MDSRDownloadTemplate()
        {
            V_MD_SR_ALL_BLL bll = new V_MD_SR_ALL_BLL();
            try
            {
                string basePath = Server.MapPath(CHubConstValues.ChubTempFolder);
                //call Function
                string result = bll.CallGET_SQL();
                string filename = result.Split('~')[0] + ".xlsx";
                string sql = result.Split('~')[1];
                string fullname = basePath + filename;
                NPOIExcelHelper npoiHelper = new NPOIExcelHelper(fullname);
                DataTable dt = bll.RunSql(sql);
                npoiHelper.DataTableToExcel(dt, "Sheet1");
                return Json(new RequestResult(fullname));
                //string filename = "SR" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                //string fullname = basePath + filename;
                //NPOIExcelHelper npoiHelper = new NPOIExcelHelper(fullname);
                //DataTable dt = bll.MDSRDownloadTemp();
                //npoiHelper.DataTableToExcel(dt, "Sheet1");
                //return Json(new RequestResult(fullname));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD MDSRDownloadTemplate", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult MDSRUploadCopy(string str)
        {
            V_MD_SR_ALL_BLL msaBLL = new V_MD_SR_ALL_BLL();
            MDSRLOADArg mslArg;
            //判断整数
            Regex regexZS = new Regex(@"^\d+$");
            //判断小数
            Regex regexXS = new Regex(@"^\d+(\.\d+)?$");
            StringBuilder sb = new StringBuilder();
            var num = 0;

            try
            {
                //权限控制
                if (IsOperate("MD_SR_MAINT"))
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        var srList = str.Split(new char[] { '\r', '\n' }, StringSplitOptions.None);
                        srList = srList.Where(s => !string.IsNullOrEmpty(s)).ToArray();
                        var count = srList[0].Split('\t').Count();
                        if (count != 7)//copy的数据列不等于7列
                            return Json(new RequestResult(false, "The Data is UnComplete"));
                        else
                        {
                            #region 检查前五列
                            foreach (var sr in srList)
                            {
                                num++;
                                var item = sr.Split('\t');
                                if (string.IsNullOrEmpty(item[0].ToString()))//PART_NO不可为空
                                    sb.Append("第" + num + "行PART_NO为空,");
                                if (string.IsNullOrEmpty(item[1].ToString()))//COMPANY_CODE不可为空
                                    sb.Append("第" + num + "行COMPANY_CODE为空,");
                                if (string.IsNullOrEmpty(item[2]))//PRICE不可为空
                                    sb.Append("第" + num + "行PRICE为空,");
                                else if (!(regexZS.IsMatch(item[2]) || regexXS.IsMatch(item[2])))//PRICE是数字
                                    sb.Append("第" + num + "行PRICE不是数字,");
                                if (string.IsNullOrEmpty(item[3]))//MOQ不可为空
                                    sb.Append("第" + num + "行MOQ为空,");
                                else if (!(regexZS.IsMatch(item[3]) || regexXS.IsMatch(item[3])))//MOQ是数字
                                    sb.Append("第" + num + "行MOQ不是数字,");
                                if (string.IsNullOrEmpty(item[4]))//LT不可为空
                                    sb.Append("第" + num + "行LT为空,");
                                else if (!(regexZS.IsMatch(item[4]) || regexXS.IsMatch(item[4])))//LT是数字
                                    sb.Append("第" + num + "行LT不是数字,");
                            }
                            #endregion

                            //有错误信息
                            if (sb != null && sb.Length > 0)
                            {
                                sb.Remove(sb.Length - 1, 1);
                                return Json(new RequestResult(false, sb.ToString()));
                            }
                            else//没有错误信息插入表MD_SR_LOAD ( SEQ_SR_LOAD.NEXTVAL)
                            {
                                var SR_LOAD_SEQ = msaBLL.GetSR_LOAD_SEQ();//( SEQ_SR_LOAD.NEXTVAL)
                                foreach (var sr in srList)
                                {
                                    var item = sr.Split('\t');
                                    mslArg = new MDSRLOADArg();
                                    mslArg.PART_NO = item[0];
                                    mslArg.COMPANY_CODE = item[1];
                                    mslArg.PRICE = item[2];
                                    mslArg.MOQ = item[3];
                                    mslArg.LT = item[4];
                                    mslArg.IS_COMMON = item[5];
                                    mslArg.COMMENTS = item[6];
                                    msaBLL.InsertMD_SR_LOAD(SR_LOAD_SEQ, mslArg, Session[CHubConstValues.SessionUser].ToString());
                                }
                                return Json(new RequestResult(true, "本次成功导入记录数量：" + num + "条"));
                            }
                        }
                    }
                    else
                        return Json(new RequestResult(false, "No Data"));
                }
                else
                    return Json(new RequestResult(false, "You cannot operate"));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD MDSRUploadCopy", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public bool IsOperate(string SECURE_ID)
        {
            V_MD_SR_ALL_BLL bll = new V_MD_SR_ALL_BLL();
            bool operate = bll.IsOperate(SECURE_ID, Session[CHubConstValues.SessionUser].ToString());
            return operate;
        }

        public string GetMDSRHTML(List<V_MD_SR_ALL> result)
        {
            StringBuilder sb = new StringBuilder();
            if (result != null && result.Any())
            {
                foreach (var item in result)
                {
                    string CanEdit = string.Empty;
                    if (item.SR_STATUS == "COMP" || item.SR_STATUS == "CONFIRM")
                        CanEdit = "disabled=\"disabled\"";

                    sb.Append("     <tr>");
                    sb.Append("         <td style=\"width:2%;\">").Append("<input type=\"checkbox\" class=\"selectCheckbox\" data-srreqno=\"" + item.SR_REQ_NO + "\" " + CanEdit + " />").Append("</td>");
                    sb.Append("         <td style=\"width:4%;\">").Append("<input type=\"button\" class=\"btn btn-primary btn-sm btnSave\" value=\"Save\" data-srreqno=\"" + item.SR_REQ_NO + "\" " + CanEdit + " />").Append("</td>");
                    //sb.Append("         <td>").Append(item.SR_STATUS).Append("</td>");
                    GetMDSRColor(sb, item.SR_STATUS);
                    sb.Append("         <td>").Append(item.SR_REQ_NO).Append("</td>");
                    sb.Append("         <td>").Append(item.PART_NO).Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm COMPANY_CODE\" value=\"" + item.COMPANY_CODE + "\" title=\"" + item.COMPANY_CODE + "\" " + CanEdit + " />").Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm Supplier_PARTNO\" value=\"" + item.SUPPLIER_PARTNO + "\" title=\"" + item.SUPPLIER_PARTNO + "\" " + CanEdit + " />").Append("</td>");
                    string PRICE = item.PRICE.HasValue ? item.PRICE.ToString() : "";
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm PRICE\" value=\"" + PRICE + "\" title=\"" + PRICE + "\" " + CanEdit + " />").Append("</td>");
                    string MOQ = item.MOQ.HasValue ? item.MOQ.ToString() : "";
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm MOQ\" value=\"" + MOQ + "\" title=\"" + MOQ + "\" " + CanEdit + " />").Append("</td>");
                    string LOT_SIZE = item.LOT_SIZE.HasValue ? item.LOT_SIZE.ToString() : "";
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm LOT_SIZE\" value=\"" + LOT_SIZE + "\" title=\"" + LOT_SIZE + "\" " + CanEdit + " />").Append("</td>");
                    string LT = item.LT.HasValue ? item.LT.ToString() : "";
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm LT\" value=\"" + LT + "\" title=\"" + LT + "\" " + CanEdit + " />").Append("</td>");
                    sb.Append("         <td>").Append(GetIS_COMMON(item.IS_COMMON, CanEdit)).Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm SR_COMMENTS\" value=\"" + item.SR_COMMENTS + "\" title=\"" + item.SR_COMMENTS + "\" " + CanEdit + " />").Append("</td>");
                    sb.Append("         <td>").Append(item.REQ_BY).Append("</td>");
                    sb.Append("         <td>").Append(item.RECORD_DATE.HasValue ? item.RECORD_DATE.Value.ToString("yyyy-MM-dd HH:mm:ss") : "").Append("</td>");
                    sb.Append("         <td>").Append(item.UPDATED_BY).Append("</td>");
                    sb.Append("         <td>").Append(item.REQ_DATE.HasValue ? item.REQ_DATE.Value.ToString("yyyy-MM-dd HH:mm:ss") : "").Append("</td>");
                    sb.Append("         <td>").Append(item.DESCRIPTION).Append("</td>");
                    sb.Append("         <td>").Append(item.COMPANY_NAME).Append("</td>");
                    sb.Append("     </tr>");
                }
            }
            return sb.ToString();
        }

        public StringBuilder GetMDSRColor(StringBuilder sb, string SR_STATUS)
        {
            switch (SR_STATUS)
            {
                case "COMP":
                    sb.Append("         <td style=\"color:green;\">");
                    break;
                case "CONFIRM":
                    sb.Append("         <td style=\"color:blue;\">");
                    break;
                case "ONHOLD":
                    sb.Append("         <td style=\"color:red;\">");
                    break;
                case "CONFIRMING":
                    sb.Append("         <td style=\"color:orange;\">");
                    break;
                default:
                    sb.Append("         <td>");
                    break;
            }
            sb.Append(SR_STATUS).Append("</td>");

            return sb;
        }


        public string GetIS_COMMON(string IS_COMMON, string CanEdit)
        {
            StringBuilder sb = new StringBuilder();
            string[] cp = new string[] { "Y", "N", "NA", "TBD" };
            sb.Append("     <select class=\"form-control input-sm IS_COMMON\" " + CanEdit + ">");
            foreach (var item in cp)
            {
                string selected = string.Empty;
                if (item == IS_COMMON)
                    selected = "selected";

                sb.Append("     <option value=\"" + item + "\" " + selected + ">").Append(item).Append("</option>");
            }
            sb.Append("     </select>");

            return sb.ToString();
        }

        [Authorize]
        public ActionResult MDSupp()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.mdsupp.ToString(), this.Request.Url.AbsoluteUri);
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult MDSuppSearch(string COMPANY_CODE)
        {
            MDSUPP_BLL mBLL = new MDSUPP_BLL();
            try
            {
                var result = mBLL.MDSuppSearch(COMPANY_CODE);
                var mainHTML = GetMDSuppHTML(result);
                return Json(new RequestResult(mainHTML));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD MDSuppSearch", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult MDSuppSave(MD_COMPANY_SNAP item)
        {
            MDSUPP_BLL mBLL = new MDSUPP_BLL();
            try
            {
                mBLL.MDSuppSave(item);
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD MDSuppSave", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        public string GetMDSuppHTML(List<MD_COMPANY_SNAP> list)
        {
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    sb.Append(" <tr>");
                    sb.Append("     <td style=\"width:3%;\">").Append("<input type=\"button\" class=\"btn btn-primary btn-sm btnSave\" value=\"Save\" data-companycode=\"" + item.COMPANY_CODE + "\" />").Append("</td>");
                    sb.Append("     <td style=\"width:4%;\">").Append(item.COMPANY_CODE).Append("</td>");
                    sb.Append("     <td style=\"width:8%;\">").Append(item.COMPANY_NAME).Append("</td>");
                    sb.Append("     <td style=\"width:8%;\">").Append("<input type=\"text\" class=\"form-control input-sm COMPANY_NAME_CN\" value=\"" + item.COMPANY_NAME_CN + "\" title=\"" + item.COMPANY_NAME_CN + "\" />").Append("</td>");
                    sb.Append("     <td style=\"width:4%;\">").Append("<input type=\"text\" class=\"form-control input-sm PLANNER\" value=\"" + item.PLANNER + "\" title=\"" + item.PLANNER + "\" />").Append(" </td>");
                    sb.Append("     <td style=\"width:4%;\">").Append("<input type=\"text\" class=\"form-control input-sm PLANNER_CODE\" value=\"" + item.PLANNER_CODE + "\" title=\"" + item.PLANNER_CODE + "\" />").Append("</td>");
                    sb.Append("     <td style=\"width:4%;\">").Append("<input type=\"text\" class=\"form-control input-sm NOTE\" value=\"" + item.NOTE + "\" title=\"" + item.NOTE + "\" />").Append("</td>");
                    sb.Append("     <td style=\"width:3%;\">").Append("<input type=\"text\" class=\"form-control input-sm GSM_SUPPLIER_NO\" value=\"" + item.GSM_SUPPLIER_NO + "\" title=\"" + item.GSM_SUPPLIER_NO + "\" />").Append("</td>");
                    sb.Append("     <td style=\"width:4%;\">").Append("<input type=\"text\" class=\"form-control input-sm VENDOR_SITE_ID\" value=\"" + item.VENDOR_SITE_ID + "\" title=\"" + item.VENDOR_SITE_ID + "\" />").Append("</td>");
                    sb.Append("     <td style=\"width:4%;\">").Append("<input type=\"text\" class=\"form-control input-sm BPA_NO\" value=\"" + item.BPA_NO + "\" title=\"" + item.BPA_NO + "\" />").Append("</td>");
                    sb.Append("     <td style=\"width:4%;\">").Append(GetINSURANCE_CODE(item.INSURANCE_CODE)).Append("</td>");
                    sb.Append("     <td style=\"width:4%;\">").Append(GetDS_TRACK(item.DS_TRACK)).Append("</td>");
                    sb.Append("     <td style=\"width:5%;\">").Append("<input type=\"text\" class=\"form-control input-sm DS_TRACK_EML\" value=\"" + item.DS_TRACK_EML + "\" title=\"" + item.DS_TRACK_EML + "\" />").Append("</td>");
                    sb.Append("     <td style=\"width:4%;\">").Append("<input type=\"text\" class=\"form-control input-sm COMPANY_NAME_SHORT\" value=\"" + item.COMPANY_NAME_SHORT + "\" title=\"" + item.COMPANY_NAME_SHORT + "\" />").Append("</td>");
                    var RETURN_ALLOW_DAYS = item.RETURN_ALLOW_DAYS == 0 ? "" : item.RETURN_ALLOW_DAYS.ToString();
                    sb.Append("     <td style=\"width:5%;\">").Append("<input type=\"text\" class=\"form-control input-sm RETURN_ALLOW_DAYS\" value=\"" + RETURN_ALLOW_DAYS + "\" title=\"" + RETURN_ALLOW_DAYS + "\" />").Append("</td>");
                    sb.Append(" </tr>");
                }
            }

            return sb.ToString();
        }

        public string GetINSURANCE_CODE(string INSURANCE_CODE)
        {
            StringBuilder sb = new StringBuilder();
            MDSUPP_BLL mBLL = new MDSUPP_BLL();
            try
            {
                var codes = mBLL.GetINSURANCE_CODE();
                sb.Append(" <select class=\"form-control input-sm INSURANCE_CODE\">");
                sb.Append("     <option value=\"\"></option>");
                if (codes != null && codes.Count() > 0)
                {
                    foreach (var item in codes)
                    {
                        if (item.INSURANCE_CODE == INSURANCE_CODE)
                            sb.Append("     <option value=\"" + item.INSURANCE_CODE + "\" selected>").Append(item.INSURANCE_CODE).Append("</option>");
                        else
                            sb.Append("     <option value=\"" + item.INSURANCE_CODE + "\">").Append(item.INSURANCE_CODE).Append("</option>");
                    }
                }
                sb.Append(" </select>");
            }
            catch (Exception ex)
            {
            }
            return sb.ToString();
        }

        public string GetDS_TRACK(string DS_TRACK)
        {
            StringBuilder sb = new StringBuilder();
            string[] track = new string[] { "", "Y", "N" };
            sb.Append(" <select class=\"form-control input-sm DS_TRACK\">");
            foreach (var item in track)
            {
                if (item == DS_TRACK)
                    sb.Append("     <option value=\"" + item + "\" selected>").Append(item).Append("</option>");
                else
                    sb.Append("     <option value=\"" + item + "\">").Append(item).Append("</option>");
            }
            sb.Append("</select>");
            return sb.ToString();
        }

    }
}