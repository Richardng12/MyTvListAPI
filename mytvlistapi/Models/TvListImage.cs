using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace mytvlistapi.Models
{
    public class TvListImage
    {
        public string Title { get; set; }
        public string Tags { get; set; }
        public string Score { get; set; }
        public string Comments { get; set; }
        public string Author { get; set; }
        public IFormFile Image { get; set; }
    }
}
