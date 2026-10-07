using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using Dapper;

namespace GarmentsAPI
{
    public class PurchaseRepository
    {
        private readonly IDbConnection _dbConnection;

        // Constructor to initialize the connection string
        public PurchaseRepository(string connectionString)
        {
            _dbConnection = new MySqlConnection(connectionString);
        }

        // Add a new purchase order to the database
        public void AddPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            string sql = @"INSERT INTO PurchaseOrders (VendorID, Date, Amount) 
                           VALUES (@VendorID, @Date, @Amount)";
            _dbConnection.Execute(sql, purchaseOrder);
        }

        // Get a purchase order by its ID
        public PurchaseOrder GetPurchaseOrderById(int purchaseId)
        {
            string sql = "SELECT * FROM PurchaseOrders WHERE PurchaseID = @PurchaseID";
            return _dbConnection.Query<PurchaseOrder>(sql, new { PurchaseID = purchaseId }).FirstOrDefault();
        }

        // Update an existing purchase order
        public void UpdatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            string sql = @"UPDATE PurchaseOrders 
                           SET VendorID = @VendorID, Date = @Date, Amount = @Amount
                           WHERE PurchaseID = @PurchaseID";
            _dbConnection.Execute(sql, purchaseOrder);
        }

        // Get a list of all purchase orders (purchase report)
        public IEnumerable<PurchaseOrder> GetPurchaseReport()
        {
            string sql = "SELECT * FROM PurchaseOrders";
            return _dbConnection.Query<PurchaseOrder>(sql).ToList();
        }
        public void DeletePurchaseOrder(int purchaseId)
        {
            string sql =
                "DELETE FROM PurchaseOrders WHERE PurchaseID = @PurchaseID";

            _dbConnection.Execute(
                sql,
                new { PurchaseID = purchaseId }
            );
        }
    }
}
