using System;
using System.Collections.Generic;

namespace CK.Models;

public partial class RptSalesAll
{
    public int DpId { get; set; }

    public string? DpName { get; set; }

    public int StoreId { get; set; }

    public string? StoreName { get; set; }

    public string? StoreFranchise { get; set; }

    public string ItemLookupCode { get; set; } = null!;

    public string? ItemName { get; set; }

    public DateTime? TransTime { get; set; }

    public DateTime? Yesterday { get; set; }

    public DateTime? TransDate { get; set; }

    public double? Qty { get; set; }

    public decimal Price { get; set; }

    public double? TotalSales { get; set; }

    public int TransactionNumber { get; set; }

    public decimal Cost { get; set; }

    public double? TotalCost { get; set; }
    public int? ByDay { get; set; }

    public int? ByMonth { get; set; }

    public int? ByYear { get; set; }
}
