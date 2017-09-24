using System;

namespace LuxMeet.ViewModels
{

    public class Event
    {
        public int id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public DateTime start_at { get; set; }
        public DateTime end_at { get; set; }
        public string body { get; set; }
        public DateTime created_at { get; set; }
        public object category_id { get; set; }
        public bool _public { get; set; }
        public string image_url { get; set; }
        public string public_url { get; set; }
        public bool archived { get; set; }
    }

}
