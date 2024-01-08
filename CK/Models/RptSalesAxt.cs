using System;
using System.Collections.Generic;

namespace CK.Models;

public partial class RptSalesAxt
{
    public string Transactionid { get; set; } = null!;

    public string StoreId { get; set; } = null!;

    public long Categoryid { get; set; }

    public long Channel { get; set; }

    public decimal Costamount { get; set; }

    public string Currency { get; set; } = null!;

    public decimal Discamount { get; set; }

    public string StoreName { get; set; } = null!;

    public int Inventstatussales { get; set; }

    public string Inventtransid { get; set; } = null!;

    public string ItemLookupCode { get; set; } = null!;

    public decimal Linenum { get; set; }

    public string Listingid { get; set; } = null!;

    public decimal Netamount { get; set; }

    public decimal Netprice { get; set; }

    public decimal Originalprice { get; set; }

    public decimal Taxamount { get; set; }

    public decimal Totaldiscamount { get; set; }

    public int Transactionstatus { get; set; }

    public string Unit { get; set; } = null!;

    public DateTime TransTime { get; set; }

    public DateTime? Yesterday { get; set; }

    public int? LastMonth { get; set; }

    public int? LastYear { get; set; }

    public DateTime? LastWeek { get; set; }

    public int? ByDay { get; set; }

    public int? ByMonth { get; set; }

    public int? ByYear { get; set; }

    public DateTime? TransDate { get; set; }

    public decimal? Qty { get; set; }

    public decimal Price { get; set; }

    public decimal? TotalSales { get; set; }

    public int Entrystatus { get; set; }

    public string TransactionNumber { get; set; } = null!;

    public int Type { get; set; }

    public string DpId { get; set; } = null!;

    public string DpName { get; set; } = null!;
}
