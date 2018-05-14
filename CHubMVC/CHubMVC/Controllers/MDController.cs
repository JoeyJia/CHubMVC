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
        public ActionResult MDReqInqSearch(string MD_REQ_NO, string PART_NO, string REQ_DATE, string REQ_BY)
        {
            V_MD_REQ_ALL1ONE_BLL mraBLL = new V_MD_REQ_ALL1ONE_BLL();
            string mainHTML = string.Empty;
            try
            {
                var result = mraBLL.MDReqInqSearch(MD_REQ_NO, PART_NO, REQ_DATE, REQ_BY);
                mainHTML = GetMDReqInqHTML(result);
                return Json(new RequestResult(mainHTML));
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
                    return Json(new RequestResult(result.First().DESCRIPTION));
                else
                    return Json(new RequestResult(""));
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
                    mrd.PRODUCT_GROUP_ID = arg.PRODUCT_GROUP_ID;
                    mrd.COMM_PART = arg.COMM_PART;
                    mrd.GLOBAL_PARTNO = arg.GLOBAL_PARTNO;
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
                    sb.Append("     <tr>");
                    sb.Append("         <td>").Append("<input type=\"checkbox\" class=\"selectCheck\" data-mdreqno=\"" + item.MD_REQ_NO + "\" data-reqlineno=\"" + item.REQ_LINE_NO + "\" />").Append("</td>");
                    sb.Append("         <td>").Append(item.REQ_STATUS).Append("</td>");
                    sb.Append("         <td>").Append(item.MD_REQ_NO).Append("</td>");
                    sb.Append("         <td>").Append(item.REQ_LINE_NO).Append("</td>");
                    sb.Append("         <td>").Append(item.PART_NO).Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control PART_DESC\" value=\"" + item.PART_DESC + "\" title=\"" + item.PART_DESC + "\" />").Append("</td>");
                    sb.Append("         <td>").Append(item.APP_STATUS).Append("</td>");
                    sb.Append("         <td>").Append(item.APP_COMMENTS).Append("</td>");
                    sb.Append("         <td>").Append(GetProductGroupID(item.PRODUCT_GROUP_ID)).Append("</td>");
                    sb.Append("         <td>").Append(GetCommPart(item.COMM_PART)).Append("</td>");
                    sb.Append("         <td>").Append(item.CHECK_EXIST == "OK" ? "<span style=\"color:green\">" + item.CHECK_EXIST + "</span>" : "<span style=\"color:red\">" + item.CHECK_EXIST + "</span>").Append("</td>");
                    sb.Append("         <td>").Append(item.CHECK_PRI_SUP == "OK" ? "<span style=\"color:green\">" + item.CHECK_PRI_SUP + "</span>" : "<span style=\"color:red\">" + item.CHECK_PRI_SUP + "</span>").Append("</td>");
                    sb.Append("         <td>").Append(item.CHECK_PRI_PB == "OK" ? "<span style=\"color:green\">" + item.CHECK_PRI_PB + "</span>" : "<span style=\"color:red\">" + item.CHECK_PRI_PB + "</span>").Append("</td>");
                    sb.Append("         <td>").Append(item.CHECK_PRI_BPA == "OK" ? "<span style=\"color:green\">" + item.CHECK_PRI_BPA + "</span>" : "<span style=\"color:red\">" + item.CHECK_PRI_BPA + "</span>").Append("</td>");
                    sb.Append("         <td>").Append(item.CHECK_COST == "OK" ? "<span style=\"color:green\">" + item.CHECK_COST + "</span>" : "<span style=\"color:red\">" + item.CHECK_COST + "</span>").Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control GLOBAL_PARTNO\" value=\"" + item.GLOBAL_PARTNO + "\" title=\"" + item.GLOBAL_PARTNO + "\" />").Append("</td>");
                    sb.Append("         <td>").Append(item.GLOBAL_PART_DESC).Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"button\" class=\"btn btn-primary btn-sm btnSave\" data-mdreqno=\"" + item.MD_REQ_NO + "\" data-reqlineno=\"" + item.REQ_LINE_NO + "\" value=\"Save\" />").Append("</td>");
                    sb.Append("     </tr>");
                }
            }


            return sb.ToString();
        }

        public string GetProductGroupID(string PRODUCT_GROUP_ID)
        {
            var result = new V_MD_REQ_ALL1ONE_BLL().GetProductGroupID();
            StringBuilder sb = new StringBuilder();
            sb.Append("     <select class=\"form-control input-sm PRODUCT_GROUP_ID\">");
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

        public string GetCommPart(string COMM_PART)
        {
            StringBuilder sb = new StringBuilder();
            string[] cp = new string[] { "", "Y", "N" };
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

                string[] pdList = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                pdList = pdList.Where(s => !string.IsNullOrEmpty(s)).ToArray();

                string partno = string.Empty; string description = string.Empty;

                foreach (var pd in pdList)
                {
                    MDReqArg mr = new MDReqArg();
                    lineno++;
                    if (pd.IndexOf("\t") > 0)
                    {
                        partno = pd.Split('\t')[0];
                        description = pd.Split('\t')[1];
                    }
                    else
                        partno = pd;

                    mr.REQ_LINE_NO = lineno;
                    mr.PART_NO = mrBLL.GetPART_NO(partno);
                    mr.PART_DESC = !string.IsNullOrEmpty(description) ? description : mrBLL.GetPART_DESC(partno);
                    mr.CHECK_EXIST = mrBLL.GetCHECK_EXIST(partno);
                    mr.GLOBAL_PARTNO = mrBLL.GetGLOBAL_PARTNO(partno);
                    mr.GLOBAL_PARTDESC = mrBLL.GetGLOBAL_PARTDESC(mr.GLOBAL_PARTNO);
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
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm txtPART_NO\" value=\"" + item.PART_NO + "\" title=\"" + item.PART_NO + "\" />").Append("</td>");
                    sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm txtPART_DESC\" value=\"" + item.PART_DESC + "\" title=\"" + item.PART_DESC + "\" />").Append("</td>");
                    sb.Append("         <td>").Append(item.CHECK_EXIST == "OK" ? "<span style=\"color:green;\">" + item.CHECK_EXIST + "</span>" : "<span style=\"color:red;\">" + item.CHECK_EXIST + "</span>").Append("</td>");
                    sb.Append("         <td>").Append(item.GLOBAL_PARTNO).Append("</td>");
                    sb.Append("         <td>").Append(item.GLOBAL_PARTDESC).Append("</td>");
                    sb.Append("         <td>").Append(item.CHECK_PRI_SUP == "OK" ? "<span style=\"color:green;\">" + item.CHECK_PRI_SUP + "</span>" : "<span style=\"color:red;\">" + item.CHECK_PRI_SUP + "</span>").Append("</td>");
                    sb.Append("         <td>").Append(item.CHECK_PRI_PB == "OK" ? "<span style=\"color:green;\">" + item.CHECK_PRI_PB + "</span>" : "<span style=\"color:red;\">" + item.CHECK_PRI_PB + "</span>").Append("</td>");
                    sb.Append("         <td>").Append(item.CHECK_PRI_BPA == "OK" ? "<span style=\"color:green;\">" + item.CHECK_PRI_BPA + "</span>" : "<span style=\"color:red;\">" + item.CHECK_PRI_BPA + "</span>").Append("</td>");
                    sb.Append("         <td>").Append(item.CHECK_COST == "OK" ? "<span style=\"color:green;\">" + item.CHECK_COST + "</span>" : "<span style=\"color:red;\">" + item.CHECK_COST + "</span>").Append("</td>");
                    sb.Append("         <td>").Append(GetProductGroupID(item.PRODUCT_GROUP_ID)).Append("</td>");
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
            sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm txtPART_NO\" value=\"\" title=\"\" />").Append("</td>");
            sb.Append("         <td>").Append("<input type=\"text\" class=\"form-control input-sm txtPART_DESC\" value=\"\" title=\"\" />").Append("</td>");
            sb.Append("         <td>").Append("<span></span>").Append("</td>");
            sb.Append("         <td>").Append("").Append("</td>");
            sb.Append("         <td>").Append("").Append("</td>");
            sb.Append("         <td>").Append("<span></span>").Append("</td>");
            sb.Append("         <td>").Append("<span></span>").Append("</td>");
            sb.Append("         <td>").Append("<span></span>").Append("</td>");
            sb.Append("         <td>").Append("<span></span>").Append("</td>");
            sb.Append("         <td>").Append(GetProductGroupID("")).Append("</td>");
            sb.Append("         <td>").Append("<input type=\"button\" class=\"btn btn-primary btn-sm btnDelete\" value=\"delete\" />").Append("</td>");
            sb.Append("     </tr>");
            return sb.ToString();
        }

        /// <summary>
        /// 获取系统号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRequestNo(List<MDReqArg> MDReqList)
        {
            bool check = true;
            foreach (var item in MDReqList)
            {
                if (string.IsNullOrEmpty(item.PART_NO) || string.IsNullOrEmpty(item.PART_DESC) || string.IsNullOrEmpty(item.PRODUCT_GROUP_ID))
                {
                    check = false;
                    break;
                }
            }
            if (!check)
                return Json(new RequestResult(false, "Part NO or DESCRIPTION or PROD GROUP cannot be null"));

            MD_REQ_BLL mrBLL = new MD_REQ_BLL();
            try
            {
                var result = mrBLL.GetRequestNo();
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD GetRequestNo", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult MDReqSave(string MD_REQ_NO, string REQ_DESC, List<MDReqArg> MDReqList)
        {
            MD_REQ_BLL mrBLL = new MD_REQ_BLL();
            MD_REQ_HEADER mrHeader = new MD_REQ_HEADER();
            MD_REQ_DETAIL mrDetail = new MD_REQ_DETAIL();

            try
            {
                //表MD_REQ_HEADER 保存
                mrHeader.MD_REQ_NO = Convert.ToDecimal(MD_REQ_NO);
                mrHeader.REQ_DESC = REQ_DESC;
                mrHeader.REQ_BY = Session[CHubConstValues.SessionUser].ToString();
                mrBLL.SaveMD_REQ_HEADER(mrHeader);

                //表MD_REQ_DETAIL 保存
                foreach (var item in MDReqList)
                {
                    mrDetail.MD_REQ_NO = Convert.ToDecimal(MD_REQ_NO);
                    mrDetail.REQ_LINE_NO = item.REQ_LINE_NO;
                    mrDetail.PART_NO = item.PART_NO;
                    mrDetail.PART_DESC = item.PART_DESC;
                    mrDetail.CHECK_EXIST = item.CHECK_EXIST;
                    mrDetail.CHECK_PRI_SUP = item.CHECK_PRI_SUP;
                    mrDetail.CHECK_PRI_PB = item.CHECK_PRI_PB;
                    mrDetail.CHECK_PRI_BPA = item.CHECK_PRI_BPA;
                    mrDetail.CHECK_COST = item.CHECK_COST;
                    mrDetail.GLOBAL_PARTNO = item.GLOBAL_PARTNO;
                    mrDetail.PRODUCT_GROUP_ID = item.PRODUCT_GROUP_ID;
                    mrBLL.SaveMD_REQ_DETAIL(mrDetail);
                }
                return Json(new RequestResult(true));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("MD MDReqSave", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

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
    }
}