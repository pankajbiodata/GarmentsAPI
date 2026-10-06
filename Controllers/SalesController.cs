using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GarmentsAPI.Controllers
{

    [Route("api/sales")]
    public class SalesController : ControllerBase
    {
        private readonly SalesRepository _salesRepository;

        public SalesController()
        {
            // Initialize the SalesRepository with the connection string
            _salesRepository = new SalesRepository( "Server='localhost';Port=3306;Database=garmentsdb;Uid=dev;Pwd=Dev1@;");
        }

        // Get all sales orders
        [HttpGet("getAll")]
        public ActionResult<IEnumerable<SalesOrder>> GetAllSales()
        {
            var sales = _salesRepository.GetAllSales();
            return Ok(sales);
        }

        // Get a specific sales order by ID
        [HttpGet("{id}")]
        public ActionResult<SalesOrder> GetSalesOrderById(int id)
        {
            var salesOrder = _salesRepository.GetSalesOrderById(id);
            if (salesOrder == null)
            {
                return NotFound(new { message = "Sales order not found" });
            }
            return Ok(salesOrder);
        }

        // Add a new sales order
        [HttpPost("addOrder")]
        public ActionResult AddSalesOrder([FromBody] SalesOrder order)
        {
            _salesRepository.AddSalesOrder(order);
            return Ok(new { status = "success", message = "Order added successfully." });
        }

        // Update an existing sales order
        [HttpPut("updateOrder")]
        public ActionResult UpdateSalesOrder([FromBody] SalesOrder order)
        {
            _salesRepository.UpdateSalesOrder(order);
            return Ok(new { status = "success", message = "Order updated successfully." });
        }

        // Delete a sales order
        [HttpDelete("deleteOrder/{id}")]
        public ActionResult DeleteSalesOrder(int id)
        {
            _salesRepository.DeleteSalesOrder(id);
            return Ok(new { status = "success", message = "Order deleted successfully." });
        }
    }

}
