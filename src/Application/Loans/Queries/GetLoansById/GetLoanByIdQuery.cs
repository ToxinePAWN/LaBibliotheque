namespace CleanMinimalApi.Application.Loans.Queries.GetReviewById;

using Entities;
using MediatR;

public class GetLoanByIdQuery : IRequest<Loan>
{
    public Guid Id { get; init; }
}
