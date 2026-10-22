namespace CleanMinimalApi.Application.Loans.Commands.UpdateReview;

using Entities;
using MediatR;

public class UpdateLoanCommand : IRequest<Loan>
{
    public Guid Id { get; set; }

    public Guid AuthorId { get; init; }

    public Guid MovieId { get; init; }

    public int Stars { get; init; }
}
