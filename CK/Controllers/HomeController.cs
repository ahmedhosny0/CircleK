using CK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using CK.Model;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace CK.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        DataCenterContext db = new DataCenterContext();

        bool exported = false;

        [HttpGet]
        public IActionResult Index(string startDate, string endDate, int? Store, int? Department, int? Supplierlist, bool exportAfterClick, string[] selectedItems,
            bool VPerDay, bool VPerMonYear, bool VPerMon, bool VPerYear, bool VQty, bool VPrice, bool VStoreName, bool VDepartment, bool VTotalSales
           , bool VItemLookupCode, bool VItemName, bool VSupplierId, bool VSupplierName,
            int? monthToFilter, string ItemLookupCodeTxt, string ItemNameTxt, bool TMT, bool RMS, bool Yesterday)
        {
            DataCenterContext db = new DataCenterContext();
            CkhelperdbContext db3 = new CkhelperdbContext();
            db.Database.SetCommandTimeout(600); // Set the timeout in seconds
            ViewBag.VBStore = db3.Liststores
 .GroupBy(m => m.StoreName)
 .Select(group => new { StoreId = group.First().StoreId, StoreName = group.Key })
 .OrderBy(m => m.StoreName)
 .ToList();
            //ViewBag.VBStore = db3.Liststores
            //   .GroupBy(m => m.StoreName)
            //   .Select(group => new SelectListItem
            //   {
            //       Value = group.First().StoreId.ToString(), // Assuming StoreId is of type int
            //       Text = group.Key
            //   })
            //   .OrderBy(m => m.Text)
            //   .ToList();
            //var query = db3.RptStores
            //   .GroupBy(m => m.StoreNameR)
            //   .Select(group => new
            //   {
            //       StoreIdD = TMT ? (int?)group.First().StoreIdD : null,
            //       StoreIdR = RMS ? (int?)group.First().StoreIdR : null,
            //       StoreNameR = group.Key
            //   })
            //   .OrderBy(m => m.StoreNameR)
            //   .ToList();

            //ViewBag.VBStore = query;
            //       if (RMS)
            //       {
            //           ViewBag.VBStore = db3.RptStores
            //             .GroupBy(m => m.StoreNameD)
            //.Select(group => new { StoreidR = group.First().StoreIdR, StoreNameR = group.Key })
            //             .OrderBy(m => m.StoreNameR)
            //             .ToList();
            //       }

            ViewBag.VBDepartment = db.Departments
  .GroupBy(m => m.Name)
  .Select(group => new { Code = group.First().Code, Name = group.Key })
  .OrderBy(m => m.Name)
  .ToList();
            //ViewBag.VBStore = db.Stores
            //  .GroupBy(m => m.Location)
            //  .Select(group => new { StoreId = group.First().StoreId, Location = group.Key })
            //  .OrderBy(m => m.Location)
            //  .ToList();

            //ViewBag.VBDepartment = db.Departments
            //  .GroupBy(m => m.Name)
            //  .Select(group => new { Id = group.First().Id, Name = group.Key })
            //  .OrderBy(m => m.Name)
            //  .ToList();
            //        ViewBag.VBItemName = db.Items
            //.Where(m => m.Description != null && !m.Description.Contains("???"))
            //.GroupBy(m => m.ItemLookupCode)
            //.Select(group => new
            //{
            //    ItemLookupCode = group.Key,
            //    Description = group.First().Description
            //})
            // .OrderBy(m => m.Description)
            //.Take(50)
            //.ToList();
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
            IQueryable<RptSalesAxt> RptSalesAxts = db.RptSalesAxts;

            if (!string.IsNullOrEmpty(startDate))
            {
                DateTime startDateTime = Convert.ToDateTime(startDate, new CultureInfo("en-GB"));
                RptSales = RptSales.Where(s => s.TransDate.HasValue && s.TransDate >= startDateTime);
                RptSalesAxts = RptSalesAxts.Where(s => s.TransDate.HasValue && s.TransDate >= startDateTime);
                // RptSalesAxts = RptSalesAxts.Where(s => s.Transdate.HasValue && s.Transdate >= startDateTime);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                DateTime endDateTime = Convert.ToDateTime(endDate, new CultureInfo("en-GB"));
                RptSales = RptSales.Where(s => s.TransDate.HasValue && s.TransDate <= endDateTime);
                RptSalesAxts = RptSalesAxts.Where(s => s.TransDate.HasValue && s.TransDate <= endDateTime);
                // RptSalesAxts = RptSalesAxts.Where(s => s.Transdate.HasValue && s.Transdate <= endDateTime);
            }   // Declared at the class level
            DateTime currentDate = DateTime.Now;
            DateTime lastWeekStartDate = currentDate.AddDays(-7);
            DateTime lastMonthDate = currentDate.AddMonths(-1);
            DateTime firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            DateTime lastDayOfLastMonth = firstDayOfCurrentMonth.AddDays(-1);
            if (Yesterday)
            {
                //RptSales = RptSales.Where(s => s.TransDate == s.Yesterday);
                //RptSales = RptSales.Where(s => s.TransDate >= lastWeekStartDate && s.TransDate <= currentDate);
                RptSales = RptSales.Where(s => s.TransDate >= firstDayOfCurrentMonth && s.TransDate <= lastDayOfLastMonth);
                RptSalesAxts = RptSalesAxts.Where(s => s.TransDate >= firstDayOfCurrentMonth && s.TransDate <= lastDayOfLastMonth);
                // RptSales = RptSales.Where(s => s.TransDate >= lastMonthDate && s.TransDate <= lastMonthDate);

            }
            if (Store != null)
            {
                if (RMS)
                {
                    RptSales = RptSales.Where(s => s.StoreId == Store.Value);
                }
                if (TMT)
                {
                    //int selectedStoreId;
                    //if (int.TryParse(Store, out selectedStoreId))
                    //{
                    //    RptSalesAxts = RptSalesAxts.Where(s => s.StoreId == selectedStoreId).ToList();
                    //}
                     RptSalesAxts = RptSalesAxts.Where(s => s.StoreId == Store.Value.ToString());
                }
               // RptSalesAxts = RptSalesAxts.Where(s => s.Store == Store.Value.ToString());

            }
            if (monthToFilter.HasValue)
            {
                RptSales = RptSales.Where(s => s.TransDate.HasValue && s.TransDate.Value.Month == monthToFilter.Value);
            }
            if (Department > 0)
            {
                RptSales = RptSales.Where(s => s.DpId == Department.Value);
                RptSalesAxts = RptSalesAxts.Where(s => s.DpId == Department.Value.ToString());

            }
            if (string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate) && Store is null && Department is null)
            {
                return View();
            }
            // Dynamic GroupBy based on selected values
            // Dynamic GroupBy based on selected values
            IQueryable<dynamic> reportData1;
            if (TMT)
            {
                if (VPerDay == true)
                {
                    if (Store != null && Department != null)
                    {
                        reportData1 = RptSalesAxts
                       .GroupBy((RptSalesAxt d) => new { Date = d.TransDate.Value.Date, d.DpName, d.StoreName, d.ByMonth, d.ByYear })
                       .Select(g => new
                       {
                           Total = g.Sum(d => d.TotalSales),
                           TotalQty = g.Sum(d => d.Qty),
                           PerDay = g.Key.Date.ToString("yyyy-MM-dd"),
                           DepName = g.Key.DpName,
                           Price = g.Max(d => d.Price),
                           PerMonth = g.Key.ByMonth,
                           PerYear = g.Key.ByYear,
                           StoreName = g.Key.StoreName,

                       });

                    }
                    else if (Store == null && Department != null)
                    {
                        reportData1 = RptSalesAxts
                            .GroupBy((RptSalesAxt d) => new { d.DpName, Date = d.TransDate.Value.Date, d.ByMonth, d.ByYear })
                            .Select(g => new
                            {
                                Total = g.Sum(d => d.TotalSales),
                                TotalQty = g.Sum(d => d.Qty),
                                DepName = g.Key.DpName,
                                Price = g.Max(d => d.Price),
                                PerMonth = g.Key.ByMonth,
                                PerYear = g.Key.ByYear,
                                PerDay = g.Key.Date.ToString("yyyy-MM-dd"),
                            });
                    }
                    else
                    {
                        reportData1 = RptSalesAxts
                            .GroupBy((RptSalesAxt d) => new { d.StoreName, Date = d.TransDate.Value.Date, d.ByMonth, d.ByYear })
                            .Select(g => new
                            {
                                Total = g.Sum(d => d.TotalSales),
                                TotalQty = g.Sum(d => d.Qty),
                                StoreName = g.Key.StoreName,
                                Price = g.Max(d => d.Price),
                                PerMonth = g.Key.ByMonth,
                                PerYear = g.Key.ByYear,
                                PerDay = g.Key.Date.ToString("yyyy-MM-dd"),
                            });
                    }
                }
                else if (VPerMon == true)
                {
                    if (Store != null && Department != null)
                    {
                        reportData1 = RptSalesAxts
                           .GroupBy((RptSalesAxt d) => new { d.ByMonth, d.DpName, d.StoreName, d.ByYear })
                           .Select(g => new
                           {
                               Total = g.Sum(d => d.TotalSales),
                               TotalQty = g.Sum(d => d.Qty),
                               PerMonth = g.Key.ByMonth,
                               PerYear = g.Key.ByYear,
                               DepName = g.Key.DpName,
                               Price = g.Max(d => d.Price),
                               StoreName = g.Key.StoreName
                           });

                    }
                    else if (Store == null && Department != null)
                    {
                        reportData1 = RptSalesAxts
                            .GroupBy((RptSalesAxt d) => new { d.DpName, d.ByMonth, d.ByYear })
                            .Select(g => new
                            {
                                Total = g.Sum(d => d.TotalSales),
                                TotalQty = g.Sum(d => d.Qty),
                                DepName = g.Key.DpName,
                                Price = g.Max(d => d.Price),
                                PerMonth = g.Key.ByMonth,
                                PerYear = g.Key.ByYear,
                            });
                    }
                    else if (Store != null && Department == null)
                    {
                        reportData1 = RptSalesAxts
                            .GroupBy((RptSalesAxt d) => new { d.StoreName, d.ByMonth, d.ByYear })
                            .Select(g => new
                            {
                                Total = g.Sum(d => d.TotalSales),
                                TotalQty = g.Sum(d => d.Qty),
                                StoreName = g.Key.StoreName,
                                Price = g.Max(d => d.Price),
                                PerMonth = g.Key.ByMonth,
                                PerYear = g.Key.ByYear,
                            });
                    }
                    else
                    {
                        reportData1 = RptSalesAxts
                            .GroupBy((RptSalesAxt d) => new { d.ByMonth, d.ByYear })
                            .Select(g => new
                            {
                                Total = g.Sum(d => d.TotalSales),
                                TotalQty = g.Sum(d => d.Qty),
                                Price = g.Max(d => d.Price),
                                PerMonth = g.Key.ByMonth,
                                PerYear = g.Key.ByYear,
                            });
                    }
                }
                else
                {
                    if (Store != null && Department != null)
                    {
                        reportData1 = RptSalesAxts
                           .GroupBy((RptSalesAxt d) => new { d.DpName, d.StoreName })
                           .Select(g => new
                           {
                               Total = g.Sum(d => d.TotalSales),
                               TotalQty = g.Sum(d => d.Qty),
                               DepName = g.Key.DpName,
                               Price = g.Max(d => d.Price),
                               StoreName = g.Key.StoreName
                           });

                    }
                    else if (Store == null && Department != null)
                    {
                        reportData1 = RptSalesAxts
                            .GroupBy((RptSalesAxt d) => new { d.DpName })
                            .Select(g => new
                            {
                                Total = g.Sum(d => d.TotalSales),
                                TotalQty = g.Sum(d => d.Qty),
                                DepName = g.Key.DpName,
                                Price = g.Max(d => d.Price),
                            });
                    }
                    else
                    {
                        reportData1 = RptSalesAxts
                            .GroupBy((RptSalesAxt d) => new { d.StoreName })
                            .Select(g => new
                            {
                                Total = g.Sum(d => d.TotalSales),
                                TotalQty = g.Sum(d => d.Qty),
                                StoreName = g.Key.StoreName,
                                Price = g.Max(d => d.Price),
                            });
                    }
                }
            }
            else
            {
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
                               PerMonth = g.Key.ByMonth,
                               PerYear = g.Key.ByYear,
                               StoreName = g.Key.StoreName,

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
                                PerMonth = g.Key.ByMonth,
                                PerYear = g.Key.ByYear,
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
                                PerMonth = g.Key.ByMonth,
                                PerYear = g.Key.ByYear,
                                PerDay = g.Key.Date.ToString("yyyy-MM-dd"),
                            });
                    }
                }
                else if (VPerMon == true)
                {
                    if (Store != null && Department != null)
                    {
                        reportData1 = RptSales
                           .GroupBy((RptSale d) => new { d.ByMonth, d.DpName, d.StoreName, d.ByYear })
                           .Select(g => new
                           {
                               Total = g.Sum(d => d.TotalSales),
                               TotalQty = g.Sum(d => d.Qty),
                               PerMonth = g.Key.ByMonth,
                               PerYear = g.Key.ByYear,
                               DepName = g.Key.DpName,
                               Price = g.Max(d => d.Price),
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
                                PerMonth = g.Key.ByMonth,
                                PerYear = g.Key.ByYear,
                            });
                    }
                    else if (Store != null && Department == null)
                    {
                        reportData1 = RptSales
                            .GroupBy((RptSale d) => new { d.StoreName, d.ByMonth, d.ByYear })
                            .Select(g => new
                            {
                                Total = g.Sum(d => d.TotalSales),
                                TotalQty = g.Sum(d => d.Qty),
                                StoreName = g.Key.StoreName,
                                Price = g.Max(d => d.Price),
                                PerMonth = g.Key.ByMonth,
                                PerYear = g.Key.ByYear,
                            });
                    }
                    else
                    {
                        reportData1 = RptSales
                            .GroupBy((RptSale d) => new { d.ByMonth, d.ByYear })
                            .Select(g => new
                            {
                                Total = g.Sum(d => d.TotalSales),
                                TotalQty = g.Sum(d => d.Qty),
                                Price = g.Max(d => d.Price),
                                PerMonth = g.Key.ByMonth,
                                PerYear = g.Key.ByYear,
                            });
                    }
                }
                else
                {
                    if (Store != null && Department != null)
                    {
                        reportData1 = RptSales
                           .GroupBy((RptSale d) => new { d.DpName, d.StoreName })
                           .Select(g => new
                           {
                               Total = g.Sum(d => d.TotalSales),
                               TotalQty = g.Sum(d => d.Qty),
                               DepName = g.Key.DpName,
                               Price = g.Max(d => d.Price),
                               StoreName = g.Key.StoreName
                           });

                    }
                    else if (Store == null && Department != null)
                    {
                        reportData1 = RptSales
                            .GroupBy((RptSale d) => new { d.DpName })
                            .Select(g => new
                            {
                                Total = g.Sum(d => d.TotalSales),
                                TotalQty = g.Sum(d => d.Qty),
                                DepName = g.Key.DpName,
                                Price = g.Max(d => d.Price),
                            });
                    }
                    else
                    {
                        reportData1 = RptSales
                            .GroupBy((RptSale d) => new { d.StoreName })
                            .Select(g => new
                            {
                                Total = g.Sum(d => d.TotalSales),
                                TotalQty = g.Sum(d => d.Qty),
                                StoreName = g.Key.StoreName,
                                Price = g.Max(d => d.Price),
                            });
                    }
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
                            worksheet.Cells[row, columnCount++].Value = item.PerYear;
                        if (VPerMon == true || VPerMonYear == true)
                            worksheet.Cells[row, columnCount++].Value = item.PerMonth;
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



        //[HttpGet]
        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //    // Set TempData variable to indicate logout
        //    TempData["IsLoggedOut"] = true;

        //    // Clear session on logout
        //    HttpContext.Session.Clear();

        //    // Prevent caching by setting appropriate HTTP headers
        //    if (!Response.Headers.ContainsKey("Cache-Control"))
        //    {
        //        Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
        //    }

        //    return RedirectToAction("Login", "Login");
        //}
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> LogOut()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Set a TempData variable to indicate logout
            TempData["IsLoggedOut"] = true;

            // Clear session on logout
            HttpContext.Session.Clear();

            // Prevent caching by setting appropriate HTTP headers
            //Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            //Response.Headers.Add("Pragma", "no-cache");
            //Response.Headers.Add("Expires", "0");
            try
            {
                if (!Response.Headers.ContainsKey("Cache-Control"))
                {
                    Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
                }

                if (!Response.Headers.ContainsKey("Pragma"))
                {
                    Response.Headers.Add("Pragma", "no-cache");
                }

                if (!Response.Headers.ContainsKey("Expires"))
                {
                    Response.Headers.Add("Expires", "0");
                }

                return RedirectToAction("Login", "Login");
            }

            catch(Exception ex) 
            {
                Console.WriteLine($"Exception in LogOut action: {ex.Message}");
                return RedirectToAction("Login", "Login");
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult index1()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
