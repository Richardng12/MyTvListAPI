using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mytvlistapi.Models
{
    public class TvItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Tags { get; set; }
        public string Score { get; set; }
        public string Progress { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Priority { get; set; }
        public string Author { get; set; }
    }
}
