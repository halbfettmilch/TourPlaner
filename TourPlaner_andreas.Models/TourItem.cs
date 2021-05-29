using System;


namespace TourPlaner_andreas.Models
{
    public class TourItem
   
    {   public int TourID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime CreationTime { get; set; }
        public int TourLength { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }




        public TourItem(int id, string name, string url, DateTime creationTime, int tourLength, int duration, string description)
        {
            this.TourID = id;
            this.Name = name;
            this.Url = url;
            this.CreationTime = creationTime;
            this.TourLength = tourLength;
            this.Duration = duration;
            this.Description = description;
        }
    }
}
