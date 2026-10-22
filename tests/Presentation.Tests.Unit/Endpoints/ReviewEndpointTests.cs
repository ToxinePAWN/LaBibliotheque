namespace CleanMinimalApi.Presentation.Tests.Unit.Endpoints;

using System.Threading.Tasks;
using CleanMinimalApi.Application.Authors.Entities;
using CleanMinimalApi.Application.Common.Exceptions;
using CleanMinimalApi.Application.Movies.Entities;
using CleanMinimalApi.Presentation.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Xunit;
using Commands = Application.Loan.Commands;
using Entities = Application.Loan.Entities;
using Queries = Application.Loan.Queries;

public class ReviewEndpointTests
{
    [Fact]
    public async Task GetReviews_ShouldReturn_Ok()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Queries.GetReviews.GetLoansQuery>())
            .ReturnsForAnyArgs(
            [
                new Entities.Loan(
                    Guid.Empty,
                    5,
                    new ReviewedMovie(Guid.Empty, "Lorem Ipsum"),
                    new ReviewAuthor(Guid.Empty, "Lorem", "Ipsum")
                )
            ]);

        // Act
        var response = await ReviewEndpoints.GetReviews(sender);

        // Assert
        var result = response.Result.ShouldBeOfType<Ok<List<Entities.Loan>>>();

        result.StatusCode.ShouldBe(StatusCodes.Status200OK);

        var value = result.Value.ShouldBeOfType<List<Entities.Loan>>();

        _ = value[0].Id.ShouldBeOfType<Guid>();
        value[0].Id.ShouldBe(Guid.Empty);
        _ = value[0].Stars.ShouldBeOfType<int>();
        value[0].Stars.ShouldBe(5);

        _ = value[0].ReviewAuthor.Id.ShouldBeOfType<Guid>();
        value[0].ReviewAuthor.Id.ShouldBe(Guid.Empty);
        _ = value[0].ReviewAuthor.FirstName.ShouldBeOfType<string>();
        value[0].ReviewAuthor.FirstName.ShouldBe("Lorem");
        _ = value[0].ReviewAuthor.LastName.ShouldBeOfType<string>();
        value[0].ReviewAuthor.LastName.ShouldBe("Ipsum");

        _ = value[0].ReviewedMovie.Id.ShouldBeOfType<Guid>();
        value[0].ReviewedMovie.Id.ShouldBe(Guid.Empty);
        _ = value[0].ReviewedMovie.Title.ShouldBeOfType<string>();
        value[0].ReviewedMovie.Title.ShouldBe("Lorem Ipsum");
    }

    [Fact]
    public async Task GetReviews_ShouldReturn_Problem()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Queries.GetReviews.GetLoansQuery>())
            .Throws(new ArgumentException("Expected Exception"));

        // Act
        var response = await ReviewEndpoints.GetReviews(sender);

        // Assert
        var result = response.Result.ShouldBeOfType<ProblemHttpResult>();

        result.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

        result.ProblemDetails.Title.ShouldBe("An error occurred while processing your request.");
        result.ProblemDetails.Instance.ShouldBe("Expected Exception");
        result.ProblemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        result.ProblemDetails.Detail.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetReviewById_ShouldReturn_Ok()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Queries.GetReviewById.GetLoanByIdQuery>())
            .ReturnsForAnyArgs(new Entities.Loan(
                Guid.Empty,
                5,
                new ReviewedMovie(Guid.Empty, "Lorem Ipsum"),
                new ReviewAuthor(Guid.Empty, "Lorem", "Ipsum")
            ));

        // Act
        var response = await ReviewEndpoints.GetReviewById(Guid.Empty, sender);

        // Assert
        var result = response.Result.ShouldBeOfType<Ok<Entities.Loan>>();

        result.StatusCode.ShouldBe(StatusCodes.Status200OK);

        var value = result.Value.ShouldBeOfType<Entities.Loan>();

        _ = value.Id.ShouldBeOfType<Guid>();
        value.Id.ShouldBe(Guid.Empty);
        _ = value.Stars.ShouldBeOfType<int>();
        value.Stars.ShouldBe(5);

        _ = value.ReviewAuthor.Id.ShouldBeOfType<Guid>();
        value.ReviewAuthor.Id.ShouldBe(Guid.Empty);
        _ = value.ReviewAuthor.FirstName.ShouldBeOfType<string>();
        value.ReviewAuthor.FirstName.ShouldBe("Lorem");
        _ = value.ReviewAuthor.LastName.ShouldBeOfType<string>();
        value.ReviewAuthor.LastName.ShouldBe("Ipsum");

        _ = value.ReviewedMovie.Id.ShouldBeOfType<Guid>();
        value.ReviewedMovie.Id.ShouldBe(Guid.Empty);
        _ = value.ReviewedMovie.Title.ShouldBeOfType<string>();
        value.ReviewedMovie.Title.ShouldBe("Lorem Ipsum");
    }

    [Fact]
    public async Task GetReviewById_ShouldReturn_NotFound()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Queries.GetReviewById.GetLoanByIdQuery>())
            .Throws(new NotFoundException("Expected Exception"));

        // Act
        var response = await ReviewEndpoints.GetReviewById(Guid.Empty, sender);

        // Assert
        var result = response.Result.ShouldBeOfType<NotFound<string>>();

        result.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
        result.Value.ShouldBe("Expected Exception");
    }

    [Fact]
    public async Task GetReviewById_ShouldReturn_Problem()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Queries.GetReviewById.GetLoanByIdQuery>())
            .Throws(new ArgumentException("Expected Exception"));

        // Act
        var response = await ReviewEndpoints.GetReviewById(Guid.Empty, sender);

        // Assert
        var result = response.Result.ShouldBeOfType<ProblemHttpResult>();

        result.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

        result.ProblemDetails.Title.ShouldBe("An error occurred while processing your request.");
        result.ProblemDetails.Instance.ShouldBe("Expected Exception");
        result.ProblemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        result.ProblemDetails.Detail.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task CreateReview_ShouldReturn_Created()
    {
        // Arrange
        //var httpRequest = Substitute.For<HttpRequest>();
        var sender = Substitute.For<ISender>();
        var request = new Requests.CreateReviewRequest
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        _ = sender
            .Send(Arg.Any<Commands.CreateReview.CreateReviewCommand>())
            .ReturnsForAnyArgs(new Entities.Loan(
                Guid.Empty,
                5,
                new ReviewedMovie(Guid.Empty, "Lorem Ipsum"),
                new ReviewAuthor(Guid.Empty, "Lorem", "Ipsum")
            ));

        // Act
        var response = await ReviewEndpoints.CreateReview(request, sender);

        // Assert
        var result = response.Result.ShouldBeOfType<Created<Entities.Loan>>();

        result.StatusCode.ShouldBe(StatusCodes.Status201Created);

        var value = result.Value.ShouldBeOfType<Entities.Loan>();

        _ = value.Id.ShouldBeOfType<Guid>();
        value.Id.ShouldBe(Guid.Empty);
        _ = value.Stars.ShouldBeOfType<int>();
        value.Stars.ShouldBe(5);

        _ = value.ReviewAuthor.Id.ShouldBeOfType<Guid>();
        value.ReviewAuthor.Id.ShouldBe(Guid.Empty);
        _ = value.ReviewAuthor.FirstName.ShouldBeOfType<string>();
        value.ReviewAuthor.FirstName.ShouldBe("Lorem");
        _ = value.ReviewAuthor.LastName.ShouldBeOfType<string>();
        value.ReviewAuthor.LastName.ShouldBe("Ipsum");

        _ = value.ReviewedMovie.Id.ShouldBeOfType<Guid>();
        value.ReviewedMovie.Id.ShouldBe(Guid.Empty);
        _ = value.ReviewedMovie.Title.ShouldBeOfType<string>();
        value.ReviewedMovie.Title.ShouldBe("Lorem Ipsum");
    }

    [Fact]
    public async Task CreateReview_ShouldReturn_NotFound()
    {
        // Arrange
        //var httpRequest = Substitute.For<HttpRequest>();
        var sender = Substitute.For<ISender>();
        var request = new Requests.CreateReviewRequest
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        _ = sender
            .Send(Arg.Any<Commands.CreateReview.CreateReviewCommand>())
            .Throws(new NotFoundException("Expected Exception"));

        // Act
        var response = await ReviewEndpoints.CreateReview(request, sender);

        // Assert
        var result = response.Result.ShouldBeOfType<NotFound<string>>();

        result.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
        result.Value.ShouldBe("Expected Exception");
    }

    [Fact]
    public async Task CreateReview_ShouldReturn_Problem()
    {
        // Arrange
        var httpRequest = Substitute.For<HttpRequest>();
        var sender = Substitute.For<ISender>();
        var request = new Requests.CreateReviewRequest
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        _ = sender
            .Send(Arg.Any<Commands.CreateReview.CreateReviewCommand>())
            .Throws(new ArgumentException("Expected Exception"));

        // Act
        var response = await ReviewEndpoints.CreateReview(request, sender);

        // Assert
        var result = response.Result.ShouldBeOfType<ProblemHttpResult>();

        result.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

        result.ProblemDetails.Title.ShouldBe("An error occurred while processing your request.");
        result.ProblemDetails.Instance.ShouldBe("Expected Exception");
        result.ProblemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        result.ProblemDetails.Detail.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task UpdateReview_ShouldReturn_NoContent()
    {
        // Arrange
        var sender = Substitute.For<ISender>();
        var request = new Requests.UpdateReviewRequest
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        _ = sender
            .Send(Arg.Any<Commands.UpdateReview.UpdateLoanCommand>())
            .ReturnsForAnyArgs(true);

        // Act
        var response = await ReviewEndpoints.UpdateReview(Guid.NewGuid(), request, sender);

        // Assert
        var result = response.Result.ShouldBeOfType<NoContent>();

        result.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task UpdateReview_ShouldReturn_NotFound()
    {
        // Arrange
        var sender = Substitute.For<ISender>();
        var request = new Requests.UpdateReviewRequest
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        _ = sender
            .Send(Arg.Any<Commands.UpdateReview.UpdateLoanCommand>())
            .Throws(new NotFoundException("Expected Exception"));

        // Act
        var response = await ReviewEndpoints.UpdateReview(Guid.NewGuid(), request, sender);

        // Assert
        var result = response.Result.ShouldBeOfType<NotFound<string>>();

        result.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
        result.Value.ShouldBe("Expected Exception");
    }

    [Fact]
    public async Task UpdateReview_ShouldReturn_Problem()
    {
        // Arrange
        var sender = Substitute.For<ISender>();
        var request = new Requests.UpdateReviewRequest
        {
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        _ = sender
            .Send(Arg.Any<Commands.UpdateReview.UpdateLoanCommand>())
            .Throws(new ArgumentException("Expected Exception"));

        // Act
        var response = await ReviewEndpoints.UpdateReview(Guid.NewGuid(), request, sender);

        // Assert
        var result = response.Result.ShouldBeOfType<ProblemHttpResult>();

        result.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

        result.ProblemDetails.Title.ShouldBe("An error occurred while processing your request.");
        result.ProblemDetails.Instance.ShouldBe("Expected Exception");
        result.ProblemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        result.ProblemDetails.Detail.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task DeleteReview_ShouldReturn_NoContent()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Commands.DeleteReview.DeleteLoanCommand>())
            .ReturnsForAnyArgs(true);

        // Act
        var response = await ReviewEndpoints.DeleteReview(Guid.NewGuid(), sender);

        // Assert
        var result = response.Result.ShouldBeOfType<NoContent>();

        result.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task DeleteReview_ShouldReturn_NotFound()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Commands.DeleteReview.DeleteLoanCommand>())
            .Throws(new NotFoundException("Expected Exception"));

        // Act
        var response = await ReviewEndpoints.DeleteReview(Guid.NewGuid(), sender);

        // Assert
        var result = response.Result.ShouldBeOfType<NotFound<string>>();

        result.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
        result.Value.ShouldBe("Expected Exception");
    }

    [Fact]
    public async Task DeleteReview_ShouldReturn_Problem()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Commands.DeleteReview.DeleteLoanCommand>())
            .Throws(new ArgumentException("Expected Exception"));

        // Act
        var response = await ReviewEndpoints.DeleteReview(Guid.Empty, sender);

        // Assert
        var result = response.Result.ShouldBeOfType<ProblemHttpResult>();

        result.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

        result.ProblemDetails.Title.ShouldBe("An error occurred while processing your request.");
        result.ProblemDetails.Instance.ShouldBe("Expected Exception");
        result.ProblemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        result.ProblemDetails.Detail.ShouldNotBeNullOrEmpty();
    }
}
