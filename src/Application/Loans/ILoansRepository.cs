namespace CleanMinimalApi.Application.Loans;

using System.Threading.Tasks;
using Entities;


public interface ILoansRepository
{
    public Task<Loan> CreateLoan(
        Guid memberId,
        Guid bookId,
        DateTime startDate,
        CancellationToken cancellationToken);

    public Task<List<Loan>> GetLoans(Guid memberId, CancellationToken cancellationToken);

    public Task<Loan> GetLoanById(Guid id, CancellationToken cancellationToken);

    public Task<bool> LoanExists(Guid id, CancellationToken cancellationToken);

    public Task<Loan> ReturnLoan(Guid id, CancellationToken cancellationToken);

    public Task<int> CountActiveLoansForBook(Guid bookId, CancellationToken cancellationToken);
}
