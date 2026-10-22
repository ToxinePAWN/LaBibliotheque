namespace CleanMinimalApi.Application.Loans.Commands.CreateReview;

using System.Threading;
using System.Threading.Tasks;
using Authors;
using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;
using Movies;

public class CreateLoanHandler(
    IMembersRepository membersRepository,
    IBooksRepository booksRepository,
    ILoansRepository loansRepository) : IRequestHandler<CreateLoanCommand, Loan>
{
    public async Task<Loan> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {
        if (!await membersRepository.MemberExists(request.MemberId, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Member);
        }

        var book = await booksRepository.GetBookById(request.BookId, cancellationToken);
        NotFoundException.ThrowIfNull(book, EntityType.Book);

        var activeLoans = await loansRepository.GetLoans(request.MemberId, cancellationToken);
        var now = DateTime.UtcNow;

        if(activeLoans.Any(al => al.StartDate.AddDays(21) < now))
        {
            throw new BusinessRulesException("The member has an active loan that he need to return.");
        }

        if(activeLoans.Count >= 3)
        {
            throw new BusinessRulesException("The member has already 3 actives loans.");
        }

        var bookActiveLoans = await loansRepository.CountActiveLoansForBook(request.BookId, cancellationToken);
        if(bookActiveLoans >= book.Copies)
        {
            throw new BusinessRulesException("Every copies of this book are currently on loan.");
        }

        return await loansRepository
            .CreateLoan(request.MemberId, request.BookId, DateTime.UtcNow, cancellationToken);
    }
}
