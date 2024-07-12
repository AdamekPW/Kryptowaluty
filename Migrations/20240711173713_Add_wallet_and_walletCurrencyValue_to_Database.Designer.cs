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
    [Migration("20240711173713_Add_wallet_and_walletCurrencyValue_to_Database")]
    partial class Add_wallet_and_walletCurrencyValue_to_Database
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

            modelBuilder.Entity("ASP_.NET_nauka.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ASP_.NET_nauka.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ASP_.NET_nauka.Models.Wallet", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalValue")
                        .HasColumnType("decimal(20, 10)");

                    b.Property<DateTime>("latestUpdate")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("ASP_.NET_nauka.Models.WalletCurrencyValue", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("CurrencyId")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("UserId", "CurrencyId");

                    b.HasIndex("CurrencyId");

                    b.ToTable("WalletCurrencyValue");
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

            modelBuilder.Entity("ASP_.NET_nauka.Models.User", b =>
                {
                    b.HasOne("ASP_.NET_nauka.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("ASP_.NET_nauka.Models.Wallet", b =>
                {
                    b.HasOne("ASP_.NET_nauka.Models.User", "User")
                        .WithOne("Wallet")
                        .HasForeignKey("ASP_.NET_nauka.Models.Wallet", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ASP_.NET_nauka.Models.WalletCurrencyValue", b =>
                {
                    b.HasOne("ASP_.NET_nauka.Models.Currency", "Currency")
                        .WithMany("WalletCurrencyValues")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ASP_.NET_nauka.Models.Wallet", "Wallet")
                        .WithMany("WalletCurrencyValue")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("ASP_.NET_nauka.Models.Currency", b =>
                {
                    b.Navigation("WalletCurrencyValues");
                });

            modelBuilder.Entity("ASP_.NET_nauka.Models.User", b =>
                {
                    b.Navigation("Wallet")
                        .IsRequired();
                });

            modelBuilder.Entity("ASP_.NET_nauka.Models.Wallet", b =>
                {
                    b.Navigation("WalletCurrencyValue");
                });
#pragma warning restore 612, 618
        }
    }
}
