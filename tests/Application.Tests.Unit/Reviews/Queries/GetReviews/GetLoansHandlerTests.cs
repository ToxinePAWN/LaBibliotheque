namespace CleanMinimalApi.Application.Tests.Unit.Reviews.Queries.GetReviews;

using System.Threading;
using System.Threading.Tasks;
using Loan;
using Loan.Entities;
using Loan.Queries.GetReviews;
using NSubstitute;
using Shouldly;
using Xunit;

public class GetLoansHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new GetLoansQuery();

        var context = Substitute.For<ILoansRepository>();
        var handler = new GetLoansHandler(context);
        var token = new CancellationTokenSource().Token;

        _ = context.GetLoans(token).Returns([new Loan(Guid.Empty, 5)]);

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).GetLoans(token);

        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<List<Loan>>();

        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(1);

        result[0].Id.ShouldBe(Guid.Empty);
        result[0].Stars.ShouldBe(5);
    }
}
