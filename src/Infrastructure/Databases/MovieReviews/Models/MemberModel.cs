namespace CleanMinimalApi.Infrastructure.Databases.MovieReviews.Models;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal record MemberModel : Entity
{
    public string FirstName { get; init; }

    public string LastName { get; init; }
}
