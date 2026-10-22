namespace CleanMinimalApi.Infrastructure.Databases.MovieReviews.Models;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal record BookModel : Entity
{
    public string Title { get; init; }
    public string Author { get; init; }
    public int Copies { get; init; }
}
