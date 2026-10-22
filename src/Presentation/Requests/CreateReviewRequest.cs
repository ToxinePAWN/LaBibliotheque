namespace CleanMinimalApi.Presentation.Requests;

public class CreateReviewRequest
{
    public Guid MemberId { get; init; }

    public Guid BookId { get; init; }
}
