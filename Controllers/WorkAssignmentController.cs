using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GarmentsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkAssignmentController : ControllerBase
    {
        private readonly WorkAssignmentRepository _workAssignmentRepository;

        // Constructor to inject the WorkAssignmentRepository
        public WorkAssignmentController(WorkAssignmentRepository workAssignmentRepository)
        {
            _workAssignmentRepository = workAssignmentRepository;
        }

        // Endpoint to process a payment for a work assignment
        [HttpPost("ProcessPayment")]
        public ActionResult ProcessPayment(int taskId, decimal paymentAmount)
        {
            try
            {
                if (paymentAmount <= 0)
                {
                    return BadRequest("Invalid payment amount.");
                }

                _workAssignmentRepository.ProcessPayment(taskId, paymentAmount);
                return Ok(new { Message = "Payment processed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Endpoint to generate a receipt for a work assignment
        [HttpGet("GenerateReceipt/{taskId}")]
        public ActionResult GenerateReceipt(int taskId)
        {
            try
            {
                var receipt = _workAssignmentRepository.GenerateReceipt(taskId);

                if (receipt == "Work Assignment not found.")
                {
                    return NotFound("Work Assignment not found.");
                }

                return Ok(new { Receipt = receipt });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Endpoint to get a worker's payment history
        [HttpGet("GetPaymentHistory/{workerId}")]
        public ActionResult GetPaymentHistory(int workerId)
        {
            try
            {
                var paymentHistory = _workAssignmentRepository.GetPaymentHistory(workerId);

                if (paymentHistory == null)
                {
                    return NotFound("No payment history found for the worker.");
                }

                return Ok(paymentHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
