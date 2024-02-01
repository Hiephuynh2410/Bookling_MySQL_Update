using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Services
{
    public class ProviderSerivces
    {
        private readonly DlctContext _dbContext;

        public ProviderSerivces(DlctContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<object>> GetAllProvider() {
            var provider = await _dbContext.Providers.ToListAsync();

            return provider.Select(s => new {
                s.ProviderId,
                s.Name,
                s.Address,
                s.Email,
                s.Phone
            }).Cast<object>().ToList();
        }

        public async Task<IActionResult> CreateProvider(Provider provider) { 
            try {
                
                _dbContext.Providers.Add(provider);
                await _dbContext.SaveChangesAsync();

                var CreateProvider = await _dbContext.Providers.FirstOrDefaultAsync(p => p.ProviderId == provider.ProviderId);

                if(CreateProvider != null) {
                    var result = new {
                        CreateProvider.ProviderId,
                        CreateProvider.Name,
                        CreateProvider.Phone,
                        CreateProvider.Address,

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

        public async Task<IActionResult> UpdateProvider(int providerId, Provider provider)
        {
            var providerUpdate = await _dbContext.Providers.FirstOrDefaultAsync(x => x.ProviderId == providerId);

            if (providerUpdate == null)
            {
                return new NotFoundObjectResult("Not found provider");
            }

            if (!string.IsNullOrEmpty(provider.Name))
            {
                providerUpdate.Name = provider.Name;
            }

            if (!string.IsNullOrEmpty(provider.Address))
            {
                providerUpdate.Address = provider.Address;
            }

            if (!string.IsNullOrEmpty(provider.Phone))
            {
                providerUpdate.Phone = provider.Phone;
            }

            if (!string.IsNullOrEmpty(provider.Email))
            {
                providerUpdate.Email = provider.Email;
            }

            _dbContext.Entry(providerUpdate).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            var updateSuccessResponse = new
            {
                Message = "Provider updated successfully",
                Name = providerUpdate.Name,
                Phone = providerUpdate.Phone,
                Address = providerUpdate.Address,
                Email = providerUpdate.Email,
            };

            return new OkObjectResult(updateSuccessResponse);
        }


        public async Task<IActionResult> DelteProvider(int providerId) {
            try {
                var provider = await _dbContext.Providers.FindAsync(providerId);
                if(provider == null) {
                    return new NotFoundObjectResult("provider not found");
                }
                _dbContext.Providers.Remove(provider);
                await _dbContext.SaveChangesAsync();

                var deleteSuccessResponse = new {
                    Message = "provider deleted successfully",
                    provider.ProviderId,
                };
                
                return new OkObjectResult(deleteSuccessResponse);

            } catch (Exception ex) {
                Console.Error.WriteLine($"Error deleting Providers: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
    }
}
