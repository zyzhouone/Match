using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Model;
using Utls;
using BLL;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

namespace Web.Areas.Main.Controllers
{
    public class OrderController : BaseController
    {
        //
        // GET: /Main/Order/

        //public ActionResult Index(string teamname, string mobile,string optStatus, int? pageIndex)
        //{

        //    var orders = new List<tblordersView>();
        //    try
        //    {
        //        orders = new OrderBll().GetOrders(teamname, mobile, optStatus,pageIndex.GetValueOrDefault(1));
        //        List<SelectListItem> Status = new MemberBll().GetDict(7);
        //        ViewData["Status"] = Status;


        //        foreach (SelectListItem r in Status)
        //        {
        //            if (optStatus == r.Value)
        //                ViewBag.optStatus += "<option value='" + r.Value.ToString() + "'selected>" + r.Text.ToString() + "</option>";
        //            else
        //                ViewBag.optStatus += "<option value='" + r.Value.ToString() + "'>" + r.Text.ToString() + "</option>";
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //    return View(orders);
        //}

        //[HttpPost]
        //public string ExpToExcel(string teamname, string mobile, string status)
        //{
        //    List<tblordersView> o = new OrderBll().ExportOrders(teamname, mobile, status);
        //    string ExportField = "Orderid,Matchname,Teamname,Mobile,Linename,Ordertotal,Paytime,Status";
        //    string[] fields = ExportField.Split(',');
        //    DataTable dt = new DataTable();
        //    List<SelectListItem> Status = new MemberBll().GetDict(7);

        //    for (int j = 0; j <= fields.Length - 1; j++)
        //    {
        //        dt.Columns.Add(fields[j]);
        //    }

        //    for (int i = 0; i < o.Count; i++)
        //    {

        //        DataRow dr = dt.NewRow();
        //        for (int j = 0; j <= fields.Length - 1; j++)
        //        {
        //            switch (fields[j])
        //            {
        //                case "Orderid":
        //                    dr[fields[j]] = o[i].Orderid;
        //                    break;
        //                case "Matchname":
        //                    dr[fields[j]] = o[i].Matchname;
        //                    break;
        //                case "Teamname":
        //                    dr[fields[j]] = o[i].Teamname;
        //                    break;
        //                case "Mobile":
        //                    dr[fields[j]] = o[i].Mobile;
        //                    break;
        //                case "Linename":
        //                    dr[fields[j]] = o[i].Linename;
        //                    break;
        //                case "Ordertotal":
        //                    dr[fields[j]] = o[i].Ordertotal;
        //                    break;
        //                case "Paytime":
        //                    dr[fields[j]] = o[i].Paytime;
        //                    break;
        //                case "Status":
        //                    foreach (SelectListItem r in Status)
        //                    {
        //                        if (o[i].Status.ToString() == r.Value)
        //                            dr[fields[j]] = r.Text.ToString();
        //                    }
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //        dt.Rows.Add(dr);
        //    }

        //    dt.Columns["Orderid"].ColumnName = "订单号";
        //    dt.Columns["Matchname"].ColumnName = "赛事名称";
        //    dt.Columns["Teamname"].ColumnName = "队伍名称";
        //    dt.Columns["Mobile"].ColumnName = "联系电话";
        //    dt.Columns["Linename"].ColumnName = "线路名称";
        //    dt.Columns["Ordertotal"].ColumnName = "订单金额";
        //    dt.Columns["Paytime"].ColumnName = "支付时间";
        //    dt.Columns["Status"].ColumnName = "状态";


        //    string file = "";
        //    try
        //    {
        //        file = GridToExcelByNPOI(dt, DateTime.Now.ToString("yyyyMMddHHmmss") + "_订单统计.xls");
        //    }
        //    catch
        //    {
        //        file = "";
        //    }

        //    return file;
        //}

        //private static string GridToExcelByNPOI(DataTable dt, string strExcelFileName)
        //{
        //    try
        //    {
        //        HSSFWorkbook workbook = new HSSFWorkbook();
        //        ISheet sheet = workbook.CreateSheet("Sheet1");

        //        ICellStyle HeadercellStyle = workbook.CreateCellStyle();
        //        HeadercellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
        //        HeadercellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
        //        HeadercellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
        //        HeadercellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
        //        HeadercellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
        //        //字体
        //        NPOI.SS.UserModel.IFont headerfont = workbook.CreateFont();
        //        headerfont.Boldweight = (short)FontBoldWeight.BOLD;
        //        HeadercellStyle.SetFont(headerfont);


        //        //用column name 作为列名
        //        int icolIndex = 0;
        //        IRow headerRow = sheet.CreateRow(0);
        //        foreach (DataColumn item in dt.Columns)
        //        {
        //            ICell cell = headerRow.CreateCell(icolIndex);
        //            cell.SetCellValue(item.ColumnName);
        //            cell.CellStyle = HeadercellStyle;
        //            icolIndex++;
        //        }

        //        ICellStyle cellStyle = workbook.CreateCellStyle();

        //        //为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
        //        cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
        //        cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
        //        cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
        //        cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
        //        cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;


        //        NPOI.SS.UserModel.IFont cellfont = workbook.CreateFont();
        //        cellfont.Boldweight = (short)FontBoldWeight.NORMAL;
        //        cellStyle.SetFont(cellfont);

        //        //建立内容行
        //        int iRowIndex = 1;
        //        int iCellIndex = 0;
        //        foreach (DataRow Rowitem in dt.Rows)
        //        {
        //            IRow DataRow = sheet.CreateRow(iRowIndex);
        //            foreach (DataColumn Colitem in dt.Columns)
        //            {

        //                ICell cell = DataRow.CreateCell(iCellIndex);
        //                cell.SetCellValue(Rowitem[Colitem].ToString());
        //                cell.CellStyle = cellStyle;
        //                iCellIndex++;
        //            }
        //            iCellIndex = 0;
        //            iRowIndex++;
        //        }

        //        //自适应列宽度
        //        for (int i = 0; i < icolIndex; i++)
        //        {
        //            sheet.AutoSizeColumn(i);
        //        }

        //        //写Excel
        //        string filepath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\Data\\" + strExcelFileName;
        //        FileStream file = new FileStream(filepath, FileMode.OpenOrCreate);
        //        workbook.Write(file);
        //        file.Flush();
        //        file.Close();
        //        return strExcelFileName;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //    }

        //}

    }
}
