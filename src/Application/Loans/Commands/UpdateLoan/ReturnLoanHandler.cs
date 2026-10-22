namespace CleanMinimalApi.Application.Loans.Commands.UpdateReview;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Authors;
using CleanMinimalApi.Application.Movies;
using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;

public class ReturnLoanHandler(
    IMembersRepository membersRepository,
    IBooksRepository booksRepository,
    ILoansRepository loansRepository) : IRequestHandler<UpdateLoanCommand, Loan>
{
    public async Task<Loan> Handle(UpdateLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = await loansRepository.GetLoanById(request.Id, cancellationToken);

        if (loan == null)
        {
            NotFoundException.Throw(EntityType.Loan);
        }

        if (!await membersRepository.MemberExists(request.AuthorId, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Member);
        }

        if (!await booksRepository.BookExists(request.MovieId, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Book);
        }

        return await loansRepository
            .ReturnLoan(loan.Id, cancellationToken);
    }
}
