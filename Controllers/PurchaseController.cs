
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mysqlx.Expr;
using Org.BouncyCastle.Asn1.Cms;

namespace GarmentsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseRepository _purchaseRepository;

        public PurchaseController(
            PurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        // --------------------------------------------------
        // Add Purchase Order
        // Admin + Manager
        // --------------------------------------------------

        [HttpPost("AddPurchaseOrder")]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult AddPurchaseOrder(
            [FromBody] PurchaseOrder purchaseOrder)
        {
            try
            {
                if (purchaseOrder == null)
                {
                    return BadRequest(
                        "Invalid purchase order data.");
                }

                if (purchaseOrder.VendorID <= 0)
                {
                    return BadRequest(
                        "Valid vendor ID is required.");
                }

                if (purchaseOrder.Amount < 0)
                {
                    return BadRequest(
                        "Amount cannot be negative.");
                }

                _purchaseRepository.AddPurchaseOrder(
                    purchaseOrder);

                return Ok(new
                {
                    Message =
                        "Purchase order added successfully."
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
        // Update Purchase Order
        // Admin + Manager
        // --------------------------------------------------

        [HttpPut("UpdatePurchaseOrder/{purchaseId:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult UpdatePurchaseOrder(
            int purchaseId,
            [FromBody] PurchaseOrder purchaseOrder)
        {
            try
            {
                if (purchaseId <= 0)
                {
                    return BadRequest(
                        "Invalid purchase ID.");
                }

                if (purchaseOrder == null)
                {
                    return BadRequest(
                        "Invalid purchase order data.");
                }

                if (purchaseOrder.PurchaseID != purchaseId)
                {
                    return BadRequest(
                        "Purchase ID does not match.");
                }

                if (purchaseOrder.VendorID <= 0)
                {
                    return BadRequest(
                        "Valid vendor ID is required.");
                }

                if (purchaseOrder.Amount < 0)
                {
                    return BadRequest(
                        "Amount cannot be negative.");
                }

                var existingOrder =
                    _purchaseRepository
                        .GetPurchaseOrderById(purchaseId);

                if (existingOrder == null)
                {
                    return NotFound(
                        "Purchase order not found.");
                }

                _purchaseRepository
                    .UpdatePurchaseOrder(
                        purchaseOrder);

                return Ok(new
                {
                    Message =
                        "Purchase order updated successfully."
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
        // Get Purchase Report
        // Admin + Manager
        // --------------------------------------------------

        [HttpGet("GetPurchaseReport")]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult GetPurchaseReport()
        {
            try
            {
                var purchaseOrders =
                    _purchaseRepository
                        .GetPurchaseReport()
                        .ToList();

                if (purchaseOrders.Count == 0)
                {
                    return NotFound(
                        "No purchase orders found.");
                }

                return Ok(purchaseOrders);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // --------------------------------------------------
        // Get Purchase By ID
        // Admin + Manager
        // --------------------------------------------------

        [HttpGet("GetPurchaseById/{purchaseId:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult GetPurchaseById(
            int purchaseId)
        {
            try
            {
                if (purchaseId <= 0)
                {
                    return BadRequest(
                        "Invalid purchase ID.");
                }

                var purchaseOrder =
                    _purchaseRepository
                        .GetPurchaseOrderById(
                            purchaseId);

                if (purchaseOrder == null)
                {
                    return NotFound(
                        "No purchase order found.");
                }

                return Ok(purchaseOrder);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // --------------------------------------------------
        // Delete Purchase Order
        // Admin only
        // --------------------------------------------------

        [HttpDelete("DeletePurchaseOrder/{purchaseId:int}")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeletePurchaseOrder(
            int purchaseId)
        {
            try
            {
                if (purchaseId <= 0)
                {
                    return BadRequest(
                        "Invalid purchase ID.");
                }

                var existingOrder =
                    _purchaseRepository
                        .GetPurchaseOrderById(
                            purchaseId);

                if (existingOrder == null)
                {
                    return NotFound(
                        "Purchase order not found.");
                }

                _purchaseRepository
                    .DeletePurchaseOrder(
                        purchaseId);

                return Ok(new
                {
                    Message =
                        "Purchase order deleted successfully."
                });
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
