using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;

namespace GarmentsAPI
{
    public class EmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void UpdateAttendance(
            int employeeId,
            double attendanceHours)
        {
            const string sql = @"
                UPDATE Employees
                SET Attendance = @AttendanceHours
                WHERE EmployeeID = @EmployeeID";

            using var connection =
                new MySqlConnection(_connectionString);

            connection.Execute(
                sql,
                new
                {
                    EmployeeID = employeeId,
                    AttendanceHours = attendanceHours
                });
        }

        public void ResetAttendance(int employeeId)
        {
            const string sql = @"
                UPDATE Employees
                SET Attendance = 0
                WHERE EmployeeID = @EmployeeID";

            using var connection =
                new MySqlConnection(_connectionString);

            connection.Execute(
                sql,
                new { EmployeeID = employeeId });
        }

        public double GetAttendanceByEmployeeId(int employeeId)
        {
            const string sql = @"
                SELECT Attendance
                FROM Employees
                WHERE EmployeeID = @EmployeeID";

            using var connection =
                new MySqlConnection(_connectionString);

            return connection.QueryFirstOrDefault<double>(
                sql,
                new { EmployeeID = employeeId });
        }

        public IEnumerable<Employee>
            GetAllEmployeesWithAttendance()
        {
            const string sql = @"
                SELECT
                    EmployeeID,
                    Name,
                    Contact,
                    Attendance,
                    Payments
                FROM Employees
                ORDER BY Name";

            using var connection =
                new MySqlConnection(_connectionString);

            return connection.Query<Employee>(sql).ToList();
        }

        public Employee AddEmployee(Employee employee)
        {
            const string sql = @"
                INSERT INTO Employees
                (
                    Name,
                    Contact,
                    Attendance,
                    Payments
                )
                VALUES
                (
                    @Name,
                    @Contact,
                    @Attendance,
                    @Payments
                );

                SELECT LAST_INSERT_ID();";

            using var connection =
                new MySqlConnection(_connectionString);

            employee.EmployeeID =
                connection.ExecuteScalar<int>(
                    sql,
                    employee);

            return employee;
        }

        public void UpdateEmployee(Employee employee)
        {
            const string sql = @"
                UPDATE Employees
                SET
                    Name = @Name,
                    Contact = @Contact,
                    Attendance = @Attendance,
                    Payments = @Payments
                WHERE EmployeeID = @EmployeeID";

            using var connection =
                new MySqlConnection(_connectionString);

            connection.Execute(sql, employee);
        }

        public void DeleteEmployee(int employeeId)
        {
            const string sql = @"
                DELETE FROM Employees
                WHERE EmployeeID = @EmployeeID";

            using var connection =
                new MySqlConnection(_connectionString);

            connection.Execute(
                sql,
                new { EmployeeID = employeeId });
        }

        public Employee GetEmployeeById(int employeeId)
        {
            const string sql = @"
                SELECT
                    EmployeeID,
                    Name,
                    Contact,
                    Attendance,
                    Payments
                FROM Employees
                WHERE EmployeeID = @EmployeeID";

            using var connection =
                new MySqlConnection(_connectionString);

            return connection.QueryFirstOrDefault<Employee>(
                sql,
                new { EmployeeID = employeeId });
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            const string sql = @"
                SELECT
                    EmployeeID,
                    Name,
                    Contact,
                    Attendance,
                    Payments
                FROM Employees
                ORDER BY Name";

            using var connection =
                new MySqlConnection(_connectionString);

            return connection.Query<Employee>(sql).ToList();
        }
    }
}