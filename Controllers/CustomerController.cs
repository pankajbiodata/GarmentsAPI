using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GarmentsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerRepository _customerRepository;

        // Constructor to inject the CustomerRepository
        public CustomerController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // Endpoint to add a new customer
        [HttpPost("AddCustomer")]
        public ActionResult AddCustomer([FromBody] Customer customer)
        {
            try
            {
                if (customer == null)
                {
                    return BadRequest("Invalid customer data.");
                }

                _customerRepository.AddCustomer(customer);
                return Ok(new { Message = "Customer added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Endpoint to update an existing customer
        [HttpPut("UpdateCustomer/{customerId}")]
        public ActionResult UpdateCustomer(int customerId, [FromBody] Customer customer)
        {
            try
            {
                if (customer == null || customer.CustomerID != customerId)
                {
                    return BadRequest("Customer data is invalid.");
                }

                var existingCustomer = _customerRepository.GetCustomerTransactions(customerId);
                if (existingCustomer == null)
                {
                    return NotFound("Customer not found.");
                }

                _customerRepository.UpdateCustomer(customer);
                return Ok(new { Message = "Customer updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // Endpoint to update an existing customer
        [HttpDelete("DeleteCustomer/{customerId}")]
        public ActionResult DeleteCustomer(int customerId)
        {
            try
            {
                if (customerId == null )
                {
                    return BadRequest("Customer data is invalid.");
                }

                var existingCustomer = _customerRepository.GetCustomerTransactions(customerId);
                if (existingCustomer == null)
                {
                    return NotFound("Customer not found.");
                }

                _customerRepository.DeleteCustomer(customerId);
                return Ok(new { Message = "Customer deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // Endpoint to get a customer's transactions
        [HttpGet("GetCustomerTransactions/{customerId}")]
        public ActionResult GetCustomerTransactions(int customerId)
        {
            try
            {
                var customer = _customerRepository.GetCustomerTransactions(customerId);

                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // Endpoint to get a customer's transactions
        [HttpGet("GetCustomersList")]
        public ActionResult GetCustomersList()
        {
            try
            {
                var customer = _customerRepository.GetCustomerList();

                if (customer == null)
                {
                    return NotFound("Customers not found.");
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
