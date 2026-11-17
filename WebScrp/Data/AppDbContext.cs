using Microsoft.EntityFrameworkCore;
using WebScrp.Models;

namespace WebScrp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    {
        
    }

    public DbSet<Element> Elements {get; set;}
}