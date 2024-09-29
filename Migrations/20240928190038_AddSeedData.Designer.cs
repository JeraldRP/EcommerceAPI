﻿// <auto-generated />
using System;
using EcommerceProductManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EcommerceProductManagement.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240928190038_AddSeedData")]
    partial class AddSeedData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CategoryProduct", b =>
                {
                    b.Property<int>("CategoriesId")
                        .HasColumnType("int");

                    b.Property<int>("ProductsId")
                        .HasColumnType("int");

                    b.HasKey("CategoriesId", "ProductsId");

                    b.HasIndex("ProductsId");

                    b.ToTable("ProductCategories", (string)null);
                });

            modelBuilder.Entity("EcommerceProductManagement.Models.Entity.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Electronic Devices",
                            Name = "Electronics"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Various Books",
                            Name = "Books"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Apparel and Accessories",
                            Name = "Clothing"
                        });
                });

            modelBuilder.Entity("EcommerceProductManagement.Models.Entity.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CustomerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Orders");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CustomerName = "John Doe",
                            OrderDate = new DateTime(2024, 9, 19, 3, 0, 38, 382, DateTimeKind.Local).AddTicks(449)
                        },
                        new
                        {
                            Id = 2,
                            CustomerName = "Last 2Month",
                            OrderDate = new DateTime(2024, 7, 29, 3, 0, 38, 382, DateTimeKind.Local).AddTicks(490)
                        },
                        new
                        {
                            Id = 3,
                            CustomerName = "Last 3Month",
                            OrderDate = new DateTime(2024, 6, 29, 3, 0, 38, 382, DateTimeKind.Local).AddTicks(494)
                        });
                });

            modelBuilder.Entity("EcommerceProductManagement.Models.Entity.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItems");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            OrderId = 1,
                            ProductId = 1,
                            Quantity = 2,
                            UnitPrice = 999.99m
                        },
                        new
                        {
                            Id = 2,
                            OrderId = 1,
                            ProductId = 2,
                            Quantity = 1,
                            UnitPrice = 599.99m
                        });
                });

            modelBuilder.Entity("EcommerceProductManagement.Models.Entity.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("StockQuantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Gaming Laptop",
                            Name = "Laptop",
                            Price = 999.99m,
                            StockQuantity = 50
                        },
                        new
                        {
                            Id = 2,
                            Description = "Latest Model",
                            Name = "Smartphone",
                            Price = 599.99m,
                            StockQuantity = 100
                        },
                        new
                        {
                            Id = 3,
                            Description = "Plain White T-shirt",
                            Name = "T-shirt",
                            Price = 9.99m,
                            StockQuantity = 200
                        });
                });

            modelBuilder.Entity("CategoryProduct", b =>
                {
                    b.HasOne("EcommerceProductManagement.Models.Entity.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EcommerceProductManagement.Models.Entity.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EcommerceProductManagement.Models.Entity.OrderItem", b =>
                {
                    b.HasOne("EcommerceProductManagement.Models.Entity.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EcommerceProductManagement.Models.Entity.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("EcommerceProductManagement.Models.Entity.Order", b =>
                {
                    b.Navigation("OrderItems");
                });
#pragma warning restore 612, 618
        }
    }
}
