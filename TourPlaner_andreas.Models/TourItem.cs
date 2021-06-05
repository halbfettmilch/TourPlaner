using System;


namespace TourPlaner_andreas.Models
{
    public class TourItem
   
    {   public int TourID { get; set; }
        public string Name { get; set; }
        public string Fromstart { get; set; }
        public string To { get; set; }
        public DateTime CreationTime { get; set; }
        public int TourLength { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }




        public TourItem(int id, string name, string fromstart,string to, DateTime creationTime, int tourLength, int duration, string description)
        {
            this.TourID = id;
            this.Name = name;
            this.Fromstart = fromstart;
            this.To = to;
            this.CreationTime = creationTime;
            this.TourLength = tourLength;
            this.Duration = duration;
            this.Description = description;
        }
    }
}
