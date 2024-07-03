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
	public DbSet<User> Users { get; set; }	
	public DbSet<Role> Roles { get; set; }
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Konfiguracja klucza głównego dla CurrencyHistory (date + CurrencyId)
		modelBuilder.Entity<CurrencyHistory>()
			.HasKey(ch => new { ch.Date, ch.CurrencyId });

		// 1:N configuration between Currency and CurrencyHistory
		modelBuilder.Entity<CurrencyHistory>()
			.HasOne(ch => ch.Currency)
			.WithMany() 
			.HasForeignKey(ch => ch.CurrencyId)
			.IsRequired();

		// 1:N configuration between Role and User
		modelBuilder.Entity<User>()
			.HasOne(u => u.Role)
			.WithMany()
			.HasForeignKey(u => u.RoleId)
			.IsRequired();


		base.OnModelCreating(modelBuilder);
	}
}

