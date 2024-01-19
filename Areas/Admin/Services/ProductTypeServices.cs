using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Services
{
    public class ProductTypeSevices
    {
        private readonly DlctContext _dbContext;

        public ProductTypeSevices(DlctContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<object>> GetAllProductTypes() {
            var productTypes = await _dbContext.Producttypes.ToListAsync();

            return productTypes.Select(p => new {
                p.ProductTypeId,
                p.Name
            }).Cast<object>().ToList();
        }

        public async Task<IActionResult> CreateProductType(Producttype productType) {
            try {
                    _dbContext.Producttypes.Add(productType);
                    await _dbContext.SaveChangesAsync();

                    var CreatedproductType = await _dbContext.Producttypes
                        .FirstOrDefaultAsync(p => p.ProductTypeId == productType.ProductTypeId);

                    if(CreatedproductType != null) {
                        var result = new {
                            CreatedproductType.ProductTypeId,
                            CreatedproductType.Name,
                        };
                        return new OkObjectResult(result);
                    } else {
                        return new NotFoundResult();
                    }
               
            } catch (Exception ex) {
                Console.Error.WriteLine($"Error creating product: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> DeleteProductType(int productTypeId) {
            try {
                var productType = await _dbContext.Producttypes.FindAsync(productTypeId);
                if(productType == null) {
                    return new NotFoundObjectResult("Product not found");
                }
                _dbContext.Producttypes.Remove(productType);
                await _dbContext.SaveChangesAsync();

                var deleteSuccessResponse = new {
                    Message = "Product deleted successfully",
                    ProductTypeId = productType.ProductTypeId,
                    Name = productType.Name
                };
                
                return new OkObjectResult(deleteSuccessResponse);

            } catch (Exception ex) {
                Console.Error.WriteLine($"Error deleting product: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
    
        // public async Task<IActionResult> UpdateProductType(int productTypeId, Producttype producttype) { 

        //     var productTypeUpdate = await _dbContext.Producttypes
        //         .FirstOrDefaultAsync(x => x.ProductTypeId == productTypeId);

        // }
    }
}
