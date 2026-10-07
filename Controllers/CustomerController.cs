using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarmentsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerRepository _customerRepository;

        public CustomerController(
            CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // =========================================================
        // ADD CUSTOMER
        // Admin, Manager, Staff
        // =========================================================

        [HttpPost("AddCustomer")]
        [Authorize(Roles = "Admin,Manager,Staff")]
        public ActionResult AddCustomer(
            [FromBody] Customer customer)
        {
            try
            {
                if (customer == null)
                {
                    return BadRequest(
                        "Invalid customer data.");
                }

                if (string.IsNullOrWhiteSpace(customer.Name))
                {
                    return BadRequest(
                        "Customer name is required.");
                }

                if (string.IsNullOrWhiteSpace(customer.Contact))
                {
                    return BadRequest(
                        "Customer contact is required.");
                }

                _customerRepository.AddCustomer(
                    customer);

                return Ok(new
                {
                    Message =
                        "Customer added successfully."
                });
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // =========================================================
        // UPDATE CUSTOMER
        // Admin, Manager, Staff
        // =========================================================

        [HttpPut("UpdateCustomer/{customerId:int}")]
        [Authorize(Roles = "Admin,Manager,Staff")]
        public ActionResult UpdateCustomer(
            int customerId,
            [FromBody] Customer customer)
        {
            try
            {
                if (customerId <= 0)
                {
                    return BadRequest(
                        "Invalid customer ID.");
                }

                if (customer == null)
                {
                    return BadRequest(
                        "Invalid customer data.");
                }

                if (customer.CustomerID != customerId)
                {
                    return BadRequest(
                        "Customer ID does not match.");
                }

                if (string.IsNullOrWhiteSpace(customer.Name))
                {
                    return BadRequest(
                        "Customer name is required.");
                }

                if (string.IsNullOrWhiteSpace(customer.Contact))
                {
                    return BadRequest(
                        "Customer contact is required.");
                }

                var existingCustomer =
                    _customerRepository
                        .GetCustomerTransactions(
                            customerId);

                if (existingCustomer == null)
                {
                    return NotFound(
                        $"Customer with ID {customerId} not found.");
                }

                _customerRepository.UpdateCustomer(
                    customer);

                return Ok(new
                {
                    Message =
                        "Customer updated successfully."
                });
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // =========================================================
        // DELETE CUSTOMER
        // Admin only
        // =========================================================

        [HttpDelete("DeleteCustomer/{customerId:int}")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteCustomer(
            int customerId)
        {
            try
            {
                if (customerId <= 0)
                {
                    return BadRequest(
                        "Invalid customer ID.");
                }

                var existingCustomer =
                    _customerRepository
                        .GetCustomerTransactions(
                            customerId);

                if (existingCustomer == null)
                {
                    return NotFound(
                        $"Customer with ID {customerId} not found.");
                }

                _customerRepository.DeleteCustomer(
                    customerId);

                return Ok(new
                {
                    Message =
                        "Customer deleted successfully."
                });
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // =========================================================
        // GET CUSTOMER TRANSACTIONS
        // Admin, Manager, Staff
        // =========================================================

        [HttpGet("GetCustomerTransactions/{customerId:int}")]
        [Authorize(Roles = "Admin,Manager,Staff")]
        public ActionResult GetCustomerTransactions(
            int customerId)
        {
            try
            {
                if (customerId <= 0)
                {
                    return BadRequest(
                        "Invalid customer ID.");
                }

                var customer =
                    _customerRepository
                        .GetCustomerTransactions(
                            customerId);

                if (customer == null)
                {
                    return NotFound(
                        $"Customer with ID {customerId} not found.");
                }

                return Ok(customer);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // =========================================================
        // GET CUSTOMER LIST
        // Admin, Manager, Staff
        // =========================================================

        [HttpGet("GetCustomersList")]
        [Authorize(Roles = "Admin,Manager,Staff")]
        public ActionResult GetCustomersList()
        {
            try
            {
                var customers =
                    _customerRepository
                        .GetCustomerList();

                if (customers == null)
                {
                    return NotFound(
                        "Customers not found.");
                }

                return Ok(customers);
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
