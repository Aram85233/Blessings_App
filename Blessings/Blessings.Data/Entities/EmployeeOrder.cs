
namespace Blessings.Data.Entities
{
    public class EmployeeOrder
    {
        public int EmployeeId { get; set; }
        public long OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public short OrderStatus { get; set; }
        public Order Order { get; set; }
        public Employee Employee { get; set; }
    }
}
