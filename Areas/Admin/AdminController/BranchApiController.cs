using Booking.Models;
using Booking.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booking.Areas.Admin.AdminController
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchApiController : Controller
    {
        private readonly BranchServices _branchServices;

        public BranchApiController(BranchServices branchServices)
        {
            _branchServices = branchServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBranch() {
            var branches = await _branchServices.GetAllBranches();
            return Ok(branches);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBranches(Branch branches) {
            var result  = await _branchServices.CreateBranch(branches);

            if(result is OkObjectResult okResult) {
                return Ok(okResult.Value);
            } else if (result is BadRequestObjectResult badRequestObjectResult) {
                return BadRequest(badRequestObjectResult.Value);
            } 
            return StatusCode(500, "Internal Server Error");
        }

         [HttpDelete("delete/{branchId}")]
        public async Task<IActionResult> DeleteProductTypesAsync(int branchId) {
            
            var result = await _branchServices.DeleteBranch(branchId);

            if (result is OkObjectResult okResult) {
            
                return Ok(okResult.Value);
            
            } else if (result is NotFoundObjectResult notFoundResult) {
            
                return NotFound(notFoundResult.Value);
            
            } else {
            
                return StatusCode(500, "Internal Server Error");
            
            }
        }
   
        [HttpPut("update/{branchId}")]
        public async Task<IActionResult> UpdateBranch(int branchId, Branch branch) {
           
            var result = await _branchServices.UpdateBranch(branchId, branch);

            if(result is OkObjectResult okResult) {

                return Ok(okResult.Value);

            } else if (result is NotFoundObjectResult notFoundResult) { 
              
                return NotFound(notFoundResult.Value);
            
            } else {
            
                return StatusCode(500, "Internal Server Error");
            
            }
        }
    }
}
