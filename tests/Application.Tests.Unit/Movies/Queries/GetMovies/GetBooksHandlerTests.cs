namespace CleanMinimalApi.Application.Tests.Unit.Movies.Queries.GetMovies;

using System.Threading;
using System.Threading.Tasks;
using Application.Movies;
using Application.Movies.Entities;
using Application.Movies.Queries.GetMovies;
using NSubstitute;
using Shouldly;
using Xunit;

public class GetBooksHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new GetBooksQuery();

        var context = Substitute.For<IBooksRepository>();
        var handler = new GetBooksHandler(context);
        var token = new CancellationTokenSource().Token;

        _ = context.GetBooks(token).Returns([new Book(Guid.Empty, "Title")]);

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).GetBooks(token);

        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<List<Book>>();

        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(1);

        result[0].Id.ShouldBe(Guid.Empty);
        result[0].Title.ShouldBe("Title");
    }
}
