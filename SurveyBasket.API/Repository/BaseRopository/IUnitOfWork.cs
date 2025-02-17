namespace SurveyBasket.API.Repository.BaseRopository;

public interface IUnitOfWork
{
	public IBaseRepo<Poll> polls { get; }
	public IBaseRepo<Question> question { get; }

}
