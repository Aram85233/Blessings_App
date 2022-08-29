namespace Blessings.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int SetId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
