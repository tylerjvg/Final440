using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final440.Models
{
    public class Plant
    {
        public int PlantID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? TimeToPlant { get; set; }
        public string? AmountOfWater { get; set; }
        public string? AmountOfSunlight { get; set; }
        public string? TypeOfDirt { get; set; }
        public string? TypeOfFood { get; set; }
        public string? AnimalsAttractedRepelled { get; set; }
        public string? Allergies { get; set; }
        public string? Toxic { get; set; }
        public string? UsesOfPlant { get; set; }

        public string ImageSource
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Name))
                    return "placeholder.png";

                var fileName = Name.ToLower().Replace(" ", "") + ".png";
                return fileName;
            }
        }
    }

}
