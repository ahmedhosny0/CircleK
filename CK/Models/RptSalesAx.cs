using System;
using System.Collections.Generic;

namespace CK.Models;

public partial class RptSalesAx
{
    public string Transactionid { get; set; } = null!;

    public string Store { get; set; } = null!;

    public long Categoryid { get; set; }

    public long Channel { get; set; }

    public decimal Costamount { get; set; }

    public string Currency { get; set; } = null!;

    public decimal Discamount { get; set; }

    public string Inventlocationid { get; set; } = null!;

    public int Inventstatussales { get; set; }

    public string Inventtransid { get; set; } = null!;

    public string Itemid { get; set; } = null!;

    public decimal Linenum { get; set; }

    public string Listingid { get; set; } = null!;

    public decimal Netamount { get; set; }

    public decimal Netamountincltax { get; set; }

    public decimal Netprice { get; set; }

    public decimal Originalprice { get; set; }

    public decimal Price { get; set; }

    public decimal Qty { get; set; }

    public decimal Taxamount { get; set; }

    public decimal Totaldiscamount { get; set; }

    public int Transactionstatus { get; set; }

    public DateTime? Transdate { get; set; }

    public string Unit { get; set; } = null!;

    public int Entrystatus { get; set; }

    public string Invoiceid { get; set; } = null!;

    public string CatCode { get; set; } = null!;

    public string CatName { get; set; } = null!;
}
