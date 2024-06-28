using Microsoft.EntityFrameworkCore;
using ASP_.NET_nauka.Models;

namespace ASP_.NET_nauka.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Currency> Currencies { get; set; }


}

