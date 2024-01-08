using System;
using System.Collections.Generic;

namespace CK.Models;

public partial class Store
{
    public int Id { get; set; }

    public int StoreId { get; set; }

    public string? Location { get; set; }

    public string? Government { get; set; }

    public bool? Closed { get; set; }

    public string? Ip { get; set; }

    public string? DatabaseName { get; set; }

    public string? Franchise { get; set; }

    public DateTime? LastTransactionDate { get; set; }

    public string? PriceCategory { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public DateTime? FirstTransactionDate { get; set; }

    public bool? Seasonal { get; set; }

    public string? Type { get; set; }

    public bool? InsideTown { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? City { get; set; }

    public string? StoreNameD365 { get; set; }
}
