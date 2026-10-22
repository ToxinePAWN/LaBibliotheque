namespace CleanMinimalApi.Presentation.Tests.Unit.Endpoints;

using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Exceptions;
using Application.Loan.Entities;
using CleanMinimalApi.Presentation.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Xunit;
using Entities = Application.Movies.Entities;
using Queries = Application.Movies.Queries;

public class MovieEndpointTests
{
    [Fact]
    public async Task GetMovies_ShouldReturn_Ok()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Queries.GetMovies.GetBooksQuery>())
            .ReturnsForAnyArgs(
            [
                new Entities.Book(Guid.Empty, "Lorem Ipsum")
            ]);

        // Act
        var response = await MovieEndpoints.GetMovies(sender);

        // Assert
        var result = response.Result.ShouldBeOfType<Ok<List<Entities.Book>>>();

        result.StatusCode.ShouldBe(StatusCodes.Status200OK);

        var value = result.Value.ShouldBeOfType<List<Entities.Book>>();

        _ = value[0].Id.ShouldBeOfType<Guid>();
        value[0].Id.ShouldBe(Guid.Empty);
        _ = value[0].Title.ShouldBeOfType<string>();
        value[0].Title.ShouldBe("Lorem Ipsum");
        _ = value[0].Reviews.ShouldBeAssignableTo<ICollection<Loan>>();
        value[0].Reviews.ShouldBeNull();
    }

    [Fact]
    public async Task GetMovies_ShouldReturn_Problem()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Queries.GetMovies.GetBooksQuery>())
            .Throws(new ArgumentException("Expected Exception"));

        // Act
        var response = await MovieEndpoints.GetMovies(sender);

        // Assert
        var result = response.Result.ShouldBeOfType<ProblemHttpResult>();

        result.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

        result.ProblemDetails.Title.ShouldBe("An error occurred while processing your request.");
        result.ProblemDetails.Instance.ShouldBe("Expected Exception");
        result.ProblemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        result.ProblemDetails.Detail.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetMovieById_ShouldReturn_Ok()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Queries.GetMovieById.GetBookByIdQuery>())
            .ReturnsForAnyArgs(new Entities.Book(Guid.Empty, "Lorem Ipsum"));

        // Act
        var response = await MovieEndpoints.GetMovieById(Guid.Empty, sender);

        // Assert
        var result = response.Result.ShouldBeOfType<Ok<Entities.Book>>();

        result.StatusCode.ShouldBe(StatusCodes.Status200OK);

        var value = result.Value.ShouldBeOfType<Entities.Book>();

        _ = value.Id.ShouldBeOfType<Guid>();
        value.Id.ShouldBe(Guid.Empty);
        _ = value.Title.ShouldBeOfType<string>();
        value.Title.ShouldBe("Lorem Ipsum");
        _ = value.Reviews.ShouldBeAssignableTo<ICollection<Loan>>();
        value.Reviews.ShouldBeNull();
    }

    [Fact]
    public async Task GetMovieById_ShouldReturn_NotFound()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Queries.GetMovieById.GetBookByIdQuery>())
            .Throws(new NotFoundException("Expected Exception"));

        // Act
        var response = await MovieEndpoints.GetMovieById(Guid.Empty, sender);

        // Assert
        var result = response.Result.ShouldBeOfType<NotFound<string>>();

        result.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
        result.Value.ShouldBe("Expected Exception");
    }

    [Fact]
    public async Task GetMovieById_ShouldReturn_Problem()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Queries.GetMovieById.GetBookByIdQuery>())
            .Throws(new ArgumentException("Expected Exception"));

        // Act
        var response = await MovieEndpoints.GetMovieById(Guid.Empty, sender);

        // Assert
        var result = response.Result.ShouldBeOfType<ProblemHttpResult>();

        result.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

        result.ProblemDetails.Title.ShouldBe("An error occurred while processing your request.");
        result.ProblemDetails.Instance.ShouldBe("Expected Exception");
        result.ProblemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        result.ProblemDetails.Detail.ShouldNotBeNullOrEmpty();
    }
}
