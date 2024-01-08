using CK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Globalization;

namespace CK.Controllers
{
    public class Test : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        /*
        [HttpGet]
        public IActionResult SalesReport(string startDate, string endDate, int? Store, int? Department, int dis)
        {
            DataCenterContext db = new DataCenterContext();
            db.Database.SetCommandTimeout(1800); // Set the timeout in seconds

            ViewBag.VBStore = new SelectList(db.Stores, "StoreId", "Location");

            ViewBag.VBDepartment = db.Departments.Select(m => new { m.Id, m.Name }).Distinct();

            //ViewBag.VBDepartment = new SelectList(db.Departments,"Id", "Name").Distinct(); //Be aware with Captial and small char if write ID will cause null value


            IQueryable<RptSale> RptSales = db.RptSales;

            if (!string.IsNullOrEmpty(startDate))
            {
                RptSales = RptSales.Where(s => s.TransDate.HasValue && s.TransDate >= Convert.ToDateTime(startDate, new CultureInfo("en-GB")));
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                RptSales = RptSales.Where(s => s.TransDate.HasValue && s.TransDate <= Convert.ToDateTime(endDate, new CultureInfo("en-GB")));
            }
            if (Store > 0)
            {
                RptSales = RptSales.Where(s => s.StoreId == Store.Value);
            }
            if (Department > 0)
            {
                RptSales = RptSales.Where(s => s.DpId == Department.Value);
            }
            if (string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate) && Store is null && Department is null)
            {
                //ViewBag.Message = string.Format("Hello {0}.\\nCurrent Date and Time: {1}", name, DateTime.Now.ToString());
                return View();
            }
            ViewBag.StartStopwatch = true;

            //DateTime date = DateTime.Now; // Replace with your DateTime value
            //double excelDateValue = date.ToOADate();
            var reportData1 = RptSales.GroupBy(d => new { d.StoreName, d.DpName, Date = d.TransDate.Value.Date })  //through this convert time is very very good -Value.Date-
    .Select(g => new
    {
        //        Total = String.Format("{0:N}", g.Sum(d => d.TotalSales))

        Total = g.Sum(d => d.TotalSales)
       ,
        StoreName = g.Key.StoreName,
        //InvoiceDate = g.First().TransDate.HasValue ? DateTime.FromOADate(g.First().TransDate.Value) : (DateTime?)null,
        InvoiceDate = g.Key.Date,
        //InvoiceDate = g.First().TransDate.HasValue ? g.First().TransDate.Value.Date : (DateTime?)null,
        //StoreName = g.First().StoreName,
        DepName = g.Key.DpName
    });//.OrderBy(x => x.InvoiceDate);
            if (dis == 1)
            {
                return View();
            }
            ViewBag.Data = reportData1;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("SalesReport");

                // Add header row
                worksheet.Cells["A1"].Value = "Invoice Date";
                worksheet.Cells["B1"].Value = "Store Name";
                worksheet.Cells["C1"].Value = "Department Name";
                worksheet.Cells["D1"].Value = "Total Sales";

                // Set header style
                using (var range = worksheet.Cells["A1:D1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Add data rows
                int row = 2;
                foreach (var item in reportData1)
                {
                    worksheet.Cells["A" + row].Value = item.InvoiceDate;
                    worksheet.Cells["B" + row].Value = item.StoreName;
                    worksheet.Cells["C" + row].Value = item.DepName;
                    worksheet.Cells["D" + row].Value = item.Total;
                    row++;
                }

                // Auto fit columns
                worksheet.Cells.AutoFitColumns();

                // Save the file
                var stream = new MemoryStream();
                package.SaveAs(stream);

                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SalesReport.xlsx");
                // return View();
            }
        }
    
        */
}
}
