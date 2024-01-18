using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Services
{
    public class ProductService
    {
        private readonly DlctContext _dbContext;

        public ProductService(DlctContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<object>> GetAllProductsWithFullInfoAsync()
        {
            var products = await _dbContext.Products
                .Include(s => s.ProductType)
                .Include(s => s.Provider)
                .ToListAsync();

            return products.Select(s => new
            {
                s.ProductId,
                s.Name,
                s.Description,
                s.Price,
                s.Quantity,
                s.Image,
                s.ProductTypeId,
                s.ProviderId,
                s.CreatedAt,
                s.UpdatedAt,
                s.CreatedBy,
                s.UpdatedBy,
                s.Sold,
                ProductType = new
                {
                    Name = s.ProductType?.Name
                },
                Provider = new
                {
                    s.Provider?.Name,
                    s.Provider?.Address,
                    s.Provider?.Email,
                    s.Provider?.Phone
                },
            }).Cast<object>().ToList();
        }

        public async Task<IActionResult> CreateProductAsync(Product product)
        {
            try
            {
                if (product.ProviderId == null || product.ProductTypeId == null)
                {
                    return new BadRequestObjectResult("Provider and ProductType are required.");
                }
                _dbContext.Products.Add(product);
                await _dbContext.SaveChangesAsync();

                var createdProduct = await _dbContext.Products
                    .Include(s => s.ProductType)
                    .Include(s => s.Provider)
                    .FirstOrDefaultAsync(p => p.ProductId == product.ProductId);

                var result = new
                {
                    createdProduct.ProductId,
                    createdProduct.Name,
                    createdProduct.Description,
                    createdProduct.Price,
                    createdProduct.Quantity,
                    createdProduct.Image,
                    createdProduct.ProductTypeId,
                    createdProduct.ProviderId,
                    createdProduct.CreatedAt,
                    createdProduct.UpdatedAt,
                    createdProduct.CreatedBy,
                    createdProduct.UpdatedBy,
                    createdProduct.Sold,
                    ProductType = new
                    {
                        Name = createdProduct.ProductType?.Name
                    },
                    Provider = new
                    {
                        createdProduct.Provider?.Name,
                        createdProduct.Provider?.Address,
                        createdProduct.Provider?.Email,
                        createdProduct.Provider?.Phone
                    },
                };

                 return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating product: {ex.Message}");
                 return new StatusCodeResult(500);
            }
        }

    }
}
