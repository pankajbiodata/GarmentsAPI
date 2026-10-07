using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mysqlx.Expr;

namespace GarmentsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryRepository _inventoryRepository;

        public InventoryController(
            InventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        // --------------------------------------------------
        // Add Item
        // Admin + Manager
        // --------------------------------------------------

        [HttpPost("AddItem")]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult AddItem(
            [FromBody] InventoryItem item)
        {
            try
            {
                if (item == null)
                {
                    return BadRequest(
                        "Invalid item data.");
                }

                _inventoryRepository.AddItem(item);

                return Ok(new
                {
                    Message =
                        "Item added successfully."
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
        // Update Item
        // Admin + Manager
        // --------------------------------------------------

        [HttpPut("UpdateItem/{itemId:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult UpdateItem(
            int itemId,
            [FromBody] InventoryItem item)
        {
            try
            {
                if (itemId <= 0)
                {
                    return BadRequest(
                        "Invalid item ID.");
                }

                if (item == null)
                {
                    return BadRequest(
                        "Invalid item data.");
                }

                if (item.ItemID != itemId)
                {
                    return BadRequest(
                        "Item ID does not match.");
                }

                var existingItem =
                    _inventoryRepository
                        .GetInventoryReport()
                        .FirstOrDefault(
                            i => i.ItemID == itemId);

                if (existingItem == null)
                {
                    return NotFound(
                        "Item not found.");
                }

                _inventoryRepository.UpdateItem(item);

                return Ok(new
                {
                    Message =
                        "Item updated successfully."
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
        // Delete Item
        // Admin only
        // --------------------------------------------------

        [HttpDelete("DeleteItem/{itemId:int}")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteItem(
            int itemId)
        {
            try
            {
                if (itemId <= 0)
                {
                    return BadRequest(
                        "Invalid item ID.");
                }

                var existingItem =
                    _inventoryRepository
                        .GetInventoryReport()
                        .FirstOrDefault(
                            i => i.ItemID == itemId);

                if (existingItem == null)
                {
                    return NotFound(
                        "Item not found.");
                }

                _inventoryRepository.DeleteItem(itemId);

                return Ok(new
                {
                    Message =
                        "Item deleted successfully."
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
        // Get Item
        // Admin + Manager + Staff
        // --------------------------------------------------

        [HttpGet("GetItem/{itemId:int}")]
        [Authorize(Roles = "Admin,Manager,Staff")]
        public ActionResult GetItem(
            int itemId)
        {
            try
            {
                if (itemId <= 0)
                {
                    return BadRequest(
                        "Invalid item ID.");
                }

                var item =
                    _inventoryRepository
                        .GetInventoryByID(itemId);

                if (item == null)
                {
                    return NotFound(
                        "Item not found.");
                }

                return Ok(item);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // --------------------------------------------------
        // Get Inventory Report
        // Admin + Manager + Staff
        // --------------------------------------------------

        [HttpGet("GetInventoryReport")]
        [Authorize(Roles = "Admin,Manager,Staff")]
        public ActionResult GetInventoryReport()
        {
            try
            {
                var items =
                    _inventoryRepository
                        .GetInventoryReport()
                        .ToList();

                if (items.Count == 0)
                {
                    return NotFound(
                        "No items found in inventory.");
                }

                return Ok(items);
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
