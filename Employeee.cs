namespace GarmentsAPI
{
    public class Employee
    {
        public int EmployeeID { get; set; }   // Primary Key for Employee
        public string Name { get; set; }      // Employee's Name
        public string Contact { get; set; }   // Employee's Contact number
        public double Attendance { get; set; } // Total attendance in hours
        public decimal Payments { get; set; } // Payments made to the Employee (Salary or other payments)
    }
}
