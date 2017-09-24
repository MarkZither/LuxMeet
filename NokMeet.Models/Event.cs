using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LuxMeet.DbModels
{
    public class Event
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        public string location { get; set; }
        public DateTime start_at { get; set; }
        public DateTime end_at { get; set; }
        public string body { get; set; }
        public DateTime created_at { get; set; }
        public int? category_id { get; set; }
        public bool _public { get; set; }
        public string image_url { get; set; }
        public string public_url { get; set; }
        public bool archived { get; set; }
    }
}
