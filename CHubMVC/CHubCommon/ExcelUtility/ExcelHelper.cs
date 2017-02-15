using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace CHubCommon
{
    public class ExcelHelper
    {
        public static System.Data.DataTable GetDTFromExcel( string fileName, bool hasTitle=true)
        {
            //OpenFileDialog openFile = new OpenFileDialog();

            //openFile.Filter = "Excel(*.xlsx)|*.xlsx|Excel(*.xls)|*.xls";

            //openFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //openFile.Multiselect = false;

            //if (openFile.ShowDialog() == DialogResult.Cancel) return null;

            //var excelFilePath = openFile.FileName;



            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();

            Sheets sheets;

            object oMissiong = System.Reflection.Missing.Value;

            Workbook workbook = null;

            System.Data.DataTable dt = new System.Data.DataTable();



            try

            {

                if (app == null) return null;

                workbook = app.Workbooks.Open(fileName, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong,

                    oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong);

                sheets = workbook.Worksheets;



                //将数据读入到DataTable中

                Worksheet worksheet = (Worksheet)sheets.get_Item(1);//读取第一张表  

                if (worksheet == null) return null;



                int iRowCount = worksheet.UsedRange.Rows.Count;

                int iColCount = worksheet.UsedRange.Columns.Count;

                //生成列头

                for (int i = 0; i < iColCount; i++)

                {

                    var name = "column" + i;

                    if (hasTitle)

                    {

                        var txt = ((Range)worksheet.Cells[1, i + 1]).Text.ToString();

                        if (!string.IsNullOrEmpty(txt)) name = txt;

                    }

                    while (dt.Columns.Contains(name)) name = name + "_1";//重复行名称会报错。

                    dt.Columns.Add(new DataColumn(name, typeof(string)));

                }

                //生成行数据

                Range range;

                int rowIdx = hasTitle ? 2 : 1;

                for (int iRow = rowIdx; iRow <= iRowCount; iRow++)

                {

                    DataRow dr = dt.NewRow();

                    for (int iCol = 1; iCol <= iColCount; iCol++)

                    {

                        range = (Range)worksheet.Cells[iRow, iCol];

                        dr[iCol - 1] = (range.Value2 == null) ? "" : range.Text.ToString();

                    }

                    dt.Rows.Add(dr);

                }

                return dt;

            }

            catch { return null; }

            finally

            {

                workbook.Close(false, oMissiong, oMissiong);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);

                workbook = null;

                app.Workbooks.Close();

                app.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(app);

                app = null;

            }
        }
    }
}
