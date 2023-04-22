using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }
        [Required]
        [MaxLength(300)]
        public string Name { get; set; } = string.Empty;

        public int ProvinceId { get; set; }


        public virtual Province Province { get; set; } = new Province();
    }
}
