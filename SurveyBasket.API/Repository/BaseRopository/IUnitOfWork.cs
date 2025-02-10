namespace SurveyBasket.API.Repository.BaseRopository;

public interface IUnitOfWork
{
	public Task<int> Commit(CancellationToken cancellationToken);
	public IBaseRepo<Poll> polls { get; }

}
