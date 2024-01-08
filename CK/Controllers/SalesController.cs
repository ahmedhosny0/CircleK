using CK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using FluentAssertions; // for export to excel
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace CK.Controllers
{
   
    public class SalesController : Controller
    {
        private readonly ILogger<SalesController> _logger;
        public SalesController(ILogger<SalesController> logger)
        {

            _logger = logger;

        }

        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddMvc();

        //    //Set Session Timeout. Default is 20 minutes.
        //    services.AddSession(options =>
        //    {
        //        options.IdleTimeout = TimeSpan.FromMinutes(30);
        //    });
        //}
        DataCenterContext db = new DataCenterContext();

        [HttpGet]
        public IActionResult SalesReport(string startDate, string endDate, int? Store, int? Department, int dis, bool exportAfterClick, string[] selectedItems)
        {
            DataCenterContext db = new DataCenterContext();
            db.Database.SetCommandTimeout(1800); // Set the timeout in seconds

            ViewBag.VBStore = new SelectList(db.Stores, "StoreId", "Location");
            ViewBag.VBDepartment = db.Departments.Select(m => new { m.Id, m.Name }).Distinct();

            IQueryable<RptSale> RptSales = db.RptSales;

            if (!string.IsNullOrEmpty(startDate))
            {
                DateTime startDateTime = Convert.ToDateTime(startDate, new CultureInfo("en-GB"));
                RptSales = RptSales.Where(s => s.TransDate.HasValue && s.TransDate >= startDateTime);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                DateTime endDateTime = Convert.ToDateTime(endDate, new CultureInfo("en-GB"));
                RptSales = RptSales.Where(s => s.TransDate.HasValue && s.TransDate <= endDateTime);
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
                return View();
            }

            ViewBag.StartStopwatch = true;
            var reportData1 = RptSales.GroupBy(d => new { d.StoreName, d.DpName, Date = d.TransDate.Value.Date })
                .Select(g => new
                {
                    //Total = String.Format("{0:N}", g.Sum(d => d.TotalSales)),
                    Total = g.Sum(d => d.TotalSales),
                    StoreName = g.Key.StoreName,
                    InvoiceDate = g.Key.Date.ToString("yyyy-MM-dd"),
                    DepName = g.Key.DpName
                });
            ViewBag.Data = reportData1;
            //var reportData1 = ViewBag.Data as IEnumerable<dynamic>;
            if (exportAfterClick == false)

            {
                return View();
            }
            else
            {
                if (selectedItems != null && selectedItems.Any())
                {
                    // Filter reportData1 based on selected items
                    reportData1 = reportData1.Where(item => selectedItems.Contains($"{item.InvoiceDate}_{item.StoreName}_{item.DepName}"));
                }
                ViewBag.Data = reportData1;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

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
                }
                stopwatch.Stop();

                // Log or display the elapsed time
                Console.WriteLine($"Time taken for export: {stopwatch.ElapsedMilliseconds / 1000.0} second");

                // Return the elapsed time as JSON to the client
                return Json(new { ElapsedTime = stopwatch.ElapsedMilliseconds / 1000.0 });

            }


            // Handle the case when the checkbox is not checked
            //return Json(new { Message = "Export canceled. Checkbox not checked." });

        }
        public IActionResult ExportToExcel(bool exportAfterClick)
        {
            if (exportAfterClick)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                var reportData1 = ViewBag.Data as IEnumerable<dynamic>;

                using (var package = new ExcelPackage())
                {
                    // Your existing export logic here

                    stopwatch.Stop();

                    // Log or display the elapsed time
                    Console.WriteLine($"Time taken for export: {stopwatch.ElapsedMilliseconds} milliseconds");

                    // Return the elapsed time as JSON to the client
                    return Json(new { ElapsedTime = stopwatch.ElapsedMilliseconds });
                }
            }
            else
            {
                // Handle the case when the checkbox is not checked
                return Json(new { Message = "Export canceled. Checkbox not checked." });
            }
        
            
    }
    }
}
// .OrderBy(x => x.InvoiceDate);
//.Select(x => new
//{
//    x.DepName,
//    Total = String.Format("{0:N}", x.Sum(d => d.Total)),,
//    x.StoreName,
//    x.InvoiceDate
//});
//Total = g.Sum(d => d.TotalSales),
//d.new_date.ToString("dd/MM/yyyy")
//InvoiceDate = g.First().TransDate.ToString(),
//InvoiceDate = Convert.ToDateTime(g.First().TransDate)
//string constr1 = @"Data Source = 192.168.1.40;User ID=sa;Password=P@ssw0rd;Database=Test;Connect Timeout=150;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";
//string constr = @"Data Source = .;User ID=sa;Password=123456;Database=TopSoft;Connect Timeout=150;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";
//using (SqlConnection con = new SqlConnection(constr))
//{

//    using (SqlCommand cmd = new SqlCommand("select itemname,sum(hsalesquantity) total from rptsales where HSalesDate between @From and @To group by ItemName", con))
//    {
//        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
//        {
//            cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(startDate, new CultureInfo("en-GB")));
//            cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(endDate, new CultureInfo("en-GB")));
//            DataTable dt = new DataTable();
//            da.Fill(dt);
//            ViewBag.Data = dt;
//        }
//    }
//}
//, d.TransDate d.StoreName,

//          var reportData = RptSales.OrderBy(d => new { d.TransDate }).GroupBy(d => new { d.StoreName, d.DpName,d.TransDate }) //, d.TransDate d.StoreName,
//.Select(
//    g => new
//    {
//        Total = g.Sum(s => s.TotalSales),
//        //d.new_date.ToString("dd/MM/yyyy")
//        InvoiceDate = g.First().TransDate.ToString(),
//        //InvoiceDate = Convert.ToDateTime(g.First().TransDate),
//        StoreName = g.First().StoreName,
//        DepName = g.First().DpName
//    });