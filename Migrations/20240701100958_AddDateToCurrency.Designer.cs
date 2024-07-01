﻿// <auto-generated />
using System;
using ASP_.NET_nauka.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ASP_.NET_nauka.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20240701100958_AddDateToCurrency")]
    partial class AddDateToCurrency
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ASP_.NET_nauka.Models.Currency", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<decimal>("Change")
                        .HasColumnType("decimal(4, 2)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("High")
                        .HasColumnType("decimal(20, 10)");

                    b.Property<decimal>("Low")
                        .HasColumnType("decimal(20, 10)");

                    b.Property<int>("Measurement")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<decimal>("Sum")
                        .HasColumnType("decimal(38, 10)");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(20, 10)");

                    b.HasKey("Id");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("ASP_.NET_nauka.Models.CurrencyHistory", b =>
                {
                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("CurrencyId")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<decimal>("AvgValue")
                        .HasColumnType("decimal(20, 10)");

                    b.Property<decimal>("High")
                        .HasColumnType("decimal(20, 10)");

                    b.Property<decimal>("Low")
                        .HasColumnType("decimal(20, 10)");

                    b.HasKey("Date", "CurrencyId");

                    b.HasIndex("CurrencyId");

                    b.ToTable("CurrenciesHistory");
                });

            modelBuilder.Entity("ASP_.NET_nauka.Models.CurrencyHistory", b =>
                {
                    b.HasOne("ASP_.NET_nauka.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");
                });
#pragma warning restore 612, 618
        }
    }
}
