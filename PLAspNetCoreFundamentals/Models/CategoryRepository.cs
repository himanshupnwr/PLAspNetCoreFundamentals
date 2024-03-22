namespace PLAspNetCoreFundamentals.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly PieShopDbContext _PieShopDbContext;

        public CategoryRepository(PieShopDbContext bethanysPieShopDbContext)
        {
            _PieShopDbContext = bethanysPieShopDbContext;
        }

        public IEnumerable<Category> AllCategories => _PieShopDbContext.Categories.OrderBy(p => p.CategoryName);
    }
}
