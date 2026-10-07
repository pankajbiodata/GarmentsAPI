using GarmentsAPI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mysqlx.Expr;

namespace GarmentsAPI.Controllers
{
    [ApiController]
    [Route("api/sales")]
    [Authorize]
    public class SalesController : ControllerBase
    {
        private readonly SalesRepository _salesRepository;

        public SalesController(
            SalesRepository salesRepository)
        {
            _salesRepository = salesRepository;
        }

        // --------------------------------------------------
        // Get All Sales Orders
        // Admin + Manager + Staff + Viewer
        // --------------------------------------------------

        [HttpGet("getAll")]
        [Authorize(Roles = "Admin,Manager,Staff,Viewer")]
        public ActionResult<IEnumerable<SalesOrder>> GetAllSales()
        {
            try
            {
                var sales =
                    _salesRepository
                        .GetAllSales()
                        .ToList();

                if (sales.Count == 0)
                {
                    return NotFound(
                        "No sales orders found.");
                }

                return Ok(sales);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // --------------------------------------------------
        // Get Sales Order By ID
        // Admin + Manager + Staff + Viewer
        // --------------------------------------------------

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Manager,Staff,Viewer")]
        public ActionResult<SalesOrder> GetSalesOrderById(
            int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(
                        "Invalid sales order ID.");
                }

                var salesOrder =
                    _salesRepository
                        .GetSalesOrderById(id);

                if (salesOrder == null)
                {
                    return NotFound(new
                    {
                        message =
                            "Sales order not found."
                    });
                }

                return Ok(salesOrder);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // --------------------------------------------------
        // Add Sales Order
        // Admin + Manager + Staff
        // --------------------------------------------------

        [HttpPost("addOrder")]
        [Authorize(Roles = "Admin,Manager,Staff")]
        public ActionResult AddSalesOrder(
            [FromBody] SalesOrder order)
        {
            try
            {
                if (order == null)
                {
                    return BadRequest(
                        "Invalid sales order data.");
                }

                _salesRepository.AddSalesOrder(order);

                return Ok(new
                {
                    status = "success",
                    message =
                        "Order added successfully."
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
        // Update Sales Order
        // Admin + Manager + Staff
        // --------------------------------------------------

        [HttpPut("updateOrder")]
        [Authorize(Roles = "Admin,Manager,Staff")]
        public ActionResult UpdateSalesOrder(
            [FromBody] SalesOrder order)
        {
            try
            {
                if (order == null)
                {
                    return BadRequest(
                        "Invalid sales order data.");
                }

                if (order.SalesOrderID <= 0)
                {
                    return BadRequest(
                        "Invalid sales order ID.");
                }

                var existingOrder =
                    _salesRepository
                        .GetSalesOrderById(
                            order.SalesOrderID);

                if (existingOrder == null)
                {
                    return NotFound(
                        "Sales order not found.");
                }

                _salesRepository.UpdateSalesOrder(order);

                return Ok(new
                {
                    status = "success",
                    message =
                        "Order updated successfully."
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
        // Delete Sales Order
        // Admin + Manager
        // --------------------------------------------------

        [HttpDelete("deleteOrder/{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult DeleteSalesOrder(
            int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(
                        "Invalid sales order ID.");
                }

                var existingOrder =
                    _salesRepository
                        .GetSalesOrderById(id);

                if (existingOrder == null)
                {
                    return NotFound(
                        "Sales order not found.");
                }

                _salesRepository.DeleteSalesOrder(id);

                return Ok(new
                {
                    status = "success",
                    message =
                        "Order deleted successfully."
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
