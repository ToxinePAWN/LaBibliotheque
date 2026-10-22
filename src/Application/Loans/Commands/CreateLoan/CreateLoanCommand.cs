namespace CleanMinimalApi.Application.Loans.Commands.CreateReview;

using Entities;
using MediatR;

public class CreateLoanCommand : IRequest<Loan>
{
    public Guid MemberId { get; init; }

    public Guid BookId { get; init; }
}
