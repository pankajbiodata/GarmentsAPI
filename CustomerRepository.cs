using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using Dapper;
namespace GarmentsAPI
{
    public class CustomerRepository
    {
        private readonly IDbConnection _dbConnection;

        public CustomerRepository(string connectionString)
        {
            _dbConnection = new MySqlConnection(connectionString);
        }

        // Add a new customer
        public void AddCustomer(Customer customer)
        {
            string sql = @"INSERT INTO Customers (Name, Contact, Address, Transactions)
                           VALUES (@Name, @Contact, @Address, @Transactions)";
            _dbConnection.Execute(sql, customer);
        }

        // Update an existing customer
        public void UpdateCustomer(Customer customer)
        {
            string sql = @"UPDATE Customers 
                           SET Name = @Name, Contact = @Contact, Address = @Address, Transactions = @Transactions 
                           WHERE CustomerID = @CustomerID";
            _dbConnection.Execute(sql, customer);
        }
        // Get a customer's List (you can expand this method if needed)
        public List<Customer> GetCustomerList()
        {
            string sql = "SELECT * FROM Customers";
            return _dbConnection.Query<Customer>(sql).ToList();
        }
        // Get a customer's transactions (you can expand this method if needed)
        public Customer GetCustomerTransactions(int customerId)
        {
            string sql = "SELECT * FROM Customers WHERE CustomerID = @CustomerID";
            return _dbConnection.Query<Customer>(sql, new { CustomerID = customerId }).FirstOrDefault();
        }
        public void DeleteCustomer(int customerId)
        {
            string sql = "DELETE FROM Customers WHERE CustomerID = @CustomerID";
            _dbConnection.Execute(sql, new { CustomerID = customerId });

        }
    }
}
