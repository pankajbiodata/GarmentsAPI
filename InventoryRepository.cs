using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using Dapper;
namespace GarmentsAPI
{
    public class InventoryRepository
    {
        private readonly IDbConnection _dbConnection;

        public InventoryRepository(string connectionString)
        {
            _dbConnection = new MySqlConnection(connectionString);
        }

        // Add a new item to the inventory
        public void AddItem(InventoryItem item)
        {
            string sql = @"INSERT INTO Inventoryitems (Name, Quantity, UnitPrice) 
                           VALUES (@Name, @Quantity, @UnitPrice)";
            _dbConnection.Execute(sql, item);
        }

        // Update an existing item in the inventory
        public void UpdateItem(InventoryItem item)
        {
            string sql = @"UPDATE Inventoryitems 
                           SET Name = @Name, Quantity = @Quantity, UnitPrice = @UnitPrice 
                           WHERE ItemID = @ItemID";
            _dbConnection.Execute(sql, item);
        }

        // Delete an item from the inventory
        public void DeleteItem(int itemId)
        {
            string sql = "DELETE FROM Inventoryitems WHERE ItemID = @ItemID";
            _dbConnection.Execute(sql, new { ItemID = itemId });
        }

        // Get a list of all inventory items
        public IEnumerable<InventoryItem> GetInventoryReport()
        {
            string sql = "SELECT * FROM Inventoryitems";
            return _dbConnection.Query<InventoryItem>(sql).ToList();
        }
        public InventoryItem GetInventoryByID(int itemID)
        {
            string sql = "SELECT * FROM Inventoryitems where ItemID= @ItemID";
            return _dbConnection.Query<InventoryItem>(sql, new { ItemID = itemID }).FirstOrDefault();

        }
    }
}
