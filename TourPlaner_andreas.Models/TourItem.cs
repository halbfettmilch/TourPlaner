using System;


namespace TourPlaner_andreas.Models
{
    public class TourItem
   
    {   public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime CreationTime { get; set; }

        public TourItem(int id, string name, string url, DateTime creationtime)
        {
            this.Id = id;
            this.Name = name;
            this.Url = url;
            this.CreationTime = creationtime;
        }
    }
}
