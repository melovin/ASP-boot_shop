using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BotyObchodASP.Models
{
    public partial class MyContext : DbContext
    {
        public MyContext()
        {
        }

        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbAdmin> TbAdmins { get; set; } = null!;
        public virtual DbSet<TbCategoriesDetail> TbCategoriesDetails { get; set; } = null!;
        public virtual DbSet<TbCategory> TbCategories { get; set; } = null!;
        public virtual DbSet<TbColor> TbColors { get; set; } = null!;
        public virtual DbSet<TbCustomer> TbCustomers { get; set; } = null!;
        public virtual DbSet<TbDelivery> TbDeliveries { get; set; } = null!;
        public virtual DbSet<TbOrder> TbOrders { get; set; } = null!;
        public virtual DbSet<TbOrderDetail> TbOrderDetails { get; set; } = null!;
        public virtual DbSet<TbPayment> TbPayments { get; set; } = null!;
        public virtual DbSet<TbPicture> TbPictures { get; set; } = null!;
        public virtual DbSet<TbProduct> TbProducts { get; set; } = null!;
        public virtual DbSet<TbStock> TbStocks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=mysqlstudenti.litv.sssvt.cz;user=zubarevaruslana;password=123456Ab;database=4b2_zubarevaruslana_db1", ServerVersion.Parse("10.3.25-mariadb"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_czech_ci")
                .HasCharSet("utf8");

            modelBuilder.Entity<TbAdmin>(entity =>
            {
                entity.ToTable("tbAdmin");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Password).HasMaxLength(1000);

                entity.Property(e => e.Username).HasMaxLength(100);
            });

            modelBuilder.Entity<TbCategoriesDetail>(entity =>
            {
                entity.ToTable("tbCategoriesDetails");

                entity.HasIndex(e => e.IdCategory, "IdCategory");

                entity.HasIndex(e => e.IdProduct, "IdProduct");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.IdCategory).HasColumnType("int(11)");

                entity.Property(e => e.IdProduct).HasColumnType("int(11)");

                entity.HasOne(d => d.IdCategoryNavigation)
                    .WithMany(p => p.TbCategoriesDetails)
                    .HasForeignKey(d => d.IdCategory)
                    .HasConstraintName("tbCategoriesDetails_ibfk_2");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.TbCategoriesDetails)
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbCategoriesDetails_ibfk_1");
            });

            modelBuilder.Entity<TbCategory>(entity =>
            {
                entity.ToTable("tbCategories");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.LeftIndex).HasColumnType("int(11)");

                entity.Property(e => e.Level).HasColumnType("int(11)");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.ParentId).HasColumnType("int(11)");

                entity.Property(e => e.Position).HasColumnType("int(11)");

                entity.Property(e => e.RightIndex).HasColumnType("int(11)");
            });

            modelBuilder.Entity<TbColor>(entity =>
            {
                entity.ToTable("tbColors");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.HexCode).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<TbCustomer>(entity =>
            {
                entity.ToTable("tbCustomers");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PostalCode).HasColumnType("int(11)");

                entity.Property(e => e.Surname).HasMaxLength(50);

                entity.Property(e => e.Tel).HasMaxLength(50);
            });

            modelBuilder.Entity<TbDelivery>(entity =>
            {
                entity.ToTable("tbDelivery");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<TbOrder>(entity =>
            {
                entity.ToTable("tbOrder");

                entity.HasIndex(e => e.IdCustomer, "IdCustomer");

                entity.HasIndex(e => e.IdPayment, "IdPayment");

                entity.HasIndex(e => e.IdDelivery, "IdShipping");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("current_timestamp()");

                entity.Property(e => e.DeliveredDate).HasColumnType("datetime");

                entity.Property(e => e.IdCustomer).HasColumnType("int(11)");

                entity.Property(e => e.IdDelivery).HasColumnType("int(11)");

                entity.Property(e => e.IdPayment).HasColumnType("int(11)");

                entity.Property(e => e.ShippedDate).HasColumnType("datetime");
                entity.Property(e => e.ShippedDate).HasColumnType("datetime");

                entity.HasOne(d => d.IdCustomerNavigation)
                    .WithMany(p => p.TbOrders)
                    .HasForeignKey(d => d.IdCustomer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbOrder_ibfk_1");

                entity.HasOne(d => d.IdDeliveryNavigation)
                    .WithMany(p => p.TbOrders)
                    .HasForeignKey(d => d.IdDelivery)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbOrder_ibfk_2");

                entity.HasOne(d => d.IdPaymentNavigation)
                    .WithMany(p => p.TbOrders)
                    .HasForeignKey(d => d.IdPayment)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbOrder_ibfk_3");
            });

            modelBuilder.Entity<TbOrderDetail>(entity =>
            {
                entity.ToTable("tbOrderDetails");

                entity.HasIndex(e => e.Id, "Id")
                    .IsUnique();

                entity.HasIndex(e => e.IdOrder, "IdOrder");

                entity.HasIndex(e => e.IdStock, "IdProduct");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Discount).HasColumnType("int(11)");

                entity.Property(e => e.IdOrder).HasColumnType("int(11)");

                entity.Property(e => e.IdStock).HasColumnType("int(11)");

                entity.Property(e => e.Quantity).HasColumnType("int(11)");

                entity.Property(e => e.Tax)
                    .HasColumnType("int(11)")
                    .HasColumnName("TAX");

                entity.HasOne(d => d.IdOrderNavigation)
                    .WithMany(p => p.TbOrderDetails)
                    .HasForeignKey(d => d.IdOrder)
                    .HasConstraintName("tbOrderDetails_ibfk_2");

                entity.HasOne(d => d.IdStockNavigation)
                    .WithMany(p => p.TbOrderDetails)
                    .HasForeignKey(d => d.IdStock)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbOrderDetails_ibfk_3");
            });

            modelBuilder.Entity<TbPayment>(entity =>
            {
                entity.ToTable("tbPayment");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<TbPicture>(entity =>
            {
                entity.ToTable("tbPictures");

                entity.HasIndex(e => e.IdProduct, "IdProduct");

                entity.HasIndex(e => e.IdStock, "IdStock");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.IdProduct).HasColumnType("int(11)");

                entity.Property(e => e.IdStock).HasColumnType("int(11)");

                entity.Property(e => e.Path).HasMaxLength(200);

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.TbPictures)
                    .HasForeignKey(d => d.IdProduct)
                    .HasConstraintName("tbPictures_ibfk_2");

                entity.HasOne(d => d.IdStockNavigation)
                    .WithMany(p => p.TbPictures)
                    .HasForeignKey(d => d.IdStock)
                    .HasConstraintName("tbPictures_ibfk_1");
            });

            modelBuilder.Entity<TbProduct>(entity =>
            {
                entity.ToTable("tbProducts");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Description).HasMaxLength(3000);

                entity.Property(e => e.DetailedDescription).HasMaxLength(3000);

                entity.Property(e => e.Material).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(50);
            });

            modelBuilder.Entity<TbStock>(entity =>
            {
                entity.ToTable("tbStock");

                entity.HasIndex(e => e.IdColor, "IdColor");

                entity.HasIndex(e => e.IdProduct, "IdProduct");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.Discount).HasColumnType("int(11)");

                entity.Property(e => e.IdColor).HasColumnType("int(11)");

                entity.Property(e => e.IdProduct).HasColumnType("int(11)");

                entity.Property(e => e.Quantity).HasColumnType("int(11)");

                entity.Property(e => e.Size).HasColumnType("int(11)");

                entity.Property(e => e.Tax)
                    .HasColumnType("int(11)")
                    .HasColumnName("TAX");

                entity.HasOne(d => d.IdColorNavigation)
                    .WithMany(p => p.TbStocks)
                    .HasForeignKey(d => d.IdColor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbStock_ibfk_2");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.TbStocks)
                    .HasForeignKey(d => d.IdProduct)
                    .HasConstraintName("tbStock_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
