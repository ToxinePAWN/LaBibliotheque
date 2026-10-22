namespace CleanMinimalApi.Application.Movies.Entities;

using Loans.Entities;

public record Book(Guid Id, string Title, string Author, int Copies);
