namespace CleanMinimalApi.Infrastructure.Databases.MovieReviews.Models;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal record LoanModel : Entity
{
    public Guid MemberId { get; set; }

    public Guid BookId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? ReturnDate { get; set; }
}
