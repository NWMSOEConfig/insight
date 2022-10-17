using Microsoft.EntityFrameworkCore;

namespace InsightApi.Models
{
    public class Category
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public List<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
    }
    public class CategoryContext : DbContext
    {
        public CategoryContext(DbContextOptions<CategoryContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;
    }
}