
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Services
{
    public class ServicesTypeServices
    {
        private readonly DlctContext _dbContext;

        public ServicesTypeServices(DlctContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<object>> GetAllServicesType()
        {
            var serviceTypes = await _dbContext.Servicetypes
                .ToListAsync();

            return serviceTypes.Select(s => new
            {
                s.ServiceTypeId,
                s.Name,
            }).Cast<object>().ToList();
        }

        public async Task<IActionResult> CreateServiceType(Servicetype servicetype)
        {
            try
            {
                _dbContext.Servicetypes.Add(servicetype);
                await _dbContext.SaveChangesAsync();

                var createdServicetype = await _dbContext.Servicetypes
                    .FirstOrDefaultAsync(p => p.ServiceTypeId == servicetype.ServiceTypeId);

                if(createdServicetype != null) {
                    var result = new
                    {
                        createdServicetype.ServiceTypeId,
                        createdServicetype.Name,
                    };

                    return new OkObjectResult(result);
                } else {
                    return new NotFoundResult();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating service types: {ex.Message}");
                 return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> DeleteService(int ServiceTypeId)
        {
            try
            {
                var ServiceType = await _dbContext.Servicetypes.FindAsync(ServiceTypeId);

                if (ServiceType == null)
                {
                    return new NotFoundObjectResult("Service type not found.");
                }

                _dbContext.Servicetypes.Remove(ServiceType);
                await _dbContext.SaveChangesAsync();

                var deleteSuccessResponse = new
                {
                    Message = "Service Type deleted successfully",
                };

                return new OkObjectResult(deleteSuccessResponse);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting product: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> UpdateServiceTypeAsync(int ServiceTypeId, Servicetype updateModel)
        {
            var ServicetypeToUpdate = await _dbContext.Servicetypes
                .FirstOrDefaultAsync(p => p.ServiceTypeId == ServiceTypeId);
            if (ServicetypeToUpdate == null)
            {
                return new NotFoundObjectResult("Not found Service type");
            }

            if (!string.IsNullOrWhiteSpace(updateModel.Name))
            {
                ServicetypeToUpdate.Name = updateModel.Name;
            }

            
            _dbContext.Entry(ServicetypeToUpdate).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            var updateSuccessResponse = new
            {
                Message = "Service type updated successfully"
            };

            return new OkObjectResult(updateSuccessResponse);
        }
    }
}
