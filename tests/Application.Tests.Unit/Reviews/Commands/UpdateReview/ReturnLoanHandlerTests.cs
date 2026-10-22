namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Commands.UpdateReview;

using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Loan;
using Loan.Commands.UpdateReview;
using CleanMinimalApi.Application.Authors;
using CleanMinimalApi.Application.Movies;
using NSubstitute;
using Shouldly;
using Xunit;

public class ReturnLoanHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Command()
    {
        // Arrange
        var command = new UpdateLoanCommand
        {
            Id = Guid.Empty,
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 1
        };

        var authorsRepository = Substitute.For<IMembersRepository>();
        var moviesRepository = Substitute.For<IBooksRepository>();
        var reviewsRepository = Substitute.For<ILoansRepository>();

        _ = reviewsRepository.LoanExists(default, default).ReturnsForAnyArgs(true);
        _ = authorsRepository.MemberExists(default, default).ReturnsForAnyArgs(true);
        _ = moviesRepository.BookExists(default, default).ReturnsForAnyArgs(true);

        var handler = new ReturnLoanHandler(authorsRepository, moviesRepository, reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        _ = await handler.Handle(command, token);

        // Assert
        _ = await reviewsRepository.Received(1).LoanExists(command.Id, token);
        _ = await authorsRepository.Received(1).MemberExists(command.AuthorId, token);
        _ = await moviesRepository.Received(1).BookExists(command.MovieId, token);
        _ = await reviewsRepository.Received(1).UpdateLoan(command.Id, command.AuthorId, command.MovieId, command.Stars, token);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_ReviewDoesNotExist()
    {
        // Arrange
        var command = new UpdateLoanCommand
        {
            Id = Guid.Empty,
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        var authorsRepository = Substitute.For<IMembersRepository>();
        var moviesRepository = Substitute.For<IBooksRepository>();
        var reviewsRepository = Substitute.For<ILoansRepository>();

        _ = reviewsRepository.LoanExists(default, default).ReturnsForAnyArgs(false);
        _ = authorsRepository.MemberExists(default, default).ReturnsForAnyArgs(true);
        _ = moviesRepository.BookExists(default, default).ReturnsForAnyArgs(true);

        var handler = new ReturnLoanHandler(authorsRepository, moviesRepository, reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(command, token));

        // Assert
        exception.Message.ShouldBe("The Review with the supplied id was not found.");

        _ = await reviewsRepository.Received(1).LoanExists(command.Id, token);
        _ = await authorsRepository.DidNotReceive().MemberExists(command.AuthorId, token);
        _ = await moviesRepository.DidNotReceive().BookExists(command.MovieId, token);
        _ = await reviewsRepository.DidNotReceive().UpdateLoan(command.Id, command.AuthorId, command.MovieId, command.Stars, token);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_AuthorDoesNotExist()
    {
        // Arrange
        var command = new UpdateLoanCommand
        {
            Id = Guid.Empty,
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        var authorsRepository = Substitute.For<IMembersRepository>();
        var moviesRepository = Substitute.For<IBooksRepository>();
        var reviewsRepository = Substitute.For<ILoansRepository>();

        _ = reviewsRepository.LoanExists(default, default).ReturnsForAnyArgs(true);
        _ = authorsRepository.MemberExists(default, default).ReturnsForAnyArgs(false);
        _ = moviesRepository.BookExists(default, default).ReturnsForAnyArgs(true);

        var handler = new ReturnLoanHandler(authorsRepository, moviesRepository, reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(command, token));

        // Assert
        exception.Message.ShouldBe("The Author with the supplied id was not found.");

        _ = await reviewsRepository.Received(1).LoanExists(command.Id, token);
        _ = await authorsRepository.Received(1).MemberExists(command.AuthorId, token);
        _ = await moviesRepository.DidNotReceive().BookExists(command.MovieId, token);
        _ = await reviewsRepository.DidNotReceive().CreateLoan(command.AuthorId, command.MovieId, command.Stars, token);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_MovieDoesNotExist()
    {
        // Arrange
        var command = new UpdateLoanCommand
        {
            Id = Guid.Empty,
            AuthorId = Guid.Empty,
            MovieId = Guid.Empty,
            Stars = 5
        };

        var authorsRepository = Substitute.For<IMembersRepository>();
        var moviesRepository = Substitute.For<IBooksRepository>();
        var reviewsRepository = Substitute.For<ILoansRepository>();

        _ = reviewsRepository.LoanExists(default, default).ReturnsForAnyArgs(true);
        _ = authorsRepository.MemberExists(default, default).ReturnsForAnyArgs(true);
        _ = moviesRepository.BookExists(default, default).ReturnsForAnyArgs(false);

        var handler = new ReturnLoanHandler(authorsRepository, moviesRepository, reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(command, token));

        // Assert
        exception.Message.ShouldBe("The Movie with the supplied id was not found.");

        _ = await reviewsRepository.Received(1).LoanExists(command.Id, token);
        _ = await authorsRepository.Received(1).MemberExists(command.AuthorId, token);
        _ = await moviesRepository.Received(1).BookExists(command.MovieId, token);
        _ = await reviewsRepository.DidNotReceive().CreateLoan(command.AuthorId, command.MovieId, command.Stars, token);
    }
}
