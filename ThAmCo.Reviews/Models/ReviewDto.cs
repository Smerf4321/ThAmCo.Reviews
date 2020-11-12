using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Reviews.Models
{
    public class ReviewDto
    {
        public int reviewId { get; set; }

        public int userId { get; set; }

        public int productId { get; set; }

        public string userName { get; set; }

        public string reviewContent { get; set; }

        public int reviewRating { get; set; }
    }
}
