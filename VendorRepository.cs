using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using Dapper;
namespace GarmentsAPI
{
    public class VendorRepository
    {
        private readonly IDbConnection _dbConnection;

        public VendorRepository(string connectionString)
        {
            _dbConnection = new MySqlConnection(connectionString);
        }

        // Add a new vendor
        public void AddVendor(Vendor vendor)
        {
            string sql = @"INSERT INTO Vendors (Name, Contact, Address, Transactions)
                           VALUES (@Name, @Contact, @Address, @Transactions)";
            _dbConnection.Execute(sql, vendor);
        }

        // Update an existing vendor
        public void UpdateVendor(Vendor vendor)
        {
            string sql = @"UPDATE Vendors 
                           SET Name = @Name, Contact = @Contact, Address = @Address, Transactions = @Transactions 
                           WHERE VendorID = @VendorID";
            _dbConnection.Execute(sql, vendor);
        }

        // Get a vendor's transactions (you can expand this method if needed)
        public Vendor GetVendorTransactions(int vendorId)
        {
            string sql = "SELECT * FROM Vendors WHERE VendorID = @VendorID";
            return _dbConnection.Query<Vendor>(sql, new { VendorID = vendorId }).FirstOrDefault();
        }
        // Get a vendor's transactions (you can expand this method if needed)
        public List<Vendor> GetVendorList()
        {
            string sql = "SELECT * FROM Vendors";
            return _dbConnection.Query<Vendor>(sql).ToList();

        }
    }
}
