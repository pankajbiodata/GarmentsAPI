using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GarmentsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorController : ControllerBase
    {
        private readonly VendorRepository _vendorRepository;

        // Constructor to inject the VendorRepository
        public VendorController(VendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        // Endpoint to add a new vendor
        [HttpPost("AddVendor")]
        public ActionResult AddVendor([FromBody] Vendor vendor)
        {
            try
            {
                if (vendor == null)
                {
                    return BadRequest("Invalid vendor data.");
                }

                _vendorRepository.AddVendor(vendor);
                return Ok(new { Message = "Vendor added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Endpoint to update an existing vendor
        [HttpPut("UpdateVendor/{vendorId}")]
        public ActionResult UpdateVendor(int vendorId, [FromBody] Vendor vendor)
        {
            try
            {
                if (vendor == null || vendor.VendorID != vendorId)
                {
                    return BadRequest("Vendor data is invalid.");
                }

                var existingVendor = _vendorRepository.GetVendorTransactions(vendorId);
                if (existingVendor == null)
                {
                    return NotFound("Vendor not found.");
                }

                _vendorRepository.UpdateVendor(vendor);
                return Ok(new { Message = "Vendor updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Endpoint to get a vendor's transactions
        [HttpGet("GetVendorTransactions/{vendorId}")]
        public ActionResult GetVendorTransactions(int vendorId)
        {
            try
            {
                var vendor = _vendorRepository.GetVendorTransactions(vendorId);

                if (vendor == null)
                {
                    return NotFound("Vendor not found.");
                }

                return Ok(vendor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // Endpoint to get a customer's transactions
        [HttpGet("GetVendorList")]
        public ActionResult GetVendorList()
        {
            try
            {
                var vendors = _vendorRepository.GetVendorList();

                if (vendors == null)
                {
                    return NotFound("Vendors not found.");
                }

                return Ok(vendors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
