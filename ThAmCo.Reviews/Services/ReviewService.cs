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

        public Task CreateReviewAsync(ReviewDto review)
        {
            throw new NotImplementedException();
        }

        public Task DeleteReviewAsync(int reviewId, string staffEmail)
        {
            throw new NotImplementedException();
        }

        public Task DeleteReviewPIIAsync(int userId, string staffEmail)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DoesReviewDtoExists(int reviewId)
        {
            throw new NotImplementedException();
        }

        public Task EditReviewAsync(ReviewDto review)
        {
            throw new NotImplementedException();
        }

        public Task<double> GetMeanRating(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<ReviewDto> GetReviewAsync(int reviewId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ReviewDto>> GetReviewListAsync(int? productId, int? userId, bool hidden, bool deleted)
        {
            throw new NotImplementedException();
        }

        public Task HideReviewAsync(int reviewId, string staffEmail)
        {
            throw new NotImplementedException();
        }

        public Task RecoverDeletedReviewAsync(int reviewId, string staffEmail)
        {
            throw new NotImplementedException();
        }

        public Task RecoverHiddenReviewAsync(int reviewId, string staffEmail)
        {
            throw new NotImplementedException();
        }
    }
}
