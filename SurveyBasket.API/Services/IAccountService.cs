using SurveyBasket.API.DTOs.Authentication;
using SurveyBasket.API.DTOs.User;

namespace SurveyBasket.API.Services;

public interface IAccountService
{
	Task<UserProfileResponse> Profile(string userId, CancellationToken cancellationToken = default);
	Task<Successes> UpdateAsync(string userId, UpdateProfileRequest request, CancellationToken cancellationToken = default);
	Task<OneOf<Successes, Errors>> ChangePasswordAsync(string userId, ChangePasswordRequest request, CancellationToken cancellationToken = default);
}
