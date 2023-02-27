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

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Fund> Funds { get; set; }

    public virtual DbSet<FundDetail> FundDetails { get; set; }

    public virtual DbSet<HistoryAction> HistoryActions { get; set; }

    public virtual DbSet<Import> Imports { get; set; }

    public virtual DbSet<ImportDetail> ImportDetails { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Rule> Rules { get; set; }

    public virtual DbSet<ShipingMethod> ShipingMethods { get; set; }

    public virtual DbSet<StatusImport> StatusImports { get; set; }

    public virtual DbSet<StatusOrder> StatusOrders { get; set; }

    public virtual DbSet<StatusPayment> StatusPayments { get; set; }

    public virtual DbSet<StatusShiping> StatusShipings { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<TypeAction> TypeActions { get; set; }

    public virtual DbSet<TypeFund> TypeFunds { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    public virtual DbSet<VoucherStatus> VoucherStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.ToTable("Brand");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd HH:mm'))")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasDefaultValueSql("('')");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd HH:mm'))")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ComboDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ComboDetail");

            entity.HasOne(d => d.Combo).WithMany()
                .HasForeignKey(d => d.ComboId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComboDetail_Product");

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComboDetail_Product1");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasDefaultValueSql("('')");
            entity.Property(e => e.Birthday)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd HH:mm'))")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateUserName).HasDefaultValueSql("('')");
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
            entity.ToTable("Employee");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Birthday).HasColumnType("date");
            entity.Property(e => e.DistrictName).HasDefaultValueSql("('')");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('')");
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

            entity.HasOne(d => d.Ward).WithMany(p => p.EmployeeWards)
                .HasForeignKey(d => d.WardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Location2");
        });

        modelBuilder.Entity<Fund>(entity =>
        {
            entity.ToTable("Fund");

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
            entity.ToTable("FundDetail");

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
                .ToTable("HistoryAction");

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
            entity.ToTable("Import");

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
            entity
                .HasNoKey()
                .ToTable("ImportDetail");

            entity.Property(e => e.ProductName).HasDefaultValueSql("('')");
            entity.Property(e => e.ProductNumber)
                .HasMaxLength(10)
                .HasDefaultValueSql("('')")
                .IsFixedLength();

            entity.HasOne(d => d.Import).WithMany()
                .HasForeignKey(d => d.ImportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImportDetail_Import");

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImportDetail_Product");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.ToTable("Location");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ParentId).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

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
            entity.Property(e => e.OrderShipingMethodId).HasDefaultValueSql("((1))");
            entity.Property(e => e.OrderShipingMethodName).HasDefaultValueSql("('')");
            entity.Property(e => e.OrderStatusId).HasDefaultValueSql("((1))");
            entity.Property(e => e.OrderStatusName).HasDefaultValueSql("('')");
            entity.Property(e => e.OrderStatusPaymentId).HasDefaultValueSql("((1))");
            entity.Property(e => e.OrderStatusPaymentName).HasDefaultValueSql("('')");
            entity.Property(e => e.OrderStatusShipingId).HasDefaultValueSql("((1))");
            entity.Property(e => e.OrderStatusShipingName).HasDefaultValueSql("('')");
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

            entity.HasOne(d => d.OrderShipingMethod).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderShipingMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_ShipingMethod");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_StatusOrder");

            entity.HasOne(d => d.OrderStatusPayment).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusPaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_StatusPayment");

            entity.HasOne(d => d.OrderStatusShiping).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusShipingId)
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

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("OrderDetail");

            entity.Property(e => e.ProductImage).HasDefaultValueSql("('')");
            entity.Property(e => e.ProductName).HasDefaultValueSql("('')");
            entity.Property(e => e.ProductNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Order).WithMany()
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Order");

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

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

            entity.HasOne(d => d.UserCreate).WithMany(p => p.Products)
                .HasForeignKey(d => d.UserCreateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Employee");
        });

        modelBuilder.Entity<Rule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Rule_1");

            entity.ToTable("Rule");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ShipingMethod>(entity =>
        {
            entity.ToTable("ShipingMethod");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<StatusImport>(entity =>
        {
            entity.ToTable("StatusImport");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasDefaultValueSql("('')");
        });

        modelBuilder.Entity<StatusOrder>(entity =>
        {
            entity.ToTable("StatusOrder");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasDefaultValueSql("('')");
        });

        modelBuilder.Entity<StatusPayment>(entity =>
        {
            entity.ToTable("StatusPayment");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<StatusShiping>(entity =>
        {
            entity.ToTable("StatusShiping");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("Supplier");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasDefaultValueSql("('')");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd HH:mm'))")
                .HasColumnType("datetime");
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

        modelBuilder.Entity<TypeAction>(entity =>
        {
            entity.ToTable("TypeAction");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasDefaultValueSql("('')");
        });

        modelBuilder.Entity<TypeFund>(entity =>
        {
            entity.ToTable("TypeFund");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasDefaultValueSql("('')");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.ToTable("Voucher");

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
            entity.ToTable("VoucherStatus");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasDefaultValueSql("('')");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
