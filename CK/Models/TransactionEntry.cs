using System;
using System.Collections.Generic;

namespace CK.Models;

public partial class TransactionEntry
{
    public decimal Commission { get; set; }

    public decimal Cost { get; set; }

    public decimal FullPrice { get; set; }

    public int StoreId { get; set; }

    public int Id { get; set; }

    public int TransactionNumber { get; set; }

    public int ItemId { get; set; }

    public decimal Price { get; set; }

    public short PriceSource { get; set; }

    public double Quantity { get; set; }

    public int SalesRepId { get; set; }

    public bool Taxable { get; set; }

    public int DetailId { get; set; }

    public string? Comment { get; set; }

    public byte[]? DbtimeStamp { get; set; }

    public int DiscountReasonCodeId { get; set; }

    public int ReturnReasonCodeId { get; set; }

    public int TaxChangeReasonCodeId { get; set; }

    public decimal SalesTax { get; set; }

    public int QuantityDiscountId { get; set; }

    public int ItemType { get; set; }

    public double? ComputedQuantity { get; set; }

    public DateTime? TransactionTime { get; set; }

    public bool IsAddMoney { get; set; }

    public int VoucherId { get; set; }
}
