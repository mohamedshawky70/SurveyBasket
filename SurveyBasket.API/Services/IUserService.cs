using SurveyBasket.API.Common;
using SurveyBasket.API.DTOs.User;
using SurveyBasket.API.Pagination;

namespace SurveyBasket.API.Services;

public interface IUserService
{
	Task<PaginationList<UserResponse>> GetAllAsync(FilterRequest filter, CancellationToken cancellationToken = default);
	Task<OneOf<UserResponse, Errors>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
	Task<OneOf<UserResponse, Errors>> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
	Task<OneOf<UserResponse, Errors>> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken = default);
	Task<OneOf<UserResponse, Errors>> ToggleStatus(string id);
	Task<OneOf<NoContentResult, Errors>> UnLock(string id, CancellationToken cancellationToken);
}
