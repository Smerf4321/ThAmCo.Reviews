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

        Task<IEnumerable<ReviewDto>>GetReviewListAsync(int? productId, int? userId, Boolean hidden, Boolean deleted);

        Task CreateReviewAsync(ReviewDto review);

        Task DeleteReviewAsync(int reviewId, string staffEmail);

        Task DeleteReviewPIIAsync(int userId, string staffEmail);

        Task HideReviewAsync(int reviewId, string staffEmail);

        Task EditReviewAsync(ReviewDto review);

        Task RecoverHiddenReviewAsync(int reviewId, string staffEmail);

        Task RecoverDeletedReviewAsync(int reviewId, string staffEmail);

        Task<double> GetMeanRating(int productId);

        Task<bool> DoesReviewExists(int reviewId);
    }
}
