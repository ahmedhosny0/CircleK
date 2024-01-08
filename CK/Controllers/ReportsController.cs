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
using DocumentFormat.OpenXml.Wordprocessing;

namespace CK.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ILogger<ReportsController> _logger;
        public ReportsController(ILogger<ReportsController> logger)
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
        public IActionResult SalesReport(string startDate, string endDate, int? Store, int? Department, bool exportAfterClick, string[] selectedItems,
            bool VPerDay, bool VPerMonYear, bool VPerMon, bool VPerYear, bool VQty, bool VPrice, bool VStoreName, int? monthToFilter)
        {
            DataCenterContext db = new DataCenterContext();
            db.Database.SetCommandTimeout(600); // Set the timeout in seconds
            ViewBag.VBStore = db.Stores
              .GroupBy(m => m.Location)
              .Select(group => new { StoreId = group.First().StoreId, Location = group.Key })
              .OrderBy(m => m.Location)
              .ToList();

            ViewBag.VBDepartment = db.Departments
              .GroupBy(m => m.Name)
              .Select(group => new { Id = group.First().Id, Name = group.Key })
              .OrderBy(m => m.Name)
              .ToList();
            ViewBag.VBItemName = db.Items
    .Where(m => m.Description != null && !m.Description.Contains("???"))
    .GroupBy(m => m.ItemLookupCode)
    .Select(group => new
    {
        ItemLookupCode = group.Key,
        Description = group.First().Description
    })
     .OrderBy(m => m.Description)
    .Take(50)
    .ToList();
            //ViewBag.VBItemName = db.Items
            //.Where(m => m.Description != null && !m.Description.Contains("?????")).GroupBy(m => m.ItemLookupCode)
            //  .Select(group => new { Description = group.First().Description, ItemLookupCode = group.Key })
            //  .OrderBy(m => m.Description)
            //  .ToList();
            //   ViewBag.VBItemName = db.Items.Select(m => new { m.Id, m.Description });
            //  ViewBag.VBItemName = new SelectList(db.Items, "Id", "Description");
            ViewBag.VBItemBarcode = db.Items.Select(m => new { m.Id, m.ItemLookupCode }).Distinct();
            //ViewBag.VBSupplier = db.Suppliers.Select(m => new { m.Id, m.SupplierName }).Distinct();
            ViewBag.VBStoreFranchise = db.Stores
                 .Where(m => m.Franchise != null)
                 .Select(m => m.Franchise)
                 .Distinct()
                 .ToList();
            // ViewBag.VBStoreFranchise = db.Stores.Select(m => new { m.Closed, m.Franchise }).Distinct();
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
            if (monthToFilter.HasValue)
            {
                RptSales = RptSales.Where(s => s.TransDate.HasValue && s.TransDate.Value.Month == monthToFilter.Value);
            }
            if (Department > 0)
            {
                RptSales = RptSales.Where(s => s.DpId == Department.Value);
            }
            if (string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate) && Store is null && Department is null)
            {
                return View();
            }
            // Dynamic GroupBy based on selected values
            // Dynamic GroupBy based on selected values
            IQueryable<dynamic> reportData1;
            if (VPerDay == true)
            {
                if (Store != null && Department != null)
                {
                    reportData1 = RptSales
                       .GroupBy((RptSale d) => new { Date = d.TransDate.Value.Date, d.DpName, d.StoreName, d.ByMonth, d.ByYear })
                       .Select(g => new
                       {
                           Total = g.Sum(d => d.TotalSales),
                           TotalQty = g.Sum(d => d.Qty),
                           PerDay = g.Key.Date.ToString("yyyy-MM-dd"),
                           DepName = g.Key.DpName,
                           Price = g.Max(d => d.Price),
                           ByYear = g.Key.ByYear,
                           ByMonth = g.Key.ByMonth,
                           StoreName = g.Key.StoreName
                       });

                }
                else if (Store == null && Department != null)
                {
                    reportData1 = RptSales
                        .GroupBy((RptSale d) => new { d.DpName, Date = d.TransDate.Value.Date, d.ByMonth, d.ByYear })
                        .Select(g => new
                        {
                            Total = g.Sum(d => d.TotalSales),
                            TotalQty = g.Sum(d => d.Qty),
                            DepName = g.Key.DpName,
                            Price = g.Max(d => d.Price),
                            ByYear = g.Key.ByYear,
                            ByMonth = g.Key.ByMonth,
                            PerDay = g.Key.Date.ToString("yyyy-MM-dd"),
                        });
                }
                else
                {
                    reportData1 = RptSales
                        .GroupBy((RptSale d) => new { d.StoreName, Date = d.TransDate.Value.Date, d.ByMonth, d.ByYear })
                        .Select(g => new
                        {
                            Total = g.Sum(d => d.TotalSales),
                            TotalQty = g.Sum(d => d.Qty),
                            StoreName = g.Key.StoreName,
                            Price = g.Max(d => d.Price),
                            ByYear = g.Key.ByYear,
                            ByMonth = g.Key.ByMonth,
                            PerDay = g.Key.Date.ToString("yyyy-MM-dd"),
                        });
                }
            }
            else
            {
                if (Store != null && Department != null)
                {
                    reportData1 = RptSales
                       .GroupBy((RptSale d) => new { d.ByMonth, d.DpName, d.StoreName, d.ByYear })
                       .Select(g => new
                       {
                           Total = g.Sum(d => d.TotalSales),
                           TotalQty = g.Sum(d => d.Qty),
                           ByMonth = g.Key.ByMonth,
                           DepName = g.Key.DpName,
                           Price = g.Max(d => d.Price),
                           ByYear = g.Key.ByYear,
                           StoreName = g.Key.StoreName
                       });

                }
                else if (Store == null && Department != null)
                {
                    reportData1 = RptSales
                        .GroupBy((RptSale d) => new { d.DpName, d.ByMonth, d.ByYear })
                        .Select(g => new
                        {
                            Total = g.Sum(d => d.TotalSales),
                            TotalQty = g.Sum(d => d.Qty),
                            DepName = g.Key.DpName,
                            Price = g.Max(d => d.Price),
                            ByYear = g.Key.ByYear,
                            PerMonth = g.Key.ByMonth
                        });
                }
                else
                {
                    reportData1 = RptSales
                        .GroupBy((RptSale d) => new { d.StoreName, d.ByMonth, d.ByYear })
                        .Select(g => new
                        {
                            Total = g.Sum(d => d.TotalSales),
                            TotalQty = g.Sum(d => d.Qty),
                            StoreName = g.Key.StoreName,
                            Price = g.Max(d => d.Price),
                            ByYear = g.Key.ByYear,
                            PerMonth = g.Key.ByMonth
                        });
                }
            }

            ViewBag.Data = reportData1;
            //var reportData1 = ViewBag.Data as IEnumerable<dynamic>;
            if (exportAfterClick == false)

            {
                return View();
            }
            else
            {
                ViewBag.Data = reportData1;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("SalesReport");

                    // Add header row
                    int columnCount = 1; // Start with the first column (A)

                    if (VPerYear == true || VPerMonYear == true)
                        worksheet.Cells[1, columnCount++].Value = "Date Per Year";
                    if (VPerMon == true || VPerMonYear == true)
                        worksheet.Cells[1, columnCount++].Value = "Date Per Month";
                    if (VPerDay == true)
                        worksheet.Cells[1, columnCount++].Value = "Date Per Day";

                    if (Store != null)
                        worksheet.Cells[1, columnCount++].Value = "Store Name";

                    if (Department != null)
                        worksheet.Cells[1, columnCount++].Value = "Department Name";

                    if (VQty == true)
                        worksheet.Cells[1, columnCount++].Value = "Total Qty";

                    if (VPrice == true)
                        worksheet.Cells[1, columnCount++].Value = "Max Price";

                    worksheet.Cells[1, columnCount++].Value = "Total Sales";

                    // Set header style
                    using (var range = worksheet.Cells[1, 1, 1, columnCount - 1])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    }

                    // Add data rows
                    int row = 2;
                    foreach (var item in reportData1)
                    {
                        columnCount = 1; // Reset column count for each row
                        if (VPerYear == true || VPerMonYear == true)
                            worksheet.Cells[row, columnCount++].Value = item.ByYear;
                        if (VPerMon == true || VPerMonYear == true)
                            worksheet.Cells[row, columnCount++].Value = item.ByMonth;
                        if (VPerDay == true)
                            worksheet.Cells[row, columnCount++].Value = item.PerDay;
                        if (Store != null)
                            worksheet.Cells[row, columnCount++].Value = item.StoreName;

                        if (Department != null)
                            worksheet.Cells[row, columnCount++].Value = item.DepName;

                        if (VQty == true)
                            worksheet.Cells[row, columnCount++].Value = item.TotalQty;

                        if (VPrice == true)
                            worksheet.Cells[row, columnCount++].Value = item.Price;

                        worksheet.Cells[row, columnCount++].Value = item.Total;
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
//reportData1 = RptSales
//        .GroupBy((RptSale d) => new { Date = d.TransDate.Value.Date,d.DpName, d.StoreName })
//        .Select(g => new
//        {
//            Total = g.Sum(d => d.TotalSales),
//            InvoiceDate = g.Key.Date.ToString("yyyy-MM-dd"),
//            DepName = g.Key.DpName,
//            StoreName = g.Key.StoreName
//        });
//var reportData1 = RptSales.GroupBy(d => new { d.StoreName, d.DpName, Date = d.TransDate.Value.Date })
//    .Select(g => new
//    {
//        //Total = String.Format("{0:N}", g.Sum(d => d.TotalSales)),
//        Total = g.Sum(d => d.TotalSales),
//        StoreName = g.Key.StoreName,
//        InvoiceDate = g.Key.Date.ToString("yyyy-MM-dd"),
//        DepName = g.Key.DpName
//    });

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