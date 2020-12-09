using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Reviews.Models;

namespace ThAmCo.Reviews.Services
{
    public interface IReviewService
    {
        Task<ReviewDto> GetReviewAsync(int reviewId);

        Task<IEnumerable<ReviewDto>>GetReviewListAsync(int? productId, int? userId);

        Task CreateReviewAsync(ReviewDto review);

        Task EditReviewAsync(ReviewDto review);

        Task DeleteReviewAsync(int reviewId);

        Task<double> GetMeanRating(int productId);

        Task<bool> DoesReviewDtoExists(int reviewId);
    }
}
