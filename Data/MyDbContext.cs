using Microsoft.EntityFrameworkCore;
using ASP_.NET_nauka.Models;
using System.Reflection.Metadata;

namespace ASP_.NET_nauka.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
        
    }

    public DbSet<Currency> Currencies { get; set; }
    public DbSet<CurrencyHistory> CurrenciesHistory { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Konfiguracja klucza głównego dla CurrencyHistory (date + CurrencyId)
		modelBuilder.Entity<CurrencyHistory>()
			.HasKey(ch => new { ch.Date, ch.CurrencyId });

		// Konfiguracja relacji jeden-do-wielu między Currency a CurrencyHistory
		modelBuilder.Entity<CurrencyHistory>()
			.HasOne(ch => ch.Currency)
			.WithMany() // Możesz określić, czy potrzebujesz navigation property w Currency
			.HasForeignKey(ch => ch.CurrencyId)
			.IsRequired();



		base.OnModelCreating(modelBuilder);
	}
}

