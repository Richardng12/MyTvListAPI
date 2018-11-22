using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace mytvlistapi.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new mytvlistapiContext (
                serviceProvider.GetRequiredService<DbContextOptions<mytvlistapiContext>>()))
            {
                // Look for any movies.
                if (context.TvItem.Count() > 0)
                {
                    return;   // DB has been seeded
                }

                context.TvItem.AddRange(
                    new TvItem
                    {
                        Id = 1,
                        Title = "Naruto Shippuden",
                        Url = "https://myanimelist.cdn-dena.com/images/anime/5/17407.jpg",
                        Score = "8",
                        Tags = "Ninja",
                        StartDate = "02-06-05",
                        EndDate = "03-06-06",
                        Width = "768",
                        Height = "432",
                        Progress = "3/200",
                        Priority = "High",
                        Comments = "hi",
                        Author = "Richard",
                        Authentication = "asdf234"
                    }


                );
                context.SaveChanges();
            }
        }
    }
}
