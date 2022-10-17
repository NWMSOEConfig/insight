using Microsoft.EntityFrameworkCore;

namespace InsightApi.Models
{
    public class Subcategory
    {
        public long Id { get; set; }
        public string? Name { get; set; }
    }
    public class SubcategoryContext : DbContext
    {
        public SubcategoryContext(DbContextOptions<SubcategoryContext> options)
            : base(options)
        {
        }

        public DbSet<Subcategory> Subcategories { get; set; } = null!;
    }
}