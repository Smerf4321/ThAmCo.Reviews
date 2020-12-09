using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Reviews.Models;

namespace ThAmCo.Reviews.Data
{
    public class ThAmCoReviewsContext : DbContext
    {
        public ThAmCoReviewsContext (DbContextOptions<ThAmCoReviewsContext> options)
            : base(options)
        {
        }

        public DbSet<ThAmCo.Reviews.Models.Review> Review { get; set; }
    }
}
