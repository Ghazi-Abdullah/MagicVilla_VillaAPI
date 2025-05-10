using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Model.Dto
{
    public class VillaDTO
    {
        //We need to define the properties of the villa, we will use the DTO to define the properties of the villa
        //This it will be same the repository for all the properties of the villa
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public int Occupancy { get; set; }
        public int Sqft { get; set; }
        public string Details { get; set; }
        [Required]
        public double Rate { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
    }
}
