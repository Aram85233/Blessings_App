namespace Blessings.Data.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsBusy { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<EmployeeOrder> EmployeeOrders { get; set; }
    }
}
