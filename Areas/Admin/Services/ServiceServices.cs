using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Booking.Services
{
    public class ServiceServices
    {
        private readonly DlctContext _dbContext;

        public ServiceServices(DlctContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<object>> GetAllServices() { 
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
                s.CreatedBy,
                s.UpdatedAt,
                s.UpdatedBy,
                ServiceType = new
                {
                    id = s.ServiceType?.ServiceTypeId,
                    Name = s.ServiceType?.Name
                },
               
            }).Cast<object>().ToList();
        }
        public async Task<IActionResult> CreateServiceAsync(Service service)
        {
            try
            {
                if (service.ServiceTypeId == null)
                {
                    return new BadRequestObjectResult("service type are required.");
                }

                DateTime currentDate = DateTime.Now;
                
                service.CreatedAt = currentDate;

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
                        createdService.Status,
                        createdService.ServiceTypeId,
                        createdService.CreatedAt,
                        createdService.UpdatedAt,
                        createdService.CreatedBy,
                        createdService.UpdatedBy,
                        Service = new
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
                Console.Error.WriteLine($"Error creating service: {ex.Message}");
                 return new StatusCodeResult(500);
            }
        }
    
        public async Task<IActionResult> DeleteServiceAsync(int ServiceId)
        {
            try
            {
                var Service = await _dbContext.Services.FindAsync(ServiceId);

                if (Service == null)
                {
                    return new NotFoundObjectResult("Service not found.");
                }

                _dbContext.Services.Remove(Service);
                await _dbContext.SaveChangesAsync();

                var deleteSuccessResponse = new
                {
                    Message = "Service deleted successfully",
                };

                return new OkObjectResult(deleteSuccessResponse);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting Service: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> DeleteAllServiceAsync(int ServiceId)
        {
            var serviToDelete = await _dbContext.Services.FindAsync(ServiceId);

            if (serviToDelete == null)
            {
                return new NotFoundObjectResult("Not found service");
            }

            _dbContext.Services.Remove(serviToDelete);
            await _dbContext.SaveChangesAsync();

            var deleteSuccessResponse = new
            {
                Message = "service deleted successfully"
            };

            return new OkObjectResult(deleteSuccessResponse);
        }

    }
}
