using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GarmentsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeRepository _employeeRepository;

        // Constructor to inject the EmployeeRepository
        public EmployeeController(EmployeeRepository employeeRepository)
        {
            _employeeRepository = new EmployeeRepository( "Server='localhost';Port=3306;Database=garmentsdb;Uid=dev;Pwd=Dev1@;");
            //_employeeRepository = employeeRepository;
        }

        // Endpoint to get attendance for a specific employee by ID
        [HttpGet("GetAttendance/{employeeId}")]
        public ActionResult GetAttendance(int employeeId)
        {
            try
            {
                var attendance = _employeeRepository.GetAttendanceByEmployeeId(employeeId);

                if (attendance == 0)
                {
                    return NotFound("Attendance record not found for the specified employee.");
                }

                return Ok(new { EmployeeId = employeeId, Attendance = attendance });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // GET: api/Employee
        [HttpGet]
        public ActionResult<IEnumerable<Employee>> GetAllEmployees()
        {
            var employees = _employeeRepository.GetAllEmployees();
            if (employees == null || employees.Count() == 0)
            {
                return NotFound("No employees found.");
            }
            return Ok(employees);
        }

        // GET: api/Employee/{id}
        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployeeById(int id)
        {
            var employee = _employeeRepository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }
            return Ok(employee);
        }

        // POST: api/Employee
        [HttpPost]
        public ActionResult<Employee> AddEmployee([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Invalid employee data.");
            }
            var newEmployee = _employeeRepository.AddEmployee(employee);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = newEmployee.EmployeeID }, newEmployee);
        }

        // PUT: api/Employee/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] Employee employee)
        {
            if (employee == null || employee.EmployeeID != id)
            {
                return BadRequest("Employee data is invalid.");
            }

            var existingEmployee = _employeeRepository.GetEmployeeById(id);
            if (existingEmployee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            _employeeRepository.UpdateEmployee(employee);
            return NoContent();
        }

        // DELETE: api/Employee/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = _employeeRepository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            _employeeRepository.DeleteEmployee(id);
            return NoContent();
        }
        // Endpoint to update attendance for an employee
        [HttpPut("UpdateAttendance/{employeeId}")]
        public ActionResult UpdateAttendance(int employeeId, [FromBody] double attendanceHours)
        {
            try
            {
                if (attendanceHours < 0)
                {
                    return BadRequest("Attendance hours cannot be negative.");
                }

                _employeeRepository.UpdateAttendance(employeeId, attendanceHours);
                return Ok(new { Message = "Attendance updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Endpoint to reset attendance for a specific employee (set to 0)
        [HttpPut("ResetAttendance/{employeeId}")]
        public ActionResult ResetAttendance(int employeeId)
        {
            try
            {
                _employeeRepository.ResetAttendance(employeeId);
                return Ok(new { Message = "Attendance reset successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Endpoint to get all employees with their attendance hours
        [HttpGet("GetAllEmployeesWithAttendance")]
        public ActionResult GetAllEmployeesWithAttendance()
        {
            try
            {
                var employees = _employeeRepository.GetAllEmployeesWithAttendance();

                if (employees == null || employees.ToList().Count == 0)
                {
                    return NotFound("No employees found.");
                }

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
