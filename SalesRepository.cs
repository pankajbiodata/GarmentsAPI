using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using Dapper;


namespace GarmentsAPI
{ 
public class SalesRepository
{
    private readonly string _connectionString ;

    // Constructor to initialize the connection string
    public SalesRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Method to retrieve all sales orders
    public IEnumerable<SalesOrder> GetAllSales()
    {
        using (IDbConnection db = new MySqlConnection(_connectionString))
        {
            return db.Query<SalesOrder>("SELECT * FROM Sales").ToList();
        }
    }

    // Method to retrieve a single sales order by ID
    public SalesOrder GetSalesOrderById(int orderId)
    {
        using (IDbConnection db = new MySqlConnection(_connectionString))
        {
            return db.QuerySingleOrDefault<SalesOrder>("SELECT * FROM Sales WHERE OrderId = @OrderId", new { OrderId = orderId });
        }
    }

    // Method to add a new sales order
    public void AddSalesOrder(SalesOrder order)
    {
        using (IDbConnection db = new MySqlConnection(_connectionString))
        {
            string query = "INSERT INTO Sales (CustomerId, Date, Amount) VALUES (@CustomerId, @Date, @Amount)";
            db.Execute(query, order);
        }
    }

    // Method to update an existing sales order
    public void UpdateSalesOrder(SalesOrder order)
    {
        using (IDbConnection db = new MySqlConnection(_connectionString))
        {
            string query = "UPDATE Sales SET CustomerId = @CustomerId, Date = @Date, Amount = @Amount WHERE OrderId = @OrderId";
            db.Execute(query, order);
        }
    }

    // Method to delete a sales order by ID
    public void DeleteSalesOrder(int orderId)
    {
        using (IDbConnection db = new MySqlConnection(_connectionString))
        {
            string query = "DELETE FROM Sales WHERE OrderId = @OrderId";
            db.Execute(query, new { OrderId = orderId });
        }
    }
}

}
