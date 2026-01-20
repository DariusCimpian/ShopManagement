using Microsoft.EntityFrameworkCore;
using Shop.Data.Data;
using Shop.Data.Models;

namespace Shop.Data.Services
{
    public class ProductService
    {
        private readonly ShopDbContext _context;

        public ProductService(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync(string? searchText = null, int? categoryId = null)
        {
            var query = _context.Products.AsQueryable();

            query = query.Include(p => p.Category);

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(p => p.Name.Contains(searchText));
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            return await query.ToListAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            if (product.Price <= 0)
            {
                throw new ArgumentException("Prețul trebuie să fie pozitiv.");
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

         public async Task AddCategoryAsync(Category category)
        {
             _context.Categories.Add(category);
             await _context.SaveChangesAsync();
        }
        public async Task DeleteCategoryAsync(int categoryId)
        {
             var category = await _context.Categories.FindAsync(categoryId);
              if (category != null)
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();
                }
        }
    }
}