namespace CleanMinimalApi.Application.Loans.Queries.GetReviewById;

using System.Threading;
using System.Threading.Tasks;
using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;

public class GetLoanByIdHandler(ILoansRepository repository) : IRequestHandler<GetLoanByIdQuery, Loan>
{
    public async Task<Loan> Handle(GetLoanByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetLoanById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Loan);

        return result;
    }
}
