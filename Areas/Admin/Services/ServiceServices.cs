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

    }
}
