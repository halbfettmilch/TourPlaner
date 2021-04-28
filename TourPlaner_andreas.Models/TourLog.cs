using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlaner_andreas.Models
{
    public class TourLog
    {
        public int Id { get; set; }
        public string LogText { get; set; }
        public TourItem LogTourItem { get; set; }

        public TourLog(int id, string logText, TourItem loggedItem)
        {
            this.Id = id;
            this.LogText = logText;
            this.LogTourItem = loggedItem;
        }
    }
}
