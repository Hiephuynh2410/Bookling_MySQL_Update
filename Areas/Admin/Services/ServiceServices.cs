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
                s.ServiceType,
                s.CreatedAt,
                s.UpdatedAt,
                s.CreatedBy,
                s.UpdatedBy,
                // ServiceTypes = new
                // {
                //     Name = s.ServiceType?.Name
                // },
               
            }).Cast<object>().ToList();
        }
        
        // public async Task<IActionResult> CreateServiceAsync(Service service)
        // {
        //     try
        //     {
        //         if (service.ServiceTypeId == null)
        //         {
        //             return new BadRequestObjectResult("Service type are required.");
        //         }

        //         service.CreatedAt = DateTime.Now;
        //         service.Status = true;

        //         _dbContext.Services.Add(service);
        //         await _dbContext.SaveChangesAsync();

        //         var createdService = await _dbContext.Services
        //             .Include(s => s.ServiceType)
        //             .FirstOrDefaultAsync(p => p.ServiceId == service.ServiceId);

        //         if(createdService != null) {
        //             var result = new
        //             {
        //                 createdService.ServiceId,
        //                 createdService.Name,
        //                 createdService.Price,
        //                 createdService.Status,
        //                 createdService.ServiceTypeId,
        //                 createdService.CreatedAt,
        //                 createdService.UpdatedAt,
        //                 createdService.CreatedBy,
        //                 createdService.UpdatedBy,
        //                 ServiceTypes = new
        //                 {
        //                     Name = createdService.ServiceType?.Name
        //                 },
        //             };

        //             return new OkObjectResult(result);
        //         } else {
        //             return new NotFoundResult();
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.Error.WriteLine($"Error creating Service: {ex.Message}");
        //          return new StatusCodeResult(500);
        //     }
        // }

        // public async Task<IActionResult> UpdateServiceAsync(int serviceId, Service updateModel)
        // {
        //     var ServiceToUpdate = await _dbContext.Services
        //         .Include(p => p.ServiceType)
        //         .FirstOrDefaultAsync(p => p.ServiceId == serviceId);
        //     if (ServiceToUpdate == null)
        //     {
        //         return new NotFoundObjectResult("Not found Service");
        //     }

        //     if (!string.IsNullOrWhiteSpace(updateModel.Name))
        //     {
        //         ServiceToUpdate.Name = updateModel.Name;
        //     }

        //     if (updateModel.ServiceTypeId.HasValue)
        //     {
        //         var updatedServiceType = await _dbContext.Servicetypes.FindAsync(updateModel.ServiceTypeId);
        //         if (updatedServiceType != null)
        //         {
        //             updatedServiceType.ServiceTypeId = serviceId;
        //         }
        //     }

        //     ServiceToUpdate.UpdatedAt = DateTime.Now;

        //     _dbContext.Entry(ServiceToUpdate).State = EntityState.Modified;
        //     await _dbContext.SaveChangesAsync();

        //     var updateSuccessResponse = new
        //     {
        //         Message = "Service updated successfully"
        //     };

        //     return new OkObjectResult(updateSuccessResponse);
        // }
    }
}
