namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Queries.GetReviewById;

using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Loan;
using Loan.Entities;
using Loan.Queries.GetReviewById;
using NSubstitute;
using Shouldly;
using Xunit;

public class GetLoanByIdHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new GetLoanByIdQuery
        {
            Id = Guid.Empty
        };

        var context = Substitute.For<ILoansRepository>();
        var handler = new GetLoanByIdHandler(context);
        var token = new CancellationTokenSource().Token;

        _ = context.GetLoanById(Arg.Any<Guid>(), token).Returns(new Loan(Guid.Empty, 5));

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).GetLoanById(query.Id, token);

        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<Loan>();

        result.Id.ShouldBe(Guid.Empty);
        result.Stars.ShouldBe(5);
    }


    [Fact]
    public async Task Handle_ShouldThrowException_DoesNotExist()
    {
        // Arrange
        var query = new GetLoanByIdQuery
        {
            Id = Guid.Empty
        };

        var context = Substitute.For<ILoansRepository>();
        var handler = new GetLoanByIdHandler(context);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(query, token));

        // Assert
        _ = await context.Received(1).GetLoanById(query.Id, token);

        exception.Message.ShouldBe("The Review with the supplied id was not found.");
    }
}
