using System;
using System.Collections.Generic;

namespace CK.Models;

public partial class Supplier
{
    public string? Country { get; set; }

    public int Hqid { get; set; }

    public DateTime LastUpdated { get; set; }

    public string? State { get; set; }

    public int Id { get; set; }

    public string? SupplierName { get; set; }

    public string? ContactName { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public string? Zip { get; set; }

    public string? EmailAddress { get; set; }

    public string? WebPageAddress { get; set; }

    public string? Code { get; set; }

    public byte[]? DbtimeStamp { get; set; }

    public string? AccountNumber { get; set; }

    public string? TaxNumber { get; set; }

    public int CurrencyId { get; set; }

    public string? PhoneNumber { get; set; }

    public string? FaxNumber { get; set; }

    public string? CustomText1 { get; set; }

    public string? CustomText2 { get; set; }

    public string? CustomText3 { get; set; }

    public string? CustomText4 { get; set; }

    public string? CustomText5 { get; set; }

    public double CustomNumber1 { get; set; }

    public double CustomNumber2 { get; set; }

    public double CustomNumber3 { get; set; }

    public double CustomNumber4 { get; set; }

    public double CustomNumber5 { get; set; }

    public DateTime? CustomDate1 { get; set; }

    public DateTime? CustomDate2 { get; set; }

    public DateTime? CustomDate3 { get; set; }

    public DateTime? CustomDate4 { get; set; }

    public DateTime? CustomDate5 { get; set; }

    public string? Terms { get; set; }

    public string Notes { get; set; } = null!;

    public int Storeid { get; set; }
}
