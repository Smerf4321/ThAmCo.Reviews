using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Reviews.Models;

namespace ThAmCo.Reviews.Data
{
    public class ReviewsContext : DbContext
    {
        public ReviewsContext (DbContextOptions<ReviewsContext> options)
            : base(options)
        {
        }

        public DbSet<ThAmCo.Reviews.Models.ReviewDto> ReviewDto { get; set; }
    }
}
