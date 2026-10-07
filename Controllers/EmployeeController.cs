using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarmentsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeRepository _employeeRepository;

        public EmployeeController(
            EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // GET: api/Employee
        // Admin and Manager can view employees
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult<IEnumerable<Employee>> GetAllEmployees()
        {
            try
            {
                var employees = _employeeRepository
                    .GetAllEmployees()
                    .ToList();

                if (employees.Count == 0)
                {
                    return NotFound("No employees found.");
                }

                return Ok(employees);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // GET: api/Employee/{id}
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult<Employee> GetEmployeeById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid employee ID.");
                }

                var employee =
                    _employeeRepository.GetEmployeeById(id);

                if (employee == null)
                {
                    return NotFound(
                        $"Employee with ID {id} not found.");
                }

                return Ok(employee);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // POST: api/Employee
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult<Employee> AddEmployee(
            [FromBody] Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest(
                        "Invalid employee data.");
                }

                if (string.IsNullOrWhiteSpace(employee.Name))
                {
                    return BadRequest(
                        "Employee name is required.");
                }

                if (string.IsNullOrWhiteSpace(employee.Contact))
                {
                    return BadRequest(
                        "Employee contact is required.");
                }

                if (employee.Attendance < 0)
                {
                    return BadRequest(
                        "Attendance cannot be negative.");
                }

                if (employee.Payments < 0)
                {
                    return BadRequest(
                        "Payments cannot be negative.");
                }

                var newEmployee =
                    _employeeRepository.AddEmployee(employee);

                return CreatedAtAction(
                    nameof(GetEmployeeById),
                    new { id = newEmployee.EmployeeID },
                    newEmployee);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // PUT: api/Employee/{id}
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult UpdateEmployee(
            int id,
            [FromBody] Employee employee)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(
                        "Invalid employee ID.");
                }

                if (employee == null)
                {
                    return BadRequest(
                        "Invalid employee data.");
                }

                if (employee.EmployeeID != id)
                {
                    return BadRequest(
                        "Employee ID does not match.");
                }

                if (string.IsNullOrWhiteSpace(employee.Name))
                {
                    return BadRequest(
                        "Employee name is required.");
                }

                if (string.IsNullOrWhiteSpace(employee.Contact))
                {
                    return BadRequest(
                        "Employee contact is required.");
                }

                if (employee.Attendance < 0)
                {
                    return BadRequest(
                        "Attendance cannot be negative.");
                }

                if (employee.Payments < 0)
                {
                    return BadRequest(
                        "Payments cannot be negative.");
                }

                var existingEmployee =
                    _employeeRepository.GetEmployeeById(id);

                if (existingEmployee == null)
                {
                    return NotFound(
                        $"Employee with ID {id} not found.");
                }

                _employeeRepository.UpdateEmployee(employee);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // DELETE: api/Employee/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(
                        "Invalid employee ID.");
                }

                var employee =
                    _employeeRepository.GetEmployeeById(id);

                if (employee == null)
                {
                    return NotFound(
                        $"Employee with ID {id} not found.");
                }

                _employeeRepository.DeleteEmployee(id);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // GET: api/Employee/GetAttendance/{employeeId}
        [HttpGet("GetAttendance/{employeeId:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult GetAttendance(int employeeId)
        {
            try
            {
                if (employeeId <= 0)
                {
                    return BadRequest(
                        "Invalid employee ID.");
                }

                var employee =
                    _employeeRepository.GetEmployeeById(employeeId);

                if (employee == null)
                {
                    return NotFound(
                        $"Employee with ID {employeeId} not found.");
                }

                var attendance =
                    _employeeRepository
                        .GetAttendanceByEmployeeId(employeeId);

                return Ok(new
                {
                    EmployeeId = employeeId,
                    Attendance = attendance
                });
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // PUT: api/Employee/UpdateAttendance/{employeeId}
        [HttpPut("UpdateAttendance/{employeeId:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult UpdateAttendance(
            int employeeId,
            [FromBody] double attendanceHours)
        {
            try
            {
                if (employeeId <= 0)
                {
                    return BadRequest(
                        "Invalid employee ID.");
                }

                if (attendanceHours < 0 ||
                    attendanceHours > 24)
                {
                    return BadRequest(
                        "Attendance must be between 0 and 24 hours.");
                }

                var employee =
                    _employeeRepository.GetEmployeeById(employeeId);

                if (employee == null)
                {
                    return NotFound(
                        $"Employee with ID {employeeId} not found.");
                }

                _employeeRepository.UpdateAttendance(
                    employeeId,
                    attendanceHours);

                return Ok(new
                {
                    Message =
                        "Attendance updated successfully."
                });
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // PUT: api/Employee/ResetAttendance/{employeeId}
        [HttpPut("ResetAttendance/{employeeId:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult ResetAttendance(int employeeId)
        {
            try
            {
                if (employeeId <= 0)
                {
                    return BadRequest(
                        "Invalid employee ID.");
                }

                var employee =
                    _employeeRepository.GetEmployeeById(employeeId);

                if (employee == null)
                {
                    return NotFound(
                        $"Employee with ID {employeeId} not found.");
                }

                _employeeRepository.ResetAttendance(employeeId);

                return Ok(new
                {
                    Message =
                        "Attendance reset successfully."
                });
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An internal server error occurred.");
            }
        }

        // GET:
        // api/Employee/GetAllEmployeesWithAttendance
        [HttpGet("GetAllEmployeesWithAttendance")]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult GetAllEmployeesWithAttendance()
        {
            try
            {
                var employees =
                    _employeeRepository
                        .GetAllEmployeesWithAttendance()
                        .ToList();

                if (employees.Count == 0)
                {
                    return NotFound("No employees found.");
                }

                return Ok(employees);
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