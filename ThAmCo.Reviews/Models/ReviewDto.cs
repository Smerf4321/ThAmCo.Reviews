using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Reviews.Models
{
    public class ReviewDto
    {
        [Key]
        public int reviewId { get; set; }

        public int userId { get; set; }

        public int productId { get; set; }

        public string userName { get; set; }

        public string reviewContent { get; set; }

        public int reviewRating { get; set; }

        public DateTime dateCreated { get; set; }

        public bool hidden { get; set; }
    }
}
