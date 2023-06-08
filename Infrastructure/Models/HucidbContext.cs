using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Models;

public partial class HucidbContext : DbContext
{
    public HucidbContext(DbContextOptions<HucidbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ComboDetail> ComboDetails { get; set; }

    public virtual DbSet<Commission> Commissions { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Fund> Funds { get; set; }

    public virtual DbSet<FundDetail> FundDetails { get; set; }

    public virtual DbSet<HistoryAction> HistoryActions { get; set; }

    public virtual DbSet<Import> Imports { get; set; }

    public virtual DbSet<ImportDetail> ImportDetails { get; set; }

    public virtual DbSet<Ip> Ips { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderCommission> OrderCommissions { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderPaymentMethod> OrderPaymentMethods { get; set; }

    public virtual DbSet<OrderSource> OrderSources { get; set; }

    public virtual DbSet<OrderTakeCare> OrderTakeCares { get; set; }

    public virtual DbSet<OtherCost> OtherCosts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Rule> Rules { get; set; }

    public virtual DbSet<SalaryType> SalaryTypes { get; set; }

    public virtual DbSet<Shift> Shifts { get; set; }

    public virtual DbSet<ShippingMethod> ShippingMethods { get; set; }

    public virtual DbSet<StatusImport> StatusImports { get; set; }

    public virtual DbSet<StatusOrder> StatusOrders { get; set; }

    public virtual DbSet<StatusPayment> StatusPayments { get; set; }

    public virtual DbSet<StatusShipping> StatusShippings { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<TimeKeeping> TimeKeepings { get; set; }

    public virtual DbSet<TypeAction> TypeActions { get; set; }

    public virtual DbSet<TypeFund> TypeFunds { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    public virtual DbSet<VoucherStatus> VoucherStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("huciapi");

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.ToTable("Brand", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd HH:mm'))")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasDefaultValueSql("('')");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd HH:mm'))")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ComboDetail>(entity =>
        {
            entity.ToTable("ComboDetail", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Combo).WithMany(p => p.ComboDetailCombos)
                .HasForeignKey(d => d.ComboId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComboDetail_Product");

            entity.HasOne(d => d.Product).WithMany(p => p.ComboDetailProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComboDetail_Product1");
        });

        modelBuilder.Entity<Commission>(entity =>
        {
            entity.ToTable("Commission", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd HH:mm'))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.UserCreate).WithMany(p => p.Commissions)
                .HasForeignKey(d => d.UserCreateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Commission_Employee");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasDefaultValueSql("('')");
            entity.Property(e => e.Birthday)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd HH:mm'))")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateUserName).HasDefaultValueSql("('')");
            entity.Property(e => e.CustomerNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DistrictName).HasDefaultValueSql("('')");
            entity.Property(e => e.Email)
                .IsUnicode(false)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.IpV4)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))");
            entity.Property(e => e.Name).HasDefaultValueSql("('')");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))");
            entity.Property(e => e.ProvinceName).HasDefaultValueSql("('')");
            entity.Property(e => e.WardName).HasDefaultValueSql("('')");

            entity.HasOne(d => d.CreateUser).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customer_Employee");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Birthday).HasColumnType("date");
            entity.Property(e => e.DistrictName).HasDefaultValueSql("('')");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.Image).IsUnicode(false);
            entity.Property(e => e.Name).HasDefaultValueSql("('')");
            entity.Property(e => e.Notes).HasDefaultValueSql("('')");
            entity.Property(e => e.Password)
                .IsUnicode(false)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.ProvinceName).HasDefaultValueSql("('')");
            entity.Property(e => e.RuleName).HasDefaultValueSql("('')");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.WardName).HasDefaultValueSql("('')");

            entity.HasOne(d => d.District).WithMany(p => p.EmployeeDistricts)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Location1");

            entity.HasOne(d => d.Province).WithMany(p => p.EmployeeProvinces)
                .HasForeignKey(d => d.ProvinceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Location");

            entity.HasOne(d => d.Rule).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Rule");

            entity.HasOne(d => d.SalaryType).WithMany(p => p.Employees)
                .HasForeignKey(d => d.SalaryTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_SalaryType");

            entity.HasOne(d => d.Ward).WithMany(p => p.EmployeeWards)
                .HasForeignKey(d => d.WardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Location2");
        });

        modelBuilder.Entity<Fund>(entity =>
        {
            entity.ToTable("Fund", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd HH:mm'))")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Name).HasDefaultValueSql("('')");
            entity.Property(e => e.Note).HasDefaultValueSql("('')");

            entity.HasOne(d => d.UserCreate).WithMany(p => p.Funds)
                .HasForeignKey(d => d.UserCreateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fund_Employee");
        });

        modelBuilder.Entity<FundDetail>(entity =>
        {
            entity.ToTable("FundDetail", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd HH:mm'))")
                .HasColumnType("datetime");
            entity.Property(e => e.Note).HasDefaultValueSql("('')");
            entity.Property(e => e.TypeFundName).HasDefaultValueSql("('')");

            entity.HasOne(d => d.Fund).WithMany(p => p.FundDetails)
                .HasForeignKey(d => d.FundId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FundDetail_Fund");

            entity.HasOne(d => d.TypeFund).WithMany(p => p.FundDetails)
                .HasForeignKey(d => d.TypeFundId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FundDetail_TypeFund");

            entity.HasOne(d => d.UserCreate).WithMany(p => p.FundDetails)
                .HasForeignKey(d => d.UserCreateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FundDetail_Employee");
        });

        modelBuilder.Entity<HistoryAction>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("HistoryAction", "dbo");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd HH:mm'))")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasDefaultValueSql("('')");

            entity.HasOne(d => d.TypeAction).WithMany()
                .HasForeignKey(d => d.TypeActionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistoryAction_TypeAction");

            entity.HasOne(d => d.UserCreate).WithMany()
                .HasForeignKey(d => d.UserCreateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistoryAction_Employee");
        });

        modelBuilder.Entity<Import>(entity =>
        {
            entity.ToTable("Import", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd HH:mm'))")
                .HasColumnType("datetime");
            entity.Property(e => e.ImportNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.StatusImportName).HasDefaultValueSql("('')");

            entity.HasOne(d => d.StatusImport).WithMany(p => p.Imports)
                .HasForeignKey(d => d.StatusImportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Import_StatusImport");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Imports)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Import_Supplier");

            entity.HasOne(d => d.UserCreate).WithMany(p => p.Imports)
                .HasForeignKey(d => d.UserCreateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Import_Employee");
        });

        modelBuilder.Entity<ImportDetail>(entity =>
        {
            entity.ToTable("ImportDetail", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ProductName).HasDefaultValueSql("('')");
            entity.Property(e => e.ProductNumber)
                .HasMaxLength(10)
                .HasDefaultValueSql("('')")
                .IsFixedLength();

            entity.HasOne(d => d.Import).WithMany(p => p.ImportDetails)
                .HasForeignKey(d => d.ImportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImportDetail_Import");

            entity.HasOne(d => d.Product).WithMany(p => p.ImportDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImportDetail_Product");
        });

        modelBuilder.Entity<Ip>(entity =>
        {
            entity.ToTable("IP");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd HH:mm'))")
                .HasColumnType("datetime");
            entity.Property(e => e.Ipv4).IsUnicode(false);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.ToTable("Location", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ParentId).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateEmployeeName).HasDefaultValueSql("('')");
            entity.Property(e => e.CustomerAddress).HasDefaultValueSql("('')");
            entity.Property(e => e.CustomerEmail).HasDefaultValueSql("('')");
            entity.Property(e => e.CustomerName).HasDefaultValueSql("('')");
            entity.Property(e => e.CustomerPhone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.DistrictName).HasDefaultValueSql("('')");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd HH:mm'))")
                .HasColumnType("datetime");
            entity.Property(e => e.OrderNote).HasDefaultValueSql("('')");
            entity.Property(e => e.OrderNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.OrderShippingMethodName).HasDefaultValueSql("('')");
            entity.Property(e => e.OrderSourceName).HasDefaultValueSql("('')");
            entity.Property(e => e.OrderStatusName).HasDefaultValueSql("('')");
            entity.Property(e => e.OrderStatusPaymentName).HasDefaultValueSql("('')");
            entity.Property(e => e.OrderStatusShippingName).HasDefaultValueSql("('')");
            entity.Property(e => e.ProvinceName).HasDefaultValueSql("('')");
            entity.Property(e => e.VoucherName).HasDefaultValueSql("('')");
            entity.Property(e => e.WardName).HasDefaultValueSql("('')");

            entity.HasOne(d => d.CreateEmployee).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CreateEmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Employee");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Customer");

            entity.HasOne(d => d.District).WithMany(p => p.OrderDistricts)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Location");

            entity.HasOne(d => d.OrderShippingMethod).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderShippingMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_ShipingMethod");

            entity.HasOne(d => d.OrderSource).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderSourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_OrderSource");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_StatusOrder");

            entity.HasOne(d => d.OrderStatusPayment).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusPaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_StatusPayment");

            entity.HasOne(d => d.OrderStatusShipping).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusShippingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_StatusShiping");

            entity.HasOne(d => d.Province).WithMany(p => p.OrderProvinces)
                .HasForeignKey(d => d.ProvinceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Location2");

            entity.HasOne(d => d.Voucher).WithMany(p => p.Orders)
                .HasForeignKey(d => d.VoucherId)
                .HasConstraintName("FK_Order_Voucher");

            entity.HasOne(d => d.Ward).WithMany(p => p.OrderWards)
                .HasForeignKey(d => d.WardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Location1");
        });

        modelBuilder.Entity<OrderCommission>(entity =>
        {
            entity.ToTable("OrderCommission", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.OrderCommission1).HasColumnName("OrderCommission");

            entity.HasOne(d => d.Employee).WithMany(p => p.OrderCommissions)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderCommission_Employee");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderCommissions)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderCommission_Order");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetail", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ProductImage).HasDefaultValueSql("('')");
            entity.Property(e => e.ProductName).HasDefaultValueSql("('')");
            entity.Property(e => e.ProductNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Product");
        });

        modelBuilder.Entity<OrderPaymentMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderPay__3214EC076B01BAD4");

            entity.ToTable("OrderPaymentMethod");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Order).WithMany(p => p.OrderPaymentMethods)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderPaym__Order__54CB950F");
        });

        modelBuilder.Entity<OrderSource>(entity =>
        {
            entity.ToTable("OrderSource", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.SourceName).HasDefaultValueSql("('')");
        });

        modelBuilder.Entity<OrderTakeCare>(entity =>
        {
            entity.ToTable("OrderTakeCare", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd HH:mm'))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderTakeCares)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderTakeCare_Order");

            entity.HasOne(d => d.UserCreate).WithMany(p => p.OrderTakeCares)
                .HasForeignKey(d => d.UserCreateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderTakeCare_Employee");
        });

        modelBuilder.Entity<OtherCost>(entity =>
        {
            entity.ToTable("OtherCost", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd HH:mm'))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.UserCreate).WithMany(p => p.OtherCosts)
                .HasForeignKey(d => d.UserCreateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OtherCost_Employee");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd HH:mm'))")
                .HasColumnType("datetime");
            entity.Property(e => e.Image)
                .IsUnicode(false)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Name).HasDefaultValueSql("('')");
            entity.Property(e => e.ProductNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("('')");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Brand");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Category");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductType");

            entity.HasOne(d => d.UserCreate).WithMany(p => p.Products)
                .HasForeignKey(d => d.UserCreateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Employee");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.ToTable("ProductType", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.ToTable("Promotion", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.Promotions)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Promotion_Product");

            entity.HasOne(d => d.UserCreate).WithMany(p => p.Promotions)
                .HasForeignKey(d => d.UserCreateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Promotion_Employee");
        });

        modelBuilder.Entity<Rule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Rule_1");

            entity.ToTable("Rule", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<SalaryType>(entity =>
        {
            entity.ToTable("SalaryType", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.ToTable("Shift");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd HH:mm'))")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ShippingMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ShipingMethod");

            entity.ToTable("ShippingMethod", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<StatusImport>(entity =>
        {
            entity.ToTable("StatusImport", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasDefaultValueSql("('')");
        });

        modelBuilder.Entity<StatusOrder>(entity =>
        {
            entity.ToTable("StatusOrder", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasDefaultValueSql("('')");
        });

        modelBuilder.Entity<StatusPayment>(entity =>
        {
            entity.ToTable("StatusPayment", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<StatusShipping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_StatusShiping");

            entity.ToTable("StatusShipping", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("Supplier", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasDefaultValueSql("('')");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd HH:mm'))")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateUserName).HasDefaultValueSql("('')");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.Name).HasDefaultValueSql("('')");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.ProvinceName).HasDefaultValueSql("('')");
            entity.Property(e => e.SupplierNumber)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.CreateUser).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Supplier_Employee");

            entity.HasOne(d => d.District).WithMany(p => p.SupplierDistricts)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Supplier_Location1");

            entity.HasOne(d => d.Province).WithMany(p => p.SupplierProvinces)
                .HasForeignKey(d => d.ProvinceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Supplier_Location");

            entity.HasOne(d => d.Ward).WithMany(p => p.SupplierWards)
                .HasForeignKey(d => d.WardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Supplier_Location2");
        });

        modelBuilder.Entity<TimeKeeping>(entity =>
        {
            entity.ToTable("TimeKeeping", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd HH:mm'))")
                .HasColumnType("datetime");

            entity.HasOne(d => d.UserCreate).WithMany(p => p.TimeKeepingUserCreates)
                .HasForeignKey(d => d.UserCreateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TimeKeeping_Employee");

            entity.HasOne(d => d.UserTimeKeeping).WithMany(p => p.TimeKeepingUserTimeKeepings)
                .HasForeignKey(d => d.UserTimeKeepingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TimeKeeping_Employee1");
        });

        modelBuilder.Entity<TypeAction>(entity =>
        {
            entity.ToTable("TypeAction", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasDefaultValueSql("('')");
        });

        modelBuilder.Entity<TypeFund>(entity =>
        {
            entity.ToTable("TypeFund", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasDefaultValueSql("('')");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.ToTable("Voucher", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.CreateUserName).HasDefaultValueSql("('')");
            entity.Property(e => e.EndDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Name).HasDefaultValueSql("('')");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.VoucherStatusName).HasDefaultValueSql("('')");

            entity.HasOne(d => d.CreateUser).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Voucher_Employee");

            entity.HasOne(d => d.VoucherStatus).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.VoucherStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Voucher_VoucherStatus");
        });

        modelBuilder.Entity<VoucherStatus>(entity =>
        {
            entity.ToTable("VoucherStatus", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasDefaultValueSql("('')");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
