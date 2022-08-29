namespace Blessings.Data.Entities
{
    public class Order
    {
        public long Id { get; set; }
        public string OrderNumber { get; set; }
        public int Quantity { get; set; }
        public int SetId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<EmployeeOrder> EmployeeOrders { get; set; }
    }
}
