using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarmentsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VendorController : ControllerBase
    {
        private readonly VendorRepository _vendorRepository;

        public VendorController(
            VendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        // --------------------------------------------------
        // Add Vendor
        // Admin + Manager
        // --------------------------------------------------

        [HttpPost("AddVendor")]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult AddVendor(
            [FromBody] Vendor vendor)
        {
            try
            {
                if (vendor == null)
                {
                    return BadRequest(
                        "Invalid vendor data.");
                }

                if (string.IsNullOrWhiteSpace(
                    vendor.Name))
                {
                    return BadRequest(
                        "Vendor name is required.");
                }

                if (string.IsNullOrWhiteSpace(
                    vendor.Contact))
                {
                    return BadRequest(
                        "Vendor contact is required.");
                }

                _vendorRepository.AddVendor(vendor);

                return Ok(new
                {
                    Message =
                        "Vendor added successfully."
                });
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // --------------------------------------------------
        // Delete Vendor
        // Admin only
        // --------------------------------------------------

        [HttpDelete("DeleteVendor/{vendorId:int}")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteVendor(
            int vendorId)
        {
            try
            {
                if (vendorId <= 0)
                {
                    return BadRequest(
                        "Invalid vendor ID.");
                }

                var existingVendor =
                    _vendorRepository
                        .GetVendorTransactions(
                            vendorId);

                if (existingVendor == null)
                {
                    return NotFound(
                        "Vendor not found.");
                }

                _vendorRepository.DeleteVendor(
                    vendorId);

                return Ok(new
                {
                    Message =
                        "Vendor deleted successfully."
                });
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // --------------------------------------------------
        // Update Vendor
        // Admin + Manager
        // --------------------------------------------------

        [HttpPut("UpdateVendor/{vendorId:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult UpdateVendor(
            int vendorId,
            [FromBody] Vendor vendor)
        {
            try
            {
                if (vendorId <= 0)
                {
                    return BadRequest(
                        "Invalid vendor ID.");
                }

                if (vendor == null)
                {
                    return BadRequest(
                        "Invalid vendor data.");
                }

                if (vendor.VendorID != vendorId)
                {
                    return BadRequest(
                        "Vendor ID does not match.");
                }

                if (string.IsNullOrWhiteSpace(
                    vendor.Name))
                {
                    return BadRequest(
                        "Vendor name is required.");
                }

                if (string.IsNullOrWhiteSpace(
                    vendor.Contact))
                {
                    return BadRequest(
                        "Vendor contact is required.");
                }

                var existingVendor =
                    _vendorRepository
                        .GetVendorTransactions(
                            vendorId);

                if (existingVendor == null)
                {
                    return NotFound(
                        "Vendor not found.");
                }

                _vendorRepository.UpdateVendor(
                    vendor);

                return Ok(new
                {
                    Message =
                        "Vendor updated successfully."
                });
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // --------------------------------------------------
        // Get Vendor Transactions
        // Admin + Manager
        // --------------------------------------------------

        [HttpGet("GetVendorTransactions/{vendorId:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult GetVendorTransactions(
            int vendorId)
        {
            try
            {
                if (vendorId <= 0)
                {
                    return BadRequest(
                        "Invalid vendor ID.");
                }

                var vendor =
                    _vendorRepository
                        .GetVendorTransactions(
                            vendorId);

                if (vendor == null)
                {
                    return NotFound(
                        "Vendor not found.");
                }

                return Ok(vendor);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // --------------------------------------------------
        // Get Vendor List
        // Admin + Manager
        // --------------------------------------------------

        [HttpGet("GetVendorList")]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult GetVendorList()
        {
            try
            {
                var vendors =
                    _vendorRepository
                        .GetVendorList();

                if (vendors == null)
                {
                    return NotFound(
                        "Vendors not found.");
                }

                return Ok(vendors);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }
    }
}
