namespace SurveyBasket.API.DTOs.Vote;

public record VotesPerQuestionResponse
(
	string Question,
	IEnumerable<VotesPerAnswerResponse> SelectedAnswers
);
