﻿// <auto-generated />
using DiasDataAccessLayer.EntityDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DiasDataAccessLayer.Migrations
{
    [DbContext(typeof(ProductDbContext))]
    [Migration("20240526134917_InitialMigration1")]
    partial class InitialMigration1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DiasShared.Entities.Models.PriceHistory", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("PriceTypeId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("PriceTypeId");

                    b.HasIndex("ProductId");

                    b.ToTable("PricesHistory");
                });

            modelBuilder.Entity("DiasShared.Entities.Models.PriceType", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<int>("PriceTypes")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("PriceTypes");

                    b.HasData(
                        new
                        {
                            id = 1,
                            PriceTypes = 0
                        },
                        new
                        {
                            id = 2,
                            PriceTypes = 1
                        });
                });

            modelBuilder.Entity("DiasShared.Entities.Models.Product", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("StockQuantity")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("DiasShared.Entities.Models.PriceHistory", b =>
                {
                    b.HasOne("DiasShared.Entities.Models.PriceType", "PriceType")
                        .WithMany("PriceHistories")
                        .HasForeignKey("PriceTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DiasShared.Entities.Models.Product", "Product")
                        .WithMany("PriceHistories")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PriceType");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("DiasShared.Entities.Models.PriceType", b =>
                {
                    b.Navigation("PriceHistories");
                });

            modelBuilder.Entity("DiasShared.Entities.Models.Product", b =>
                {
                    b.Navigation("PriceHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
