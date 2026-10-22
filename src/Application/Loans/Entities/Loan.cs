namespace CleanMinimalApi.Application.Loans.Entities;

public record Loan(
    Guid Id,
    Guid MemberId,
    Guid BookId,
    DateTime StartDate,
    DateTime? ReturnDate);
