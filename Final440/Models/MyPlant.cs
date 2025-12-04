using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final440.Models
{
    public class MyPlant
    {
        public int MyPlantID { get; set; }
        public int PlantID { get; set; }

        public string? CustomName { get; set; }
        public string? Instructions { get; set; }
        public string? Notes { get; set; }

        public string PlantName { get; set; } = string.Empty;

        public Plant? Plant { get; set; }

        public string DisplayName =>
            string.IsNullOrWhiteSpace(CustomName) ? PlantName : CustomName;
    }
}
