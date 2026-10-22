namespace CleanMinimalApi.Application.Authors.Entities;

using Loans.Entities;

public record Member(Guid Id, string FirstName, string LastName, ICollection<Loan> Loans = null);
