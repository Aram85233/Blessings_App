using System.ComponentModel.DataAnnotations;

namespace Blessings.Models
{
    public class SetModel
    {
        public SetModel()
        {
            Assortments = new HashSet<AssortmentModel>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int DurationInHours { get; set; }
        public virtual ICollection<AssortmentModel> Assortments { get; set; }
    }
}
