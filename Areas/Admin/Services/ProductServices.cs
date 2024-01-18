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

                if(createdProduct != null) {
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
                } else {
                    return new NotFoundResult();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating product: {ex.Message}");
                 return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> DeleteProductAsync(int productId)
        {
            try
            {
                var product = await _dbContext.Products.FindAsync(productId);

                if (product == null)
                {
                    return new NotFoundObjectResult("Product not found.");
                }

                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();

                var deleteSuccessResponse = new
                {
                    Message = "Product deleted successfully"
                };

                return new OkObjectResult(deleteSuccessResponse);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting product: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> UpdateProductAsync(int productId, Product updateModel)
        {
            var productToUpdate = await _dbContext.Products
                .Include(p => p.ProductType)
                .Include(p => p.Provider)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
            if (productToUpdate == null)
            {
                return new NotFoundObjectResult("Not found Product");
            }

            if (!string.IsNullOrWhiteSpace(updateModel.Name))
            {
                productToUpdate.Name = updateModel.Name;
            }

            if (!string.IsNullOrWhiteSpace(updateModel.Description))
            {
                productToUpdate.Description = updateModel.Description;
            }

            if (updateModel.Price.HasValue)
            {
                productToUpdate.Price = updateModel.Price;
            }

            if (updateModel.Quantity.HasValue)
            {
                productToUpdate.Quantity = updateModel.Quantity;
            }

          

            if (updateModel.ProductTypeId.HasValue)
            {
                var updatedProductType = await _dbContext.Producttypes.FindAsync(updateModel.ProductTypeId);
                if (updatedProductType != null)
                {
                    productToUpdate.ProductType = updatedProductType;
                }
            }

            if (updateModel.ProviderId.HasValue)
            {
                var updatedProvider = await _dbContext.Providers.FindAsync(updateModel.ProviderId);
                if (updatedProvider != null)
                {
                    productToUpdate.Provider = updatedProvider;
                }
            }

            productToUpdate.UpdatedAt = DateTime.Now;
            productToUpdate.UpdatedBy = updateModel.UpdatedBy;
            
            _dbContext.Entry(productToUpdate).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            var updateSuccessResponse = new
            {
                Message = "Product updated successfully"
            };

            return new OkObjectResult(updateSuccessResponse);
        }
    }
}
