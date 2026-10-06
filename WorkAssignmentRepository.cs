using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using Dapper;
namespace GarmentsAPI
{
    public class WorkAssignmentRepository
    {
        private readonly IDbConnection _dbConnection;

        public WorkAssignmentRepository(string connectionString)
        {
            _dbConnection = new MySqlConnection(connectionString);
        }

        // Process a payment for a work assignment
        public void ProcessPayment(int taskId, decimal paymentAmount)
        {
            string sql = @"UPDATE WorkAssignments 
                           SET Payment = @Payment
                           WHERE TaskID = @TaskID";
            _dbConnection.Execute(sql, new { TaskID = taskId, Payment = paymentAmount });
        }

        // Generate a receipt (this can be further expanded with detailed receipt generation)
        public string GenerateReceipt(int taskId)
        {
            string sql = "SELECT * FROM WorkAssignments WHERE TaskID = @TaskID";
            var assignment = _dbConnection.Query<WorkAssignment>(sql, new { TaskID = taskId }).FirstOrDefault();

            if (assignment == null)
                return "Work Assignment not found.";

            return $"Receipt for Task {assignment.TaskID}: Payment of {assignment.Payment:C2} processed.";
        }

        // Get the payment history for a worker (employee)
        public IEnumerable<WorkAssignment> GetPaymentHistory(int workerId)
        {
            string sql = "SELECT * FROM WorkAssignments WHERE WorkerID = @WorkerID";
            return _dbConnection.Query<WorkAssignment>(sql, new { WorkerID = workerId });
        }
    }
}
