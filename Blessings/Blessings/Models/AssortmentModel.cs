using System.ComponentModel.DataAnnotations;

namespace Blessings.Models
{
    public class AssortmentModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int SetId { get; set; }
    }
}
