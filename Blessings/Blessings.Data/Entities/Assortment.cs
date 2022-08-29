namespace Blessings.Data.Entities
{
    public class Assortment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SetId { get; set; }
        public Set Set { get; set; }
    }
}
