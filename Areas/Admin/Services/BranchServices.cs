using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Booking.Services
{
    public class BranchServices
    {
        private readonly DlctContext _dbContext;

        public BranchServices(DlctContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<object>> GetAllBranches() {
            var branches = await _dbContext.Branches
                    .Include(s => s.Staff)
                    .ToListAsync();

            return branches.Select(s => new {
                s.BranchId,
                s.Address,
                s.Hotline,
                Staff = s.Staff.Select(staff => new {
                    staff.StaffId,
                    staff.Name,
                    staff.Username,
                    staff.Phone,
                    staff.Address,
                    staff.Email,
                    staff.Status,
                    staff.IsDisabled,
                    staff.RoleId,
                    staff.CreatedAt,
                    staff.UpdatedAt,
                    staff.CreatedBy,
                    staff.UpdatedBy,
                    staff.BranchId
                }).ToList()
            }).Cast<object>().ToList();
        }
    
        public async Task<IActionResult> CreateBranch(Branch branch) {
            try {
                _dbContext.Branches.Add(branch);
                await _dbContext.SaveChangesAsync();

                var CreatedBranch = await _dbContext.Branches
                        .Include(s => s.Staff)
                        .FirstOrDefaultAsync(p => p.BranchId == branch.BranchId);

                if(CreatedBranch != null) {
                    var result = new {
                        CreatedBranch.BranchId,
                        CreatedBranch.Address,
                        CreatedBranch.Hotline,
                        Staff = CreatedBranch.Staff.Select( s => new {
                            StaffId = s.StaffId,
                            Name = s.Name,
                            Username = s.Username,
                            Phone = s.Phone,
                            Address = s.Address,
                            Email = s.Email,
                            IsDisabled = s.IsDisabled,
                            Status = s.Status,
                            CreatedAt = s.CreatedAt,
                            RoleId = s.RoleId,
                            CreatedBy = s.CreatedBy,
                            UpdatedAt = s.UpdatedAt,
                            BranchId = s.BranchId,
                            UpdatedBy = s.UpdatedBy,
                        }).ToList()
                    };
                    return new OkObjectResult(CreatedBranch);
                } else {
                    return new NotFoundResult();
                }

            } catch (Exception ex) {
                Console.Error.WriteLine($"Error creating product: {ex.Message}");
                return new StatusCodeResult(500); 
            }
        }
    
        public async Task<IActionResult> DeleteBranch(int branchId) {
            try {
                var branch = await _dbContext.Branches.FindAsync(branchId);
                if(branch == null) {
                    return new NotFoundObjectResult("branch not found");
                }
                
                _dbContext.Branches.Remove(branch);
                await _dbContext.SaveChangesAsync();

                var deleteSuccessResponse = new {
                    message = "Delete Branch sucess",
                    branchId = branch.BranchId,
                    address = branch.Address,
                    hotline = branch.Hotline
                };

                return new OkObjectResult(deleteSuccessResponse);
            } catch (Exception ex) {
                Console.Error.WriteLine($"Error deleting product: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> UpdateBranch(int brnachId, Branch branch) { 

            var branchUpdate = await _dbContext.Branches
                .FirstOrDefaultAsync(x => x.BranchId == brnachId);

            if (branchUpdate == null) {
                return new NotFoundObjectResult("Not found branch");
            }
            
            if (!string.IsNullOrWhiteSpace(branch.Address))
            {
                branchUpdate.Address = branch.Address;
            }

            _dbContext.Entry(branchUpdate).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            
            var updateSuccessResponse = new {
                Message = "branch updated successfully",
                address = branch.Address,
                hotline = branch.Hotline
            };
          
            return new OkObjectResult(updateSuccessResponse);
        }
    }
}
