using Microsoft.EntityFrameworkCore;
using ASP_.NET_nauka.Models;
using System.Reflection.Metadata;
using Microsoft.Extensions.Configuration;

namespace ASP_.NET_nauka.Data;

public class MyDbContext : DbContext
{
	/*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		string connectionString = "Server = (localdb)\\MSSQLLocalDB; Initial Catalog = DataBases; Integrated Security = True";
		optionsBuilder.UseSqlServer(connectionString);
	}*/
	public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
	{

	}

	public DbSet<Currency> Currencies { get; set; }
	public DbSet<CurrencyHistory> CurrenciesHistory { get; set; }
	public DbSet<User> Users { get; set; }
	public DbSet<Role> Roles { get; set; }
	public DbSet<Wallet> Wallets { get; set; }
	public DbSet<WalletCurrencyValue> WalletCurrencyValues { get; set; }
	public DbSet<ActiveOrder> ActiveOrders { get; set; }
	public DbSet<CompletedOrder> CompletedOrders { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// CurrencyHistory primary key
		modelBuilder.Entity<CurrencyHistory>()
			.HasKey(ch => new { ch.Date, ch.CurrencyId });

		// 1:N configuration between Currency and CurrencyHistory
		modelBuilder.Entity<CurrencyHistory>()
			.HasOne(ch => ch.Currency)
			.WithMany(c => c.CurrenciesHistories) 
			.HasForeignKey(ch => ch.CurrencyId)
			.IsRequired();

		// 1:N configuration between Role and User
		modelBuilder.Entity<User>()
			.HasOne(u => u.Role)
			.WithMany()
			.HasForeignKey(u => u.RoleId)
			.IsRequired();

		// Wallet primary key
		modelBuilder.Entity<Wallet>()
			.HasKey(W => W.UserId);

		// 1:1 configuration between Wallet and User
		modelBuilder.Entity<Wallet>()
			.HasOne(W => W.User)
			.WithOne(U => U.Wallet)
			.HasForeignKey<Wallet>(w => w.UserId)
			.IsRequired();

		// WalletCurrencyValue primary key
		modelBuilder.Entity<WalletCurrencyValue>()
			.HasKey(WCV => new { WCV.UserId, WCV.CurrencyId });

		// 1:N configuration between Wallet and WalletCurrencyValue
		modelBuilder.Entity<WalletCurrencyValue>()
			.HasOne(WCV => WCV.Wallet)
			.WithMany(W => W.WalletCurrencyValue)
			.HasForeignKey(WCV => WCV.UserId);

		// 1:N configuration between Currency and WalletCurrencyValue
		modelBuilder.Entity<WalletCurrencyValue>()
			.HasOne(WCV => WCV.Currency)
			.WithMany(C => C.WalletCurrencyValues)
			.HasForeignKey(WCV => WCV.CurrencyId);

		// 1:N configuration between User and ActiveOrder
		modelBuilder.Entity<ActiveOrder>()
			.HasOne(AO => AO.User)
			.WithMany(U => U.ActiveOrders)
			.HasForeignKey(AO => AO.UserId);

        // 1:N configuration between Currency and ActiveOrder
        modelBuilder.Entity<ActiveOrder>()
            .HasOne(AO => AO.Currency)
            .WithMany(C => C.ActiveOrders)
            .HasForeignKey(AO => AO.CurrencyId);

        // 1:N configuration between User and CompletedOrder
        modelBuilder.Entity<CompletedOrder>()
            .HasOne(CO => CO.User)
            .WithMany(U => U.CompletedOrders)
            .HasForeignKey(CO => CO.UserId);

        // 1:N configuration between Currency and CompletedOrder
        modelBuilder.Entity<CompletedOrder>()
            .HasOne(CO => CO.Currency)
            .WithMany(C => C.CompletedOrders)
            .HasForeignKey(CO => CO.CurrencyId);


        base.OnModelCreating(modelBuilder);
	}
}

