namespace Blessings.Data.Entities
{
    public class Set
    {
        public Set()
        {
            Assortments = new HashSet<Assortment>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int DurationInHours { get; set; }
        public virtual ICollection<Assortment> Assortments { get; set; }
    }
}
