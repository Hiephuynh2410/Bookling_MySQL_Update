
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace Booking.Services
{
    public class ServicesTypeServices
    {
        private readonly DlctContext _dbContext;

        public ServicesTypeServices(DlctContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<object>> GetAllServicesTypes() {
            var ServicesTypes = await _dbContext.Servicetypes.ToListAsync();

            return ServicesTypes.Select(p => new {
                p.ServiceTypeId,
                p.Name
            }).Cast<object>().ToList();
        }

        public async Task<IActionResult> CreateServicesType(Servicetype servicetype) {
            try {
                    _dbContext.Servicetypes.Add(servicetype);
                    await _dbContext.SaveChangesAsync();

                    var CreatedServicestType = await _dbContext.Servicetypes
                        .FirstOrDefaultAsync(p => p.ServiceTypeId == servicetype.ServiceTypeId);

                    if(CreatedServicestType != null) {
                        var result = new {
                            CreatedServicestType.ServiceTypeId,
                            CreatedServicestType.Name,
                        };
                        return new OkObjectResult(result);
                    } else {
                        return new NotFoundResult();
                    }
               
            } catch (Exception ex) {
                Console.Error.WriteLine($"Error creating Service type: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> DeleteServicesType(int serviceTypeId) {
            try {
                var serviceType = await _dbContext.Servicetypes.FindAsync(serviceTypeId);
                if(serviceType == null) {
                    return new NotFoundObjectResult("serviceType not found");
                }
                _dbContext.Servicetypes.Remove(serviceType);
                await _dbContext.SaveChangesAsync();

                var deleteSuccessResponse = new {
                    Message = "serviceType deleted successfully",
                    serviceTypeId = serviceType.ServiceTypeId,
                    Name = serviceType.Name
                };
                
                return new OkObjectResult(deleteSuccessResponse);

            } catch (Exception ex) {
                Console.Error.WriteLine($"Error deleting serviceType: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
    
        public async Task<IActionResult> UpdateServicesType(int serviceTypeId, Servicetype serviceTypes) { 

            var serviceTypesUpdate = await _dbContext.Servicetypes
                .FirstOrDefaultAsync(x => x.ServiceTypeId == serviceTypeId);

            if (serviceTypesUpdate == null) {
                return new NotFoundObjectResult("Not found serviceTypes");
            }
            
            if (!string.IsNullOrWhiteSpace(serviceTypesUpdate.Name))
            {
                serviceTypesUpdate.Name = serviceTypes.Name;
            }

            _dbContext.Entry(serviceTypesUpdate).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            
            var updateSuccessResponse = new {
                Message = "service type updated successfully",
                Name = serviceTypes.Name
            };
          
            return new OkObjectResult(updateSuccessResponse);
        }
    }
}
