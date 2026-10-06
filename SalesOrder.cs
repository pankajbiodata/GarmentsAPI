namespace GarmentsAPI
{
    public class SalesOrder
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }

}
