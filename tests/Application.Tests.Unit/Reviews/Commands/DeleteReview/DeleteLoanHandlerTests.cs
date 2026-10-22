namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Commands.DeleteReview;

using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Loan;
using Loan.Commands.DeleteReview;
using NSubstitute;
using Shouldly;
using Xunit;

public class DeleteLoanHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Command()
    {
        // Arrange
        var command = new DeleteLoanCommand { Id = Guid.Empty };
        var reviewsRepository = Substitute.For<ILoansRepository>();

        _ = reviewsRepository.LoanExists(default, default).ReturnsForAnyArgs(true);

        var handler = new DeleteLoanHandler(reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        _ = await handler.Handle(command, token);

        // Assert
        _ = await reviewsRepository.Received(1).LoanExists(command.Id, token);
        _ = await reviewsRepository.Received(1).DeleteReview(command.Id, token);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_ReviewDoesNotExist()
    {
        // Arrange
        var command = new DeleteLoanCommand { Id = Guid.Empty };
        var reviewsRepository = Substitute.For<ILoansRepository>();

        _ = reviewsRepository.LoanExists(default, default).ReturnsForAnyArgs(false);

        var handler = new DeleteLoanHandler(reviewsRepository);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(command, token));

        // Assert
        exception.Message.ShouldBe("The Review with the supplied id was not found.");

        _ = await reviewsRepository.Received(1).LoanExists(command.Id, token);
        _ = await reviewsRepository.DidNotReceive().DeleteReview(command.Id, token);
    }
}
