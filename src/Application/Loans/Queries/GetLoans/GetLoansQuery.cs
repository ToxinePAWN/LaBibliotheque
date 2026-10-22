namespace CleanMinimalApi.Application.Loans.Queries.GetReviews;

using Entities;
using MediatR;

public class GetLoansQuery : IRequest<List<Loan>>
{
    public Guid Id { get; set; }
}
