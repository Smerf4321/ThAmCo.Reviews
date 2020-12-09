using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Reviews.Data;
using ThAmCo.Reviews.Models;

namespace ThAmCo.Reviews.Services
{
    public class ReviewService : IReviewService
    {
        private ThAmCoReviewsContext _context;

        public ReviewService (ThAmCoReviewsContext context)
        {
            _context = context;
        }

        public async Task CreateReviewAsync(ReviewDto review)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DoesReviewDtoExists(int reviewId)
        {
            throw new NotImplementedException();
        }

        public async Task EditReviewAsync(ReviewDto review)
        {
            throw new NotImplementedException();
        }

        public async Task<double> GetMeanRating(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<ReviewDto> GetReviewAsync(int reviewId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewListAsync(int? productId, int? userId)
        {
            throw new NotImplementedException();
        }
    }
}
