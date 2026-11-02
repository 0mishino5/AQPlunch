using System;
using System.Collections.Generic;
using AQPlunch.RestaurantService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AQPlunch.RestaurantService.Infrastructure.Database;

public partial class RestaurantDbContext : DbContext
{
    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<DiningTable> DiningTables { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.HasIndex(e => new { e.RestaurantId, e.Name }, "categories_restaurant_id_name_key").IsUnique();

            entity.Property(e => e.CategoryId)
                .ValueGeneratedNever()
                .HasColumnName("category_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Categories)
                .HasForeignKey(d => d.RestaurantId)
                .HasConstraintName("categories_restaurant_id_fkey");
        });

        modelBuilder.Entity<DiningTable>(entity =>
        {
            entity.HasKey(e => e.TableId).HasName("dining_tables_pkey");

            entity.ToTable("dining_tables");

            entity.HasIndex(e => e.QrToken, "dining_tables_qr_token_key").IsUnique();

            entity.HasIndex(e => new { e.RestaurantId, e.TableNumber }, "dining_tables_restaurant_id_table_number_key").IsUnique();

            entity.Property(e => e.TableId)
                .ValueGeneratedNever()
                .HasColumnName("table_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.QrToken).HasColumnName("qr_token");
            entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");
            entity.Property(e => e.TableNumber).HasColumnName("table_number");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.DiningTables)
                .HasForeignKey(d => d.RestaurantId)
                .HasConstraintName("dining_tables_restaurant_id_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("product_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsAvailable)
                .HasDefaultValue(true)
                .HasColumnName("is_available");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("products_category_id_fkey");
        });

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(e => e.RestaurantId).HasName("restaurants_pkey");

            entity.ToTable("restaurants");

            entity.Property(e => e.RestaurantId)
                .ValueGeneratedNever()
                .HasColumnName("restaurant_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
