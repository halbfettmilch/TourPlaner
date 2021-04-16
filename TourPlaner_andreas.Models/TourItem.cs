using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlaner_andreas.Models
{
    public class TourItem
   
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Annotation { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
