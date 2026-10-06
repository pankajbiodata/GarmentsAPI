using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GarmentsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryRepository _inventoryRepository;

        // Constructor to inject the InventoryRepository
        public InventoryController(InventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        // Endpoint to add a new item to the inventory
        [HttpPost("AddItem")]
        public ActionResult AddItem([FromBody] InventoryItem item)
        {
            try
            {
                if (item == null)
                {
                    return BadRequest("Invalid item data.");
                }

                _inventoryRepository.AddItem(item);
                return Ok(new { Message = "Item added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Endpoint to update an existing item in the inventory
        [HttpPut("UpdateItem/{itemId}")]
        public ActionResult UpdateItem(int itemId, [FromBody] InventoryItem item)
        {
            try
            {
                if (item == null || item.ItemID != itemId)
                {
                    return BadRequest("Item data is invalid.");
                }

                var existingItem = _inventoryRepository.GetInventoryReport().FirstOrDefault(i => i.ItemID == itemId);
                if (existingItem == null)
                {
                    return NotFound("Item not found.");
                }

                _inventoryRepository.UpdateItem(item);
                return Ok(new { Message = "Item updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Endpoint to delete an item from the inventory
        [HttpDelete("DeleteItem/{itemId}")]
        public ActionResult DeleteItem(int itemId)
        {
            try
            {
                var existingItem = _inventoryRepository.GetInventoryReport().FirstOrDefault(i => i.ItemID == itemId);
                if (existingItem == null)
                {
                    return NotFound("Item not found.");
                }

                _inventoryRepository.DeleteItem(itemId);
                return Ok(new { Message = "Item deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // Endpoint to delete an item from the inventory
        // Endpoint to get a inventory by itemId
        [HttpGet("GetItem/{itemId}")]
        public ActionResult GetItem(int itemId)
        {
            try
            {
                var item = _inventoryRepository.GetInventoryByID(itemId);

                if (item == null)
                {
                    return NotFound("Item not found.");
                }

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // Endpoint to get the inventory report (list of all items)
        [HttpGet("GetInventoryReport")]
        public ActionResult GetInventoryReport()
        {
            try
            {
                var items = _inventoryRepository.GetInventoryReport();

                if (items == null || items.ToList().Count == 0)
                {
                    return NotFound("No items found in inventory.");
                }

                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
