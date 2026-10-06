using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GarmentsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseRepository _purchaseRepository;

        // Constructor to inject the PurchaseRepository
        public PurchaseController(PurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        // Endpoint to add a new purchase order
        [HttpPost("AddPurchaseOrder")]
        public ActionResult AddPurchaseOrder([FromBody] PurchaseOrder purchaseOrder)
        {
            try
            {
                if (purchaseOrder == null)
                {
                    return BadRequest("Invalid purchase order data.");
                }

                _purchaseRepository.AddPurchaseOrder(purchaseOrder);
                return Ok(new { Message = "Purchase order added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Endpoint to update an existing purchase order
        [HttpPut("UpdatePurchaseOrder/{purchaseId}")]
        public ActionResult UpdatePurchaseOrder(int purchaseId, [FromBody] PurchaseOrder purchaseOrder)
        {
            try
            {
                if (purchaseOrder == null || purchaseOrder.PurchaseID != purchaseId)
                {
                    return BadRequest("Purchase order data is invalid.");
                }

                var existingOrder = _purchaseRepository.GetPurchaseOrderById(purchaseId);
                if (existingOrder == null)
                {
                    return NotFound("Purchase order not found.");
                }

                _purchaseRepository.UpdatePurchaseOrder(purchaseOrder);
                return Ok(new { Message = "Purchase order updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Endpoint to get all purchase orders or generate a report
        [HttpGet("GetPurchaseReport")]
        public ActionResult GetPurchaseReport()
        {
            try
            {
                var purchaseOrders = _purchaseRepository.GetPurchaseReport();

                if (purchaseOrders == null || purchaseOrders.ToList().Count == 0)
                {
                    return NotFound("No purchase orders found.");
                }

                return Ok(purchaseOrders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // Endpoint to get purchase order by id
        [HttpGet("GetPurchaseById/{purchaseId}")]
        public ActionResult  GetPurchaseById(int purchaseId)
        {
            try
            {
                var purchaseOrder = _purchaseRepository.GetPurchaseOrderById(purchaseId);

                if (purchaseOrder == null )
                {
                    return NotFound("No purchase order found.");
                }

                return Ok(purchaseOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
