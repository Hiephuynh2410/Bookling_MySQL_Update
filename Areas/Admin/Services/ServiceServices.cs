using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Services
{
    public class ServiceServices
    {
        private readonly DlctContext _dbContext;

        public ServiceServices(DlctContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<object>> GetAllServicesWithFullInfoAsync()
        {
            var services = await _dbContext.Services
                .Include(s => s.ServiceType)
                .ToListAsync();

            return services.Select(s => new
            {
                s.ServiceId,
                s.Name,
                s.Price,
                s.Status,
                s.ServiceTypeId,
                s.CreatedAt,
                s.UpdatedAt,
                s.CreatedBy,
                s.UpdatedBy,
                ServiceType = new {
                    Name = s.ServiceType?.Name
                }
            }).Cast<object>().ToList();
        }
        
        public async Task<IActionResult> CreateServiceAsync(Service service) {
            try
            {
               
                service.CreatedAt = DateTime.Now.Date;
                service.Status = true;
                _dbContext.Services.Add(service);
                await _dbContext.SaveChangesAsync();

                var createdService = await _dbContext.Services
                    .Include(s => s.ServiceType)
                    .FirstOrDefaultAsync(p => p.ServiceId == service.ServiceId);

                if(createdService != null) {
                    var result = new
                    {
                        createdService.ServiceId,
                        createdService.Name,
                        createdService.Price,
                        createdService.ServiceTypeId,
                        createdService.CreatedAt,
                        createdService.UpdatedAt,
                        createdService.CreatedBy,
                        createdService.UpdatedBy,
                        ServiceType = new
                        {
                            Name = createdService.ServiceType?.Name
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

        public async Task<IActionResult> UpdateServiceAsync(int serviceId, Service updateModel)
        {
            var serviceToUpdate = await _dbContext.Services
                .Include(p => p.ServiceType)
                .FirstOrDefaultAsync(p => p.ServiceId == serviceId);
            
            if (serviceToUpdate == null)
            {
                return new NotFoundObjectResult("Not found Service");
            }
            if (!string.IsNullOrWhiteSpace(updateModel.Name))
            {
                serviceToUpdate.Name = updateModel.Name;
            }
       

             if (updateModel.ServiceTypeId.HasValue)
            {
                var updatedServiceType = await _dbContext.Servicetypes.FindAsync(updateModel.ServiceTypeId);
                if (updatedServiceType != null)
                {
                    serviceToUpdate.ServiceType = updatedServiceType;
                }
            }

            serviceToUpdate.UpdatedAt = DateTime.Now;
            serviceToUpdate.Price = updateModel.Price;

            _dbContext.Entry(serviceToUpdate).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            var updateSuccessResponse = new
            {
                Message = "Service updated successfully",
                name = serviceToUpdate.Name,
                price = serviceToUpdate.Price
            };

            return new OkObjectResult(updateSuccessResponse);
        }
    
        public async Task<IActionResult> DeleteServiceAsync(int serviceId)
        {
            try
            {
                var service = await _dbContext.Services.FindAsync(serviceId);

                if (service == null)
                {
                    return new NotFoundObjectResult("service not found.");
                }

                _dbContext.Services.Remove(service);
                await _dbContext.SaveChangesAsync();

                var deleteSuccessResponse = new
                {
                    Message = "service deleted successfully",
                };

                return new OkObjectResult(deleteSuccessResponse);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting service: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
    }
}
