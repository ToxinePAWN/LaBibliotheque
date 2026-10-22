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
using Entities = Application.Authors.Entities;
using Queries = Application.Authors.Queries;

public class AuthorEndpointTests
{
    [Fact]
    public async Task GetAuthors_ShouldReturn_Ok()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Queries.GetAuthors.GetMemberQuery>())
            .ReturnsForAnyArgs(
            [
                new Entities.Member(Guid.Empty, "Lorem", "Ipsum")
            ]);

        // Act
        var response = await AuthorEndpoints.GetAuthors(sender);

        // Assert
        var result = response.Result.ShouldBeOfType<Ok<List<Entities.Member>>>();

        result.StatusCode.ShouldBe(StatusCodes.Status200OK);

        var value = result.Value.ShouldBeOfType<List<Entities.Member>>();

        _ = value[0].Id.ShouldBeOfType<Guid>();
        value[0].Id.ShouldBe(Guid.Empty);
        _ = value[0].FirstName.ShouldBeOfType<string>();
        value[0].FirstName.ShouldBe("Lorem");
        _ = value[0].LastName.ShouldBeOfType<string>();
        value[0].LastName.ShouldBe("Ipsum");
        _ = value[0].Loans.ShouldBeAssignableTo<ICollection<Loan>>();
        value[0].Loans.ShouldBeNull();
    }

    [Fact]
    public async Task GetAuthors_ShouldReturn_Problem()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Queries.GetAuthors.GetMemberQuery>())
            .Throws(new ArgumentException("Expected Exception"));

        // Act
        var response = await AuthorEndpoints.GetAuthors(sender);

        // Assert
        var result = response.Result.ShouldBeOfType<ProblemHttpResult>();

        result.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

        result.ProblemDetails.Title.ShouldBe("An error occurred while processing your request.");
        result.ProblemDetails.Instance.ShouldBe("Expected Exception");
        result.ProblemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        result.ProblemDetails.Detail.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetAuthorById_ShouldReturn_Ok()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Queries.GetAuthorById.GetMemberByIdQuery>())
            .ReturnsForAnyArgs(new Entities.Member(Guid.Empty, "Lorem", "Ipsum"));

        // Act
        var response = await AuthorEndpoints.GetAuthorById(Guid.Empty, sender);

        // Assert
        var result = response.Result.ShouldBeOfType<Ok<Entities.Member>>();

        result.StatusCode.ShouldBe(StatusCodes.Status200OK);

        var value = result.Value.ShouldBeOfType<Entities.Member>();

        _ = value.Id.ShouldBeOfType<Guid>();
        value.Id.ShouldBe(Guid.Empty);
        _ = value.FirstName.ShouldBeOfType<string>();
        value.FirstName.ShouldBe("Lorem");
        _ = value.LastName.ShouldBeOfType<string>();
        value.LastName.ShouldBe("Ipsum");
        _ = value.Loans.ShouldBeAssignableTo<ICollection<Loan>>();
        value.Loans.ShouldBeNull();
    }

    [Fact]
    public async Task GetAuthorById_ShouldReturn_NotFound()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Queries.GetAuthorById.GetMemberByIdQuery>())
            .Throws(new NotFoundException("Expected Exception"));

        // Act
        var response = await AuthorEndpoints.GetAuthorById(Guid.Empty, sender);

        // Assert
        var result = response.Result.ShouldBeOfType<NotFound<string>>();

        result.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
        result.Value.ShouldBe("Expected Exception");
    }

    [Fact]
    public async Task GetAuthorById_ShouldReturn_Problem()
    {
        // Arrange
        var sender = Substitute.For<ISender>();

        _ = sender
            .Send(Arg.Any<Queries.GetAuthorById.GetMemberByIdQuery>())
            .Throws(new ArgumentException("Expected Exception"));

        // Act
        var response = await AuthorEndpoints.GetAuthorById(Guid.Empty, sender);

        // Assert
        var result = response.Result.ShouldBeOfType<ProblemHttpResult>();

        result.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);

        result.ProblemDetails.Title.ShouldBe("An error occurred while processing your request.");
        result.ProblemDetails.Instance.ShouldBe("Expected Exception");
        result.ProblemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        result.ProblemDetails.Detail.ShouldNotBeNullOrEmpty();
    }
}
