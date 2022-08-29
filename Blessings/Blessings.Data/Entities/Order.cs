namespace Blessings.Data.Entities
{
    public class Order
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public string OrderNumber { get; set; }
        public int Quantity { get; set; }
        public int SetId { get; set; }
        public short Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public Set Set { get; set; }
        public virtual ICollection<EmployeeOrder> EmployeeOrders { get; set; }
    }
}
