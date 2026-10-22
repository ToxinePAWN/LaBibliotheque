namespace CleanMinimalApi.Infrastructure.Databases.MovieReviews;

using System;
using Application.Authors;
using Application.Authors.Entities;
using Application.Loans;
using Application.Loans.Entities;
using Application.Movies;
using Application.Movies.Entities;
using AutoMapper;
using Extensions;
using Microsoft.EntityFrameworkCore;
using Models;

internal class EntityFrameworkBookLoansRepository : IMembersRepository, IBooksRepository, ILoansRepository
{
    private readonly MovieReviewsDbContext context;
    private readonly TimeProvider timeProvider;
    private readonly IMapper mapper;

    public EntityFrameworkBookLoansRepository(
        MovieReviewsDbContext context,
        TimeProvider timeProvider,
        IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(mapper);

        this.context = context;
        this.timeProvider = timeProvider;
        this.mapper = mapper;

        _ = this.context.Database.EnsureDeleted();
        _ = this.context.Database.EnsureCreated();
        _ = this.context.AddData();
    }

    #region Members

    public virtual async Task<List<Member>> GetMembers(CancellationToken cancellationToken)
    {
        var authors = await this.context.Authors
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return this.mapper.Map<List<Member>>(authors);
    }

    public virtual async Task<Member> GetMemberById(Guid id, CancellationToken cancellationToken)
    {
        var author = await this.context.Authors
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return this.mapper.Map<Member>(author);
        ;
    }

    public virtual async Task<bool> MemberExists(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Authors.AsNoTracking().AnyAsync(a => a.Id == id, cancellationToken);
    }

    #endregion Members

    #region Books

    public virtual async Task<List<Book>> GetBooks(CancellationToken cancellationToken)
    {
        var result = await this.context.Books
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return this.mapper.Map<List<Book>>(result);
    }

    public virtual async Task<Book> GetBookById(Guid id, CancellationToken cancellationToken)
    {
        var result = await this.context.Books
            .Where(r => r.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return this.mapper.Map<Book>(result);
    }

    public virtual async Task<bool> BookExists(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Books.AsNoTracking().AnyAsync(m => m.Id == id, cancellationToken);
    }

    #endregion Books

    #region Loans

    public async Task<Loan> CreateLoan(
        Guid memberId,
        Guid bookId,
        DateTime startDate,
        CancellationToken cancellationToken)
    {
        var loan = new LoanModel
        {
            MemberId = memberId,
            BookId = bookId,
            StartDate =  startDate,
        };

        var id = this.context.Add(loan).Entity.Id;

        _ = await this.context.SaveChangesAsync(cancellationToken);

        var result = await this.context.Loans
            .Where(r => r.Id == id)
            .AsNoTracking()
            .FirstAsync(cancellationToken);

        return this.mapper.Map<Loan>(result);
    }

    public async Task<Loan> GetLoanById(Guid id, CancellationToken cancellationToken)
    {
        var result = await this.context.Loans
            .Where(r => r.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return this.mapper.Map<Loan>(result);
    }

    public async Task<List<Loan>> GetLoans(Guid memberId, CancellationToken cancellationToken)
    {
        var result = await this.context.Loans
            .Where(r => r.MemberId == memberId)
            .Where(r => r.ReturnDate == null)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return this.mapper.Map<List<Loan>>(result);
    }

    public async Task<bool> LoanExists(Guid id, CancellationToken cancellationToken)
    {
        return await this.context.Loans.AnyAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Loan> ReturnLoan(Guid id, CancellationToken cancellationToken)
    {
        var loan = await this.context.Loans.FirstOrDefaultAsync(r => r.Id == id, cancellationToken: cancellationToken);

        loan.ReturnDate = this.timeProvider.GetUtcNow().UtcDateTime;

        _ = this.context.Update(loan);
        _ = await this.context.SaveChangesAsync(cancellationToken);

        return this.mapper.Map<Loan>(loan);
    }

    public async Task<int> CountActiveLoansForBook(Guid bookId, CancellationToken cancellationToken)
    {
        var activeLoan = await this.context.Loans.CountAsync(r => r.Id == bookId && r.ReturnDate == null, cancellationToken: cancellationToken);

        return this.mapper.Map<int>(activeLoan);
    }
    #endregion Loans
}
