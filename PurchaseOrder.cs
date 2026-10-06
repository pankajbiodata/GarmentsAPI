namespace GarmentsAPI
{
    public class PurchaseOrder
    {
        public int PurchaseID { get; set; }  // Corresponds to PurchaseID in the table
        public int VendorID { get; set; }    // Corresponds to VendorID in the table
        public DateTime Date { get; set; }   // Corresponds to Date in the table
        public decimal Amount { get; set; }  // Corresponds to Amount in the table
    }
}
