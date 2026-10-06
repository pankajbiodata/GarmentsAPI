using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using Dapper;

namespace GarmentsAPI
{
    public class EmployeeRepository
    {
        private readonly IDbConnection _dbConnection;

        // Constructor to initialize the connection string
        public EmployeeRepository(string connectionString)
        {
            _dbConnection = new MySqlConnection(connectionString);
        }

        // Add or update attendance for an employee
        public void UpdateAttendance(int employeeId, double attendanceHours)
        {
            // SQL query to update attendance hours for the given employee
            string sql = "UPDATE Employees SET Attendance = @AttendanceHours WHERE EmployeeID = @EmployeeID";
            _dbConnection.Execute(sql, new { EmployeeID = employeeId, AttendanceHours = attendanceHours });
        }

        // Reset attendance (set to 0) for a specific employee
        public void ResetAttendance(int employeeId)
        {
            string sql = "UPDATE Employees SET Attendance = 0 WHERE EmployeeID = @EmployeeID";
            _dbConnection.Execute(sql, new { EmployeeID = employeeId });
        }

        // Get attendance record for a specific employee
        public double GetAttendanceByEmployeeId(int employeeId)
        {
            string sql = "SELECT Attendance FROM Employees WHERE EmployeeID = @EmployeeID";
            return _dbConnection.Query<double>(sql, new { EmployeeID = employeeId }).FirstOrDefault();
        }

        // Get all employees with their attendance hours
        public IEnumerable<Employee> GetAllEmployeesWithAttendance()
        {
            string sql = "SELECT EmployeeID, Name, Contact, Attendance, Payments FROM Employees";
            return _dbConnection.Query<Employee>(sql).ToList();
        }
        // Add a new employee
        public Employee AddEmployee(Employee employee)
        {
            string sql = "INSERT INTO Employees (Name, Contact, Attendance, Payments) VALUES (@Name, @Contact, @Attendance, @Payments);";
            var result = _dbConnection.Execute(sql, employee);
            return employee;
        }

        // Update an existing employee
        public void UpdateEmployee(Employee employee)
        {
            string sql = "UPDATE Employees SET Name = @Name, Contact = @Contact, Attendance = @Attendance, Payments = @Payments WHERE EmployeeID = @EmployeeID";
            _dbConnection.Execute(sql, employee);
        }

        // Delete an employee by ID
        public void DeleteEmployee(int employeeId)
        {
            string sql = "DELETE FROM Employees WHERE EmployeeID = @EmployeeID";
            _dbConnection.Execute(sql, new { EmployeeID = employeeId });
        }

        // Get employee details by ID
        public Employee GetEmployeeById(int employeeId)
        {
            string sql = "SELECT EmployeeID, Name, Contact, Attendance, Payments FROM Employees WHERE EmployeeID = @EmployeeID";
            return _dbConnection.Query<Employee>(sql, new { EmployeeID = employeeId }).FirstOrDefault();
        }

        // Get all employees
        public IEnumerable<Employee> GetAllEmployees()
        {
            string sql = "SELECT EmployeeID, Name, Contact, Attendance, Payments FROM Employees";
            return _dbConnection.Query<Employee>(sql).ToList();
        }
    }
}
