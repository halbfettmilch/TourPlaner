using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlaner_andreas.Models
{
    public class TourLog
    {
        public int LogId { get; set; }
        public DateTime Date { get; set; }
        public int MaxVelocity { get; set; }
        public int MinVelocity { get; set; }
        public int AvVelocity { get; set; }
        public int CaloriesBurnt { get; set; }
        public int Duration { get; set; }
        public string Author { get; set; }
        public string Comment { get; set; }
        public TourItem LogTourItem { get; set; }

        public TourLog(int logId, DateTime date,int maxVelocity,int minVelocity, int avVelocity, int caloriesBurnt, int duration, string author, string comment,TourItem loggedItem)
        {
            this.LogId = logId;
            this.Date = date;
            this.MaxVelocity = maxVelocity;
            this.MinVelocity = minVelocity;
            this.AvVelocity = avVelocity;
            this.CaloriesBurnt = caloriesBurnt;
            this.Duration = duration;
            this.Author = author;
            this.Comment = comment;
            this.LogTourItem = loggedItem;
        }
    }
}
