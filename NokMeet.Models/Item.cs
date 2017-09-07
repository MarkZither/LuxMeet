using System;
using System.ComponentModel.DataAnnotations;

namespace LuxMeet.Models
{

    public class Item
    {
        public string id { get; set; }
        [Required]
        public string title { get; set; }
        public string summary { get; set; }
        public Active[] active { get; set; }
        public Alt alt { get; set; }
        public string embed { get; set; }
        public DateTime published { get; set; }
        public DateTime updated { get; set; }
        public Author author { get; set; }
        public Group group { get; set; }
        public string location { get; set; }
        public string date_string { get; set; }
    }

    public class Alt
    {
        public int id { get; set; }
        public string texthtml { get; set; }
    }

    public class Author
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email_address { get; set; }
    }

    public class Group
    {
        public int id { get; set; }
        public string name { get; set; }
        public Logo logo { get; set; }
    }

    public class Logo
    {
        public int id { get; set; }
        public string url { get; set; }
    }

    public class Active
    {
        public int id { get; set; }
        public DateTime active { get; set; }
        public string time_tbc { get; set; }
        public DateTime ends { get; set; }
        public string approximate_description { get; set; }
        public int week_of_year_number { get; set; }
        public int day_of_week_number { get; set; }
    }

}
