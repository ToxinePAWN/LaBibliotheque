namespace CleanMinimalApi.Application.Loans.Queries.GetReviews;

using System.Threading;
using System.Threading.Tasks;
using Entities;
using MediatR;

public class GetLoansHandler(ILoansRepository repository) : IRequestHandler<GetLoansQuery, List<Loan>>
{
    public async Task<List<Loan>> Handle(GetLoansQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetLoans(request.Id, cancellationToken);
    }
}
