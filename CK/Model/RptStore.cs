using System;
using System.Collections.Generic;

namespace CK.Model;

public partial class RptStore
{
    public int Id { get; set; }

    public string? Franchise { get; set; }

    public string? Rmsd365 { get; set; }

    public int StoreIdR { get; set; }

    public string? CompanyName { get; set; }

    public string? StoreNameR { get; set; }

    public int? StoreIdD { get; set; }

    public string? StoreNameD { get; set; }

    public string? Email { get; set; }

    public DateTime? FirstTransactionR { get; set; }

    public DateTime? LastTransactionR { get; set; }

    public DateTime? FirstTransactionD { get; set; }

    public DateTime? LastTransactionD { get; set; }

    public string? PriceCategory { get; set; }

    public string ServerName { get; set; } = null!;

    public string? Zkip { get; set; }

    public string? DbName { get; set; }

    public string? DbZkname { get; set; }

    public bool? IsClosed { get; set; }

    public bool? IsAttendance { get; set; }

    public bool? IsAlly { get; set; }

    public bool? IsInMap { get; set; }

    public string? Lat { get; set; }

    public string? Long { get; set; }
}
