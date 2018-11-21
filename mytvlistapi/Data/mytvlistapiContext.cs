using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace mytvlistapi.Models
{
    public class mytvlistapiContext : DbContext
    {
        public mytvlistapiContext (DbContextOptions<mytvlistapiContext> options)
            : base(options)
        {
        }

        public DbSet<mytvlistapi.Models.TvItem> TvItem { get; set; }
    }
}
