namespace SurveyBasket.API.DTOs.Vote;

public record VoteResponse
(
	string VoterName,
	DateTime VoteDate,
	IEnumerable<QuestionAnswerResponse> SelectedAnswer
);
