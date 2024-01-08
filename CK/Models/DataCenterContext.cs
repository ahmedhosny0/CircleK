using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CK.Models;

public partial class DataCenterContext : DbContext
{
    public DataCenterContext()
    {
    }

    public DataCenterContext(DbContextOptions<DataCenterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<RptSale> RptSales { get; set; }

    public virtual DbSet<RptSalesAx> RptSalesAxes { get; set; }

    public virtual DbSet<RptSalesAxt> RptSalesAxts { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<TransactionEntry> TransactionEntries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=192.168.1.156;User ID=sa;Password=P@ssw0rd;Database=DATA_CENTER;Connect Timeout=150;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Storeid }).HasName("PK_department_1");

            entity.ToTable("department");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Storeid).HasColumnName("storeid");
            entity.Property(e => e.Code)
                .HasMaxLength(17)
                .HasColumnName("code");
            entity.Property(e => e.DbtimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("DBTimeStamp");
            entity.Property(e => e.Hqid).HasColumnName("HQID");
            entity.Property(e => e.Name).HasMaxLength(30);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => new { e.Storeid, e.Id, e.ItemLookupCode }).IsClustered(false);

            entity.ToTable("Item", tb => tb.HasTrigger("TR_ITEM_LOG"));

            entity.HasIndex(e => e.BinLocation, "IX_BinLocation").HasFillFactor(80);

            entity.HasIndex(e => e.CategoryId, "IX_CategoryID").HasFillFactor(80);

            entity.HasIndex(e => e.DepartmentId, "IX_DepartmentID").HasFillFactor(80);

            entity.HasIndex(e => e.Description, "IX_Description").HasFillFactor(80);

            entity.HasIndex(e => e.Hqid, "IX_HQID").HasFillFactor(80);

            entity.HasIndex(e => new { e.Storeid, e.DepartmentId }, "IX_Item").HasFillFactor(80);

            entity.HasIndex(e => e.ItemLookupCode, "IX_ItemLookupCode").HasFillFactor(80);

            entity.HasIndex(e => e.DbtimeStamp, "IX_Item_TimeStamp")
                .IsUnique()
                .IsClustered()
                .HasFillFactor(80);

            entity.HasIndex(e => e.SupplierId, "IX_SupplierID").HasFillFactor(80);

            entity.Property(e => e.Storeid).HasColumnName("storeid");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ItemLookupCode)
                .HasMaxLength(25)
                .HasDefaultValue("");
            entity.Property(e => e.BinLocation)
                .HasMaxLength(20)
                .HasDefaultValue("");
            entity.Property(e => e.BlockSalesAfterDate).HasColumnType("datetime");
            entity.Property(e => e.BlockSalesBeforeDate).HasColumnType("datetime");
            entity.Property(e => e.BlockSalesReason)
                .HasMaxLength(30)
                .HasDefaultValue("");
            entity.Property(e => e.BlockSalesScheduleId).HasColumnName("BlockSalesScheduleID");
            entity.Property(e => e.BuydownPrice).HasColumnType("money");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CommissionAmount).HasColumnType("money");
            entity.Property(e => e.CommissionMaximum).HasColumnType("money");
            entity.Property(e => e.Content)
                .HasDefaultValue("")
                .HasColumnType("ntext");
            entity.Property(e => e.Cost).HasColumnType("money");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DbtimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("DBTimeStamp");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.Description)
                .HasMaxLength(30)
                .HasDefaultValue("");
            entity.Property(e => e.ExtendedDescription)
                .HasDefaultValue("")
                .HasColumnType("ntext");
            entity.Property(e => e.Hqid).HasColumnName("HQID");
            entity.Property(e => e.LastCost).HasColumnType("money");
            entity.Property(e => e.LastCounted).HasColumnType("datetime");
            entity.Property(e => e.LastReceived).HasColumnType("datetime");
            entity.Property(e => e.LastSold).HasColumnType("datetime");
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MessageId).HasColumnName("MessageID");
            entity.Property(e => e.Msrp)
                .HasColumnType("money")
                .HasColumnName("MSRP");
            entity.Property(e => e.Notes)
                .HasDefaultValue("")
                .HasColumnType("ntext");
            entity.Property(e => e.NumberFormat).HasMaxLength(20);
            entity.Property(e => e.PictureName)
                .HasMaxLength(50)
                .HasDefaultValue("");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.PriceA).HasColumnType("money");
            entity.Property(e => e.PriceB).HasColumnType("money");
            entity.Property(e => e.PriceC).HasColumnType("money");
            entity.Property(e => e.PriceLowerBound).HasColumnType("money");
            entity.Property(e => e.PriceUpperBound).HasColumnType("money");
            entity.Property(e => e.QuantityDiscountId).HasColumnName("QuantityDiscountID");
            entity.Property(e => e.ReplacementCost).HasColumnType("money");
            entity.Property(e => e.SaleEndDate).HasColumnType("datetime");
            entity.Property(e => e.SalePrice).HasColumnType("money");
            entity.Property(e => e.SaleScheduleId).HasColumnName("SaleScheduleID");
            entity.Property(e => e.SaleStartDate).HasColumnType("datetime");
            entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");
            entity.Property(e => e.SubDescription1)
                .HasMaxLength(30)
                .HasDefaultValue("");
            entity.Property(e => e.SubDescription2)
                .HasMaxLength(30)
                .HasDefaultValue("");
            entity.Property(e => e.SubDescription3)
                .HasMaxLength(30)
                .HasDefaultValue("");
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.TaxId).HasColumnName("TaxID");
            entity.Property(e => e.Taxable).HasDefaultValue(true);
            entity.Property(e => e.TenderId).HasColumnName("TenderID");
            entity.Property(e => e.UnitOfMeasure)
                .HasMaxLength(4)
                .HasDefaultValue("");
            entity.Property(e => e.UsuallyShip)
                .HasMaxLength(255)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<RptSale>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RptSales");

            entity.Property(e => e.Cost).HasColumnType("money");
            entity.Property(e => e.DpId).HasColumnName("DpID");
            entity.Property(e => e.DpName).HasMaxLength(30);
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.ItemLookupCode).HasMaxLength(25);
            entity.Property(e => e.ItemName).HasMaxLength(30);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.StoreFirstTransactionDate).HasColumnName("Store_FIRST_TRANSACTION_DATE");
            entity.Property(e => e.StoreFranchise)
                .HasMaxLength(50)
                .HasColumnName("Store_Franchise");
            entity.Property(e => e.StoreGov).HasMaxLength(50);
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.StoreName)
                .HasMaxLength(50)
                .HasColumnName("Store_Name");
            entity.Property(e => e.StoreNameD365).HasMaxLength(50);
            entity.Property(e => e.StorePriceCat)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.StoreType).HasMaxLength(50);
            entity.Property(e => e.StoreType1).HasColumnName("Store_Type");
            entity.Property(e => e.SupplierCode).HasMaxLength(17);
            entity.Property(e => e.SupplierName).HasMaxLength(30);
            entity.Property(e => e.Tax).HasColumnType("money");
            entity.Property(e => e.TransTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<RptSalesAx>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RptSalesAx");

            entity.Property(e => e.CatCode)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CatName)
                .HasMaxLength(254)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Categoryid).HasColumnName("CATEGORYID");
            entity.Property(e => e.Channel).HasColumnName("CHANNEL");
            entity.Property(e => e.Costamount)
                .HasColumnType("numeric(32, 6)")
                .HasColumnName("COSTAMOUNT");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("CURRENCY");
            entity.Property(e => e.Discamount)
                .HasColumnType("numeric(32, 6)")
                .HasColumnName("DISCAMOUNT");
            entity.Property(e => e.Entrystatus).HasColumnName("ENTRYSTATUS");
            entity.Property(e => e.Inventlocationid)
                .HasMaxLength(10)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("INVENTLOCATIONID");
            entity.Property(e => e.Inventstatussales).HasColumnName("INVENTSTATUSSALES");
            entity.Property(e => e.Inventtransid)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("INVENTTRANSID");
            entity.Property(e => e.Invoiceid)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("INVOICEID");
            entity.Property(e => e.Itemid)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("ITEMID");
            entity.Property(e => e.Linenum)
                .HasColumnType("numeric(32, 16)")
                .HasColumnName("LINENUM");
            entity.Property(e => e.Listingid)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("LISTINGID");
            entity.Property(e => e.Netamount)
                .HasColumnType("numeric(32, 6)")
                .HasColumnName("NETAMOUNT");
            entity.Property(e => e.Netamountincltax)
                .HasColumnType("numeric(32, 6)")
                .HasColumnName("NETAMOUNTINCLTAX");
            entity.Property(e => e.Netprice)
                .HasColumnType("numeric(32, 6)")
                .HasColumnName("NETPRICE");
            entity.Property(e => e.Originalprice)
                .HasColumnType("numeric(32, 6)")
                .HasColumnName("ORIGINALPRICE");
            entity.Property(e => e.Price).HasColumnType("numeric(32, 6)");
            entity.Property(e => e.Qty)
                .HasColumnType("numeric(32, 6)")
                .HasColumnName("QTY");
            entity.Property(e => e.Store)
                .HasMaxLength(10)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("STORE");
            entity.Property(e => e.Taxamount)
                .HasColumnType("numeric(32, 6)")
                .HasColumnName("TAXAMOUNT");
            entity.Property(e => e.Totaldiscamount)
                .HasColumnType("numeric(32, 6)")
                .HasColumnName("TOTALDISCAMOUNT");
            entity.Property(e => e.Transactionid)
                .HasMaxLength(44)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("TRANSACTIONID");
            entity.Property(e => e.Transactionstatus).HasColumnName("TRANSACTIONSTATUS");
            entity.Property(e => e.Transdate).HasColumnName("TRANSDATE");
            entity.Property(e => e.Unit)
                .HasMaxLength(10)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("UNIT");
        });

        modelBuilder.Entity<RptSalesAxt>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RptSalesAxt");

            entity.Property(e => e.Categoryid).HasColumnName("CATEGORYID");
            entity.Property(e => e.Channel).HasColumnName("CHANNEL");
            entity.Property(e => e.Costamount)
                .HasColumnType("numeric(32, 6)")
                .HasColumnName("COSTAMOUNT");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("CURRENCY");
            entity.Property(e => e.Discamount)
                .HasColumnType("numeric(32, 6)")
                .HasColumnName("DISCAMOUNT");
            entity.Property(e => e.DpId)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.DpName)
                .HasMaxLength(254)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Entrystatus).HasColumnName("ENTRYSTATUS");
            entity.Property(e => e.Inventstatussales).HasColumnName("INVENTSTATUSSALES");
            entity.Property(e => e.Inventtransid)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("INVENTTRANSID");
            entity.Property(e => e.ItemLookupCode)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Linenum)
                .HasColumnType("numeric(32, 16)")
                .HasColumnName("LINENUM");
            entity.Property(e => e.Listingid)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("LISTINGID");
            entity.Property(e => e.Netamount)
                .HasColumnType("numeric(32, 6)")
                .HasColumnName("NETAMOUNT");
            entity.Property(e => e.Netprice)
                .HasColumnType("numeric(32, 6)")
                .HasColumnName("NETPRICE");
            entity.Property(e => e.Originalprice)
                .HasColumnType("numeric(32, 6)")
                .HasColumnName("ORIGINALPRICE");
            entity.Property(e => e.Price).HasColumnType("numeric(32, 6)");
            entity.Property(e => e.Qty).HasColumnType("numeric(32, 6)");
            entity.Property(e => e.StoreId)
                .HasMaxLength(10)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("StoreID");
            entity.Property(e => e.StoreName)
                .HasMaxLength(10)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Taxamount)
                .HasColumnType("numeric(32, 6)")
                .HasColumnName("TAXAMOUNT");
            entity.Property(e => e.TotalSales).HasColumnType("numeric(32, 6)");
            entity.Property(e => e.Totaldiscamount)
                .HasColumnType("numeric(32, 6)")
                .HasColumnName("TOTALDISCAMOUNT");
            entity.Property(e => e.TransTime).HasColumnType("datetime");
            entity.Property(e => e.TransactionNumber)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Transactionid)
                .HasMaxLength(44)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("TRANSACTIONID");
            entity.Property(e => e.Transactionstatus).HasColumnName("TRANSACTIONSTATUS");
            entity.Property(e => e.Type).HasColumnName("TYPE");
            entity.Property(e => e.Unit)
                .HasMaxLength(10)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("UNIT");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("PK_STORES_1");

            entity.ToTable("STORES");

            entity.Property(e => e.StoreId)
                .ValueGeneratedNever()
                .HasColumnName("STORE_ID");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Closed).HasColumnName("CLOSED");
            entity.Property(e => e.DatabaseName)
                .HasMaxLength(50)
                .HasColumnName("DATABASE_NAME");
            entity.Property(e => e.FirstTransactionDate)
                .HasColumnType("datetime")
                .HasColumnName("FIRST_TRANSACTION_DATE");
            entity.Property(e => e.Franchise)
                .HasMaxLength(50)
                .HasColumnName("FRANCHISE");
            entity.Property(e => e.Government)
                .HasMaxLength(50)
                .HasColumnName("GOVERNMENT");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.InsideTown).HasColumnName("INSIDE_TOWN");
            entity.Property(e => e.Ip)
                .HasMaxLength(50)
                .HasColumnName("IP");
            entity.Property(e => e.LastTransactionDate)
                .HasColumnType("datetime")
                .HasColumnName("LAST_TRANSACTION_DATE");
            entity.Property(e => e.LastUpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("LAST_UPDATE_DATE");
            entity.Property(e => e.Latitude).HasColumnType("decimal(28, 10)");
            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .HasColumnName("LOCATION");
            entity.Property(e => e.Longitude).HasColumnType("decimal(28, 10)");
            entity.Property(e => e.PriceCategory)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("PRICE_CATEGORY");
            entity.Property(e => e.Seasonal).HasColumnName("SEASONAL");
            entity.Property(e => e.StoreNameD365).HasMaxLength(50);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("TYPE");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Storeid }).IsClustered(false);

            entity.ToTable("Supplier");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Storeid).HasColumnName("storeid");
            entity.Property(e => e.AccountNumber).HasMaxLength(20);
            entity.Property(e => e.Address1).HasMaxLength(30);
            entity.Property(e => e.Address2).HasMaxLength(30);
            entity.Property(e => e.City).HasMaxLength(30);
            entity.Property(e => e.Code).HasMaxLength(17);
            entity.Property(e => e.ContactName).HasMaxLength(30);
            entity.Property(e => e.Country).HasMaxLength(20);
            entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");
            entity.Property(e => e.CustomDate1).HasColumnType("datetime");
            entity.Property(e => e.CustomDate2).HasColumnType("datetime");
            entity.Property(e => e.CustomDate3).HasColumnType("datetime");
            entity.Property(e => e.CustomDate4).HasColumnType("datetime");
            entity.Property(e => e.CustomDate5).HasColumnType("datetime");
            entity.Property(e => e.CustomText1).HasMaxLength(30);
            entity.Property(e => e.CustomText2).HasMaxLength(30);
            entity.Property(e => e.CustomText3).HasMaxLength(30);
            entity.Property(e => e.CustomText4).HasMaxLength(30);
            entity.Property(e => e.CustomText5).HasMaxLength(30);
            entity.Property(e => e.DbtimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("DBTimeStamp");
            entity.Property(e => e.EmailAddress).HasMaxLength(255);
            entity.Property(e => e.FaxNumber).HasMaxLength(30);
            entity.Property(e => e.Hqid).HasColumnName("HQID");
            entity.Property(e => e.LastUpdated).HasColumnType("datetime");
            entity.Property(e => e.Notes).HasColumnType("ntext");
            entity.Property(e => e.PhoneNumber).HasMaxLength(30);
            entity.Property(e => e.State).HasMaxLength(20);
            entity.Property(e => e.SupplierName).HasMaxLength(30);
            entity.Property(e => e.TaxNumber).HasMaxLength(20);
            entity.Property(e => e.Terms).HasMaxLength(50);
            entity.Property(e => e.WebPageAddress).HasMaxLength(255);
            entity.Property(e => e.Zip).HasMaxLength(20);
        });

        modelBuilder.Entity<TransactionEntry>(entity =>
        {
            entity.HasKey(e => new { e.StoreId, e.Id }).IsClustered(false);

            entity.ToTable("TransactionEntry");

            entity.HasIndex(e => new { e.StoreId, e.ItemId }, "<Name of Missing Index, sysname,>").HasFillFactor(80);

            entity.HasIndex(e => e.Quantity, "IX_Quantity").HasFillFactor(80);

            entity.HasIndex(e => new { e.StoreId, e.TransactionNumber }, "IX_TransactionEntry").HasFillFactor(80);

            entity.HasIndex(e => new { e.StoreId, e.ItemId }, "IX_TransactionEntry11").HasFillFactor(80);

            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.Commission).HasColumnType("money");
            entity.Property(e => e.Cost).HasColumnType("money");
            entity.Property(e => e.DbtimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("DBTimeStamp");
            entity.Property(e => e.DetailId).HasColumnName("DetailID");
            entity.Property(e => e.DiscountReasonCodeId).HasColumnName("DiscountReasonCodeID");
            entity.Property(e => e.FullPrice).HasColumnType("money");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.QuantityDiscountId).HasColumnName("QuantityDiscountID");
            entity.Property(e => e.ReturnReasonCodeId).HasColumnName("ReturnReasonCodeID");
            entity.Property(e => e.SalesRepId).HasColumnName("SalesRepID");
            entity.Property(e => e.SalesTax).HasColumnType("money");
            entity.Property(e => e.TaxChangeReasonCodeId).HasColumnName("TaxChangeReasonCodeID");
            entity.Property(e => e.TransactionTime).HasColumnType("datetime");
            entity.Property(e => e.VoucherId).HasColumnName("VoucherID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
