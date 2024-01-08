using System;
using System.Collections.Generic;

namespace CK.Models;

public partial class Department
{
    public int Hqid { get; set; }

    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }

    public byte[]? DbtimeStamp { get; set; }

    public int Storeid { get; set; }
}
