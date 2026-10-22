namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Commands.CreateReview;

using System.Threading;
using System.Threading.Tasks;
using Application.Authors;
using Application.Common.Exceptions;
using Application.Movies;
using Loan;
using Loan.Commands.CreateReview;
using NSubstitute;
using Shouldly;
using Xunit;

public class CreateHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Command()
    {
        // Arrange
        var command = new CreateReviewCommand
        {
            AuthorId = Guid.Empty,
            BookId = Guid.Empty,
            Stars = 5
        };

        var authorsRepository = Substitute.For<IMembersRepository>();
        var moviesRepository = Substitute.For<IBooksRepository>();
        var reviewsRepository = Substitute.For<ILoansRepository>();

        _ = authorsRepository.MemberExists(default, default).ReturnsForAnyArgs(true);
        _ = moviesRepository.BookExists(default, default).ReturnsForAnyArgs(true);

        var handler = new CreateLoanHandler(authorsRepository, moviesRepository, reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        _ = await handler.Handle(command, token);

        // Assert
        _ = await authorsRepository.Received(1).MemberExists(command.AuthorId, token);
        _ = await moviesRepository.Received(1).BookExists(command.BookId, token);
        _ = await reviewsRepository.Received(1).CreateLoan(command.AuthorId, command.BookId, command.Stars, token);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_AuthorDoesNotExist()
    {
        // Arrange
        var command = new CreateReviewCommand
        {
            AuthorId = Guid.Empty,
            BookId = Guid.Empty,
            Stars = 5
        };

        var authorsRepository = Substitute.For<IMembersRepository>();
        var moviesRepository = Substitute.For<IBooksRepository>();
        var reviewsRepository = Substitute.For<ILoansRepository>();

        _ = authorsRepository.MemberExists(default, default).ReturnsForAnyArgs(false);
        _ = moviesRepository.BookExists(default, default).ReturnsForAnyArgs(true);

        var handler = new CreateLoanHandler(authorsRepository, moviesRepository, reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(command, token));

        // Assert
        exception.Message.ShouldBe("The Author with the supplied id was not found.");

        _ = await authorsRepository.Received(1).MemberExists(command.AuthorId, token);
        _ = await moviesRepository.DidNotReceive().BookExists(command.BookId, token);
        _ = await reviewsRepository.DidNotReceive().CreateLoan(command.AuthorId, command.BookId, command.Stars, token);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_MovieDoesNotExist()
    {
        // Arrange
        var command = new CreateReviewCommand
        {
            AuthorId = Guid.Empty,
            BookId = Guid.Empty,
            Stars = 5
        };
        var authorsRepository = Substitute.For<IMembersRepository>();
        var moviesRepository = Substitute.For<IBooksRepository>();
        var reviewsRepository = Substitute.For<ILoansRepository>();

        _ = authorsRepository.MemberExists(default, default).ReturnsForAnyArgs(true);
        _ = moviesRepository.BookExists(default, default).ReturnsForAnyArgs(false);

        var handler = new CreateLoanHandler(authorsRepository, moviesRepository, reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(command, token));

        // Assert
        exception.Message.ShouldBe("The Movie with the supplied id was not found.");

        _ = await authorsRepository.Received(1).MemberExists(command.AuthorId, token);
        _ = await moviesRepository.Received(1).BookExists(command.BookId, token);
        _ = await reviewsRepository.DidNotReceive().CreateLoan(command.AuthorId, command.BookId, command.Stars, token);
    }
}
