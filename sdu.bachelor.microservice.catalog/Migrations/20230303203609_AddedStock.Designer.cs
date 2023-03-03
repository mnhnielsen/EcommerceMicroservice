﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using sdu.bachelor.microservice.catalog.DbContexts;

#nullable disable

namespace sdu.bachelor.microservice.catalog.Migrations
{
    [DbContext(typeof(ProductsContext))]
    [Migration("20230303203609_AddedStock")]
    partial class AddedStock
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.3");

            modelBuilder.Entity("sdu.bachelor.microservice.catalog.Entities.Brand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Brands");

                    b.HasData(
                        new
                        {
                            Id = new Guid("cd3eb3f1-0143-495b-9b90-9d1e8e46fbad"),
                            Title = "Trek"
                        },
                        new
                        {
                            Id = new Guid("e57ed7c0-4cc5-4d12-a88b-ed9f2997d918"),
                            Title = "Colnago"
                        },
                        new
                        {
                            Id = new Guid("e29de237-8203-4e3e-8066-4ac71d2c707f"),
                            Title = "Factor"
                        });
                });

            modelBuilder.Entity("sdu.bachelor.microservice.catalog.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("BrandId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(2500)
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<int>("Stock")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = new Guid("7201fd50-25b9-4b7d-99a7-b367b73222f8"),
                            BrandId = new Guid("cd3eb3f1-0143-495b-9b90-9d1e8e46fbad"),
                            Description = "High-End aero bike for the flats",
                            Price = 10000.0,
                            Stock = 5,
                            Title = "Madone"
                        },
                        new
                        {
                            Id = new Guid("d4b1d999-862d-4cf9-bcb7-b79de08768b9"),
                            BrandId = new Guid("e57ed7c0-4cc5-4d12-a88b-ed9f2997d918"),
                            Description = "Made for winning",
                            Price = 12000.0,
                            Stock = 2,
                            Title = "V4Rs"
                        },
                        new
                        {
                            Id = new Guid("ab0f5a1f-9b48-4862-8e6a-bced8d20558e"),
                            BrandId = new Guid("e29de237-8203-4e3e-8066-4ac71d2c707f"),
                            Description = "For the mountains",
                            Price = 11000.0,
                            Stock = 1,
                            Title = "Vam"
                        });
                });

            modelBuilder.Entity("sdu.bachelor.microservice.catalog.Entities.Product", b =>
                {
                    b.HasOne("sdu.bachelor.microservice.catalog.Entities.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");
                });
#pragma warning restore 612, 618
        }
    }
}
