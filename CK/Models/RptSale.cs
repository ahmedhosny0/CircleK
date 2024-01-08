using System;
using System.Collections.Generic;

namespace CK.Models;

public partial class RptSale
{
    public int DpId { get; set; }

    public string? DpName { get; set; }

    public int StoreCode { get; set; }

    public int StoreId { get; set; }

    public string? StoreName { get; set; }

    public string? StoreFranchise { get; set; }

    public DateTime? StoreFirstTransactionDate { get; set; }

    public string? StoreGov { get; set; }

    public string? StorePriceCat { get; set; }

    public string? StoreNameD365 { get; set; }

    public string? StoreType { get; set; }

    public int ItemId { get; set; }

    public string? ItemName { get; set; }

    public string ItemLookupCode { get; set; } = null!;

    public short ItemType { get; set; }

    public DateTime? TransTime { get; set; }

    public DateTime? Yesterday { get; set; }

    public int? LastMonth { get; set; }

    public int? LastYear { get; set; }

    public DateTime? LastWeek { get; set; }

    public int? ByDay { get; set; }

    public int? ByMonth { get; set; }

    public int? ByYear { get; set; }

    public DateTime? TransDate { get; set; }

    public double Qty { get; set; }

    public decimal Price { get; set; }

    public double? TotalSales { get; set; }

    public int TransactionNumber { get; set; }

    public decimal Cost { get; set; }

    public double? TotalCost { get; set; }

    public double? Profit { get; set; }

    public decimal Tax { get; set; }

    public double? TotalSalesWithoutTax { get; set; }

    public double? TotalCostWithoutTax { get; set; }

    public int SupplierId { get; set; }

    public string? SupplierCode { get; set; }

    public string? SupplierName { get; set; }

    public int StoreType1 { get; set; }
}
