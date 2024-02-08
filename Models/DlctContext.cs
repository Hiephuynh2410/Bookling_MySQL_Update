using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Booking.Models;

public partial class DlctContext : DbContext
{
    public DlctContext()
    {
    }

    public DlctContext(DbContextOptions<DlctContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<Billdetail> Billdetails { get; set; }

    public virtual DbSet<BlogCategory> BlogCategories { get; set; }

    public virtual DbSet<BlogPost> BlogPosts { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Bookingdetail> Bookingdetails { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Combo> Combos { get; set; }

    public virtual DbSet<Combodetail> Combodetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Producttype> Producttypes { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Scheduledetail> Scheduledetails { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Servicetype> Servicetypes { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=DLCT;user=root;password=Hiep2410", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.3.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("PRIMARY");

            entity.ToTable("BILL");

            entity.HasIndex(e => e.ClientId, "FK_BILL_CLIENT");

            entity.Property(e => e.BillId).HasColumnName("Bill_id");
            entity.Property(e => e.ClientId).HasColumnName("Client_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.Client).WithMany(p => p.Bills)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_BILL_CLIENT");
        });

        modelBuilder.Entity<Billdetail>(entity =>
        {
            entity.HasKey(e => new { e.BillId, e.ProductId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("BILLDETAIL");

            entity.HasIndex(e => e.ProductId, "FK_BILLDETAIL_PRODUCT");

            entity.Property(e => e.BillId).HasColumnName("Bill_id");
            entity.Property(e => e.ProductId).HasColumnName("Product_id");
            entity.Property(e => e.Price).HasPrecision(10);

            entity.HasOne(d => d.Bill).WithMany(p => p.Billdetails)
                .HasForeignKey(d => d.BillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BILLDETAIL_BILL");

            entity.HasOne(d => d.Product).WithMany(p => p.Billdetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BILLDETAIL_PRODUCT");
        });

        modelBuilder.Entity<BlogCategory>(entity =>
        {
            entity.HasKey(e => e.BlogCategoryId).HasName("PRIMARY");

            entity.ToTable("BLOG_CATEGORIES");

            entity.Property(e => e.BlogCategoryId).HasColumnName("Blog_category_id");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(500);
        });

        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.HasKey(e => e.BlogPostId).HasName("PRIMARY");

            entity.ToTable("BLOG_POSTS");

            entity.HasIndex(e => e.BlogCategoryId, "FK_BLOG_POSTS_CATEGORY");

            entity.HasIndex(e => e.StaffId, "FK_BLOG_POSTS_STAFF");

            entity.Property(e => e.BlogPostId).HasColumnName("Blog_post_id");
            entity.Property(e => e.BlogCategoryId).HasColumnName("Blog_category_id");
            entity.Property(e => e.Body).HasMaxLength(500);
            entity.Property(e => e.DateTime)
                .HasColumnType("datetime")
                .HasColumnName("Date_time");
            entity.Property(e => e.StaffId).HasColumnName("Staff_id");
            entity.Property(e => e.Thumbnail).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.BlogCategory).WithMany(p => p.BlogPosts)
                .HasForeignKey(d => d.BlogCategoryId)
                .HasConstraintName("FK_BLOG_POSTS_CATEGORY");

            entity.HasOne(d => d.Staff).WithMany(p => p.BlogPosts)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_BLOG_POSTS_STAFF");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PRIMARY");

            entity.ToTable("BOOKING");

            entity.HasIndex(e => e.BranchId, "FK_BOOKING_BRANCH");

            entity.HasIndex(e => e.ClientId, "FK_BOOKING_CLIENT");

            entity.HasIndex(e => e.ComboId, "FK_BOOKING_COMBO");

            entity.HasIndex(e => e.StaffId, "FK_BOOKING_STAFF");

            entity.Property(e => e.BookingId).HasColumnName("Booking_id");
            entity.Property(e => e.BranchId).HasColumnName("Branch_id");
            entity.Property(e => e.ClientId).HasColumnName("Client_id");
            entity.Property(e => e.ComboId).HasColumnName("Combo_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.DateTime)
                .HasColumnType("datetime")
                .HasColumnName("Date_time");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(10);
            entity.Property(e => e.StaffId).HasColumnName("Staff_id");

            entity.HasOne(d => d.Branch).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK_BOOKING_BRANCH");

            entity.HasOne(d => d.Client).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_BOOKING_CLIENT");

            entity.HasOne(d => d.Combo).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ComboId)
                .HasConstraintName("FK_BOOKING_COMBO");

            entity.HasOne(d => d.Staff).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_BOOKING_STAFF");
        });

        modelBuilder.Entity<Bookingdetail>(entity =>
        {
            entity.HasKey(e => new { e.BookingId, e.ServiceId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("BOOKINGDETAIL");

            entity.HasIndex(e => e.ServiceId, "FK_BOOKINGDETAIL_SERVICE");

            entity.Property(e => e.BookingId).HasColumnName("Booking_id");
            entity.Property(e => e.ServiceId).HasColumnName("Service_id");
            entity.Property(e => e.Price).HasPrecision(10);

            entity.HasOne(d => d.Booking).WithMany(p => p.Bookingdetails)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOOKINGDETAIL_BOOKING");

            entity.HasOne(d => d.Service).WithMany(p => p.Bookingdetails)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOOKINGDETAIL_SERVICE");
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("PRIMARY");

            entity.ToTable("BRANCH");

            entity.Property(e => e.BranchId).HasColumnName("Branch_id");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Hotline).HasMaxLength(10);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.ProductId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("CART");

            entity.HasIndex(e => e.ProductId, "FK_CART_PRODUCT");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ProductId).HasColumnName("Product_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CART_PRODUCT");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CART_CLIENT");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PRIMARY");

            entity.ToTable("CLIENT");

            entity.HasIndex(e => e.RoleId, "FK_CLIENT_ROLE");

            entity.Property(e => e.ClientId).HasColumnName("Client_id");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .HasColumnName("Created_by");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(10);
            entity.Property(e => e.RoleId).HasColumnName("Role_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .HasColumnName("Updated_by");
            entity.Property(e => e.Username).HasMaxLength(255);

            entity.HasOne(d => d.Role).WithMany(p => p.Clients)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_CLIENT_ROLE");
        });

        modelBuilder.Entity<Combo>(entity =>
        {
            entity.HasKey(e => e.ComboId).HasName("PRIMARY");

            entity.ToTable("COMBO");

            entity.Property(e => e.ComboId).HasColumnName("Combo_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_by");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasPrecision(10);
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_by");
        });

        modelBuilder.Entity<Combodetail>(entity =>
        {
            entity.HasKey(e => new { e.ComboId, e.ServiceId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("COMBODETAIL");

            entity.HasIndex(e => e.ServiceId, "FK_COMBODETAIL_SERVICE");

            entity.Property(e => e.ComboId).HasColumnName("Combo_id");
            entity.Property(e => e.ServiceId).HasColumnName("Service_id");
            entity.Property(e => e.Price).HasPrecision(10);

            entity.HasOne(d => d.Combo).WithMany(p => p.Combodetails)
                .HasForeignKey(d => d.ComboId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COMBODETAIL_COMBO");

            entity.HasOne(d => d.Service).WithMany(p => p.Combodetails)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COMBODETAIL_SERVICE");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PRIMARY");

            entity.ToTable("PRODUCT");

            entity.HasIndex(e => e.ProviderId, "FK_PRODUCT_PROVIDER");

            entity.HasIndex(e => e.ProductTypeId, "FK_PRODUCT_TYPE");

            entity.Property(e => e.ProductId).HasColumnName("Product_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_by");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Image).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasPrecision(10);
            entity.Property(e => e.ProductTypeId).HasColumnName("Product_type_id");
            entity.Property(e => e.ProviderId).HasColumnName("Provider_id");
            entity.Property(e => e.Sold).HasDefaultValueSql("'0'");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_by");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PRODUCT_TYPE");

            entity.HasOne(d => d.Provider).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PRODUCT_PROVIDER");
        });

        modelBuilder.Entity<Producttype>(entity =>
        {
            entity.HasKey(e => e.ProductTypeId).HasName("PRIMARY");

            entity.ToTable("PRODUCTTYPE");

            entity.Property(e => e.ProductTypeId).HasColumnName("Product_type_id");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.ProviderId).HasName("PRIMARY");

            entity.ToTable("PROVIDER");

            entity.Property(e => e.ProviderId).HasColumnName("Provider_id");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("ROLE");

            entity.Property(e => e.RoleId).HasColumnName("Role_id");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PRIMARY");

            entity.ToTable("SCHEDULE");

            entity.Property(e => e.ScheduleId).HasColumnName("Schedule_id");
            entity.Property(e => e.Time).HasColumnType("time");
        });

        modelBuilder.Entity<Scheduledetail>(entity =>
        {
            entity.HasKey(e => new { e.ScheduleId, e.StaffId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("SCHEDULEDETAIL");

            entity.HasIndex(e => e.StaffId, "FK_SCHEDULEDETAIL_STAFF");

            entity.Property(e => e.ScheduleId).HasColumnName("Schedule_id");
            entity.Property(e => e.StaffId).HasColumnName("Staff_id");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Scheduledetails)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SCHEDULEDETAIL_SCHEDULE");

            entity.HasOne(d => d.Staff).WithMany(p => p.Scheduledetails)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SCHEDULEDETAIL_STAFF");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PRIMARY");

            entity.ToTable("SERVICE");

            entity.HasIndex(e => e.ServiceTypeId, "FK_SERVICE_TYPE");

            entity.Property(e => e.ServiceId).HasColumnName("Service_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_by");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasPrecision(10);
            entity.Property(e => e.ServiceTypeId).HasColumnName("Service_type_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_by");

            entity.HasOne(d => d.ServiceType).WithMany(p => p.Services)
                .HasForeignKey(d => d.ServiceTypeId)
                .HasConstraintName("FK_SERVICE_TYPE");
        });

        modelBuilder.Entity<Servicetype>(entity =>
        {
            entity.HasKey(e => e.ServiceTypeId).HasName("PRIMARY");

            entity.ToTable("SERVICETYPE");

            entity.Property(e => e.ServiceTypeId).HasColumnName("Service_type_id");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PRIMARY");

            entity.ToTable("STAFF");

            entity.HasIndex(e => e.BranchId, "FK_STAFF_BRANCH");

            entity.HasIndex(e => e.RoleId, "FK_STAFF_ROLE");

            entity.Property(e => e.StaffId).HasColumnName("Staff_id");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.BranchId).HasColumnName("Branch_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .HasColumnName("Created_by");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.LastFailedLoginAttempt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(10);
            entity.Property(e => e.RoleId).HasColumnName("Role_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .HasColumnName("Updated_by");
            entity.Property(e => e.Username).HasMaxLength(255);

            entity.HasOne(d => d.Branch).WithMany(p => p.Staff)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK_STAFF_BRANCH");

            entity.HasOne(d => d.Role).WithMany(p => p.Staff)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_STAFF_ROLE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
