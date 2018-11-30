using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CHubCommon;
using CHubDBEntity.UnmanagedModel;
using CHubModel;
using CHubBLL;
using System.Text;
using System.Data;
using System.IO;

namespace CHubMVC.Controllers
{
    public class DLController : BaseController
    {
        public ActionResult IHubLoad()
        {
            string appUser = Session[CHubConstValues.SessionUser].ToString();
            APP_RECENT_PAGES_BLL rpBLL = new APP_RECENT_PAGES_BLL();
            rpBLL.Add(appUser, CHubEnum.PageNameEnum.ihubload.ToString(), this.Request.Url.AbsoluteUri);
            ViewBag.AppUser = appUser;
            return View();
        }


        [Authorize]
        [HttpPost]
        public ActionResult GetLOAD_TYPE(string appUser)
        {
            DL_BLL dBLL = new DL_BLL();
            try
            {
                var result = dBLL.GetLOAD_TYPEs(appUser);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("DL GetLOAD_TYPE", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetLOAD_DESC(string LOAD_TYPE)
        {
            DL_BLL dBLL = new DL_BLL();
            try
            {
                var result = dBLL.GetLOAD_DESC(LOAD_TYPE);
                return Json(new RequestResult(result));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("DL GetLOAD_DESC", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult IHubDownload(string LOAD_TYPE)
        {
            DL_BLL dBLL = new DL_BLL();
            try
            {
                IHUB_LOAD_TYPE ilt = dBLL.GetIHUB_LOAD_TYPE(LOAD_TYPE);
                //string folderPath = Server.MapPath(CHubConstValues.ChubTempFolder);
                //FileInfo folder = new FileInfo(folderPath);
                //if (!Directory.Exists(folder.FullName))
                //    Directory.CreateDirectory(folder.FullName);

                string fullHref = string.Empty;
                string fileName = ilt.LOAD_TEMPLATE;
                //string fileFullName = folder.FullName + ilt.LOAD_TEMPLATE;
                if (ilt.LOAD_FMT == "XLS")
                    fullHref = @"/dl/DownLoad?fileName=" + fileName;
                else
                    fullHref = @"/dl/DownloadTXT?fileName=" + fileName;
                return Json(new RequestResult(fullHref));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("DL IHubDownload", ex);
                return Json(new RequestResult(false, ex.Message));
            }
        }



        [Authorize]
        [HttpPost]
        public ActionResult IHubUpload(string LOAD_TYPE)
        {
            if (!string.IsNullOrEmpty(LOAD_TYPE))
            {
                HttpPostedFileBase hpf = Request.Files["ihubloadInput"];//xls,xlsx;csv
                DL_BLL dBLL = new DL_BLL();
                IHUB_LOAD_TYPE ilt = new IHUB_LOAD_TYPE();
                try
                {
                    ilt = dBLL.GetIHUB_LOAD_TYPE(LOAD_TYPE);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                bool hasTitle = true;

                string fileName = hpf.FileName;
                string extension = Path.GetExtension(fileName);//后缀名

                //导入的时候要匹配当前LOAD_TYPE的LOAD_FMT文件后缀格式
                string load_fmt = ilt.LOAD_FMT;
                if (!extension.Contains(load_fmt.ToLower()))
                    return Json(new RequestResult(false, "Format does not match"));

                decimal first_row = ilt.FIRST_ROW;//从第几行开始导入
                string delimiter = ilt.DELIMITER;//csv文件用，分隔符

                //获取集合
                List<IHUB_LOAD_BASE> ilbs = GetLists(hpf, extension, first_row, delimiter,ref hasTitle);

                string LOAD_BATCH = dBLL.GetLOAD_BATCH();
                string LOAD_BY = Session[CHubConstValues.SessionUser].ToString();
                int Count = 0;
                bool load = true;

                //新增
                if (ilbs != null && ilbs.Count > 0)
                {
                    int firstStart;
                    decimal lineNo = first_row - 1;
                    if (extension == ".xls" || extension == ".xlsx")
                    {
                        if (hasTitle)
                            firstStart = (int)(first_row - 2);
                        else
                            firstStart = (int)(first_row - 1);
                    }
                    else
                        firstStart = (int)(first_row - 1);

                    for (int i = firstStart; i < ilbs.Count; i++)
                    {
                        Count++;
                        lineNo++;
                        string LOAD_LINE_NO = lineNo.ToString();
                        try
                        {
                            dBLL.AddIHUB_LOAD_BASE(ilbs[i], LOAD_BATCH, LOAD_TYPE, LOAD_BY, LOAD_LINE_NO);
                        }
                        catch (Exception ex)
                        {
                            load = false;
                            break;
                        }
                    }
                }
                else
                    return Json(new RequestResult(false, "No data"));

                if (!load)
                    return Json(new RequestResult(false, "fail to load"));

                //执行存过，监测
                try
                {
                    //dBLL.ExecP_IHUB_LOAD_POST(113, LOAD_TYPE);
                    dBLL.ExecP_IHUB_LOAD_POST(Convert.ToDecimal(LOAD_BATCH), LOAD_TYPE);
                }
                catch (Exception ex)
                {
                    return Json(new RequestResult(false, ex.Message));
                }
                return Json(new RequestResult(true, "成功导入数据:" + Count + "行"));
            }
            else
                return Json(new RequestResult(false, "No LOAD_TYPE select"));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hpf">上传文件</param>
        /// <param name="extension">后缀名</param>
        /// <param name="first_row">从第几行开始导入</param>
        /// <param name="delimiter">csv文件用，分隔符</param>
        /// <returns></returns>
        public List<IHUB_LOAD_BASE> GetLists(HttpPostedFileBase hpf, string extension, decimal first_row, string delimiter,ref bool hasTitle)
        {
            List<IHUB_LOAD_BASE> ilbs = new List<IHUB_LOAD_BASE>();

            string tempGuid = Guid.NewGuid().ToString();
            string folderPath = Server.MapPath(CHubConstValues.ChubTempFolder);
            FileInfo folder = new FileInfo(folderPath);
            if (!Directory.Exists(folder.FullName))
                Directory.CreateDirectory(folder.FullName);

            string fileFullName = string.Empty;
            if (extension == ".xls" || extension == ".xlsx")
                fileFullName = folder.FullName + tempGuid + ".xlsx";//excel
            else
                fileFullName = folder.FullName + tempGuid + ".csv";//csv
            hpf.SaveAs(fileFullName);

            try
            {
                if (extension == ".xls" || extension == ".xlsx")//excel
                {
                    ilbs = EXCELToList(fileFullName,first_row,ref hasTitle);
                    //NPOIExcelHelper npoiHelper = new NPOIExcelHelper(fileFullName);
                    //DataTable dt = npoiHelper.ExcelToDataTable();
                    //System.IO.File.Delete(fileFullName);
                    //if (dt == null && dt.Rows.Count == 0)
                    //    return ilbs;

                    //ilbs = ClassConvert.DataTableToList<IHUB_LOAD_BASE>(dt);
                    //if (ilbs == null || ilbs.Count == 0)
                    //    return ilbs;
                }
                else//csv
                {
                    ilbs = CSVToList(fileFullName, delimiter);
                }
                System.IO.File.Delete(fileFullName);
            }
            catch (Exception ex)
            {
                return ilbs;
            }


            return ilbs;
        }


        public List<IHUB_LOAD_BASE> EXCELToList(string fileName,decimal first_row,ref bool hasTitle)
        {
            List<IHUB_LOAD_BASE> ilbs = new List<IHUB_LOAD_BASE>();

            DataTable dt = new DataTable();
            NPOIExcelHelper npoiHelper = new NPOIExcelHelper(fileName);
            if (first_row == 1)
                dt = npoiHelper.ExcelToDataTable(false);
            else
                dt = npoiHelper.ExcelToDataTable();

            string title = dt.Columns[0].Caption;
            if (title == "0")
                hasTitle = false;

            if (dt == null && dt.Rows.Count == 0)
                return ilbs;
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                List<string> datas = new List<string>();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    datas.Add(dt.Rows[i][j].ToString());
                }

                if (datas.Count > 24)
                    datas = datas.Take(24).ToList();

                if (datas.Count < 24)
                {
                    for (int a = datas.Count; a < 24; a++)
                    {
                        datas.Add("");
                    }
                }
                IHUB_LOAD_BASE ilb = new IHUB_LOAD_BASE()
                {
                    T01 = datas[0],
                    T02 = datas[1],
                    T03 = datas[2],
                    T04 = datas[3],
                    T05 = datas[4],
                    T06 = datas[5],
                    T07 = datas[6],
                    T08 = datas[7],
                    T09 = datas[8],
                    T10 = datas[9],
                    T11 = datas[10],
                    T12 = datas[11],
                    T13 = datas[12],
                    T14 = datas[13],
                    T15 = datas[14],
                    T16 = datas[15],
                    T17 = datas[16],
                    T18 = datas[17],
                    T19 = datas[18],
                    T20 = datas[19],
                    T21 = datas[20],
                    T22 = datas[21],
                    T23 = datas[22],
                    T24 = datas[23]
                };
                ilbs.Add(ilb);
            }
            return ilbs;
        }



        /// <summary>
        /// CSV To List
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public List<IHUB_LOAD_BASE> CSVToList(string fileName, string delimiter)
        {
            List<IHUB_LOAD_BASE> ilbs = new List<IHUB_LOAD_BASE>();

            string str = string.Empty;
            //FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read);
            //StreamReader sr = new StreamReader(fs, Encoding.Default);

            using (StreamReader sr = new StreamReader(fileName, Encoding.Default))
            {
                while ((str = sr.ReadLine()) != null)
                {
                    List<string> data = new List<string>();
                    List<string> strs = str.Split(delimiter.ToCharArray()).ToList();

                    var count = strs.Count;
                    if (count > 24)
                        strs = strs.Take(24).ToList();

                    for (int i = 0; i < strs.Count; i++)
                    {
                        data.Add(strs[i]);
                    }
                    if (count < 24)
                    {
                        for (int i = count; i < 24; i++)
                        {
                            data.Add("");
                        }
                    }
                    #region
                    IHUB_LOAD_BASE item = new IHUB_LOAD_BASE()
                    {
                        T01 = data[0],
                        T02 = data[1],
                        T03 = data[2],
                        T04 = data[3],
                        T05 = data[4],
                        T06 = data[5],
                        T07 = data[6],
                        T08 = data[7],
                        T09 = data[8],
                        T10 = data[9],
                        T11 = data[10],
                        T12 = data[11],
                        T13 = data[12],
                        T14 = data[13],
                        T15 = data[14],
                        T16 = data[15],
                        T17 = data[16],
                        T18 = data[17],
                        T19 = data[18],
                        T20 = data[19],
                        T21 = data[20],
                        T22 = data[21],
                        T23 = data[22],
                        T24 = data[23]
                    };
                    #endregion
                    ilbs.Add(item);
                }
            }
            
            return ilbs;
        }




        public void DownloadTXT(string fileName)
        {
            string folderPath = Server.MapPath(CHubConstValues.ChubTemplateFolder);
            FileInfo folder = new FileInfo(folderPath);
            if (!Directory.Exists(folder.FullName))
                Directory.CreateDirectory(folder.FullName);
            string fullPath = folder.FullName + fileName;

            FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.ContentType = "application/octet-stream";
            //通知浏览器下载文件而不是打开
            Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(Path.GetFileName(fullPath), System.Text.Encoding.UTF8));
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

        public ActionResult DownLoad(string fileName)
        {
            string folderPath = Server.MapPath(CHubConstValues.ChubTemplateFolder);
            FileInfo folder = new FileInfo(folderPath);
            if (!Directory.Exists(folder.FullName))
                Directory.CreateDirectory(folder.FullName);

            string fullPath = folder.FullName + fileName;

            return File(fullPath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(fullPath));
        }

    }
}