namespace CleanMinimalApi.Application.Tests.Unit.Authors.Queries.GetAuthorById;

using System.Threading;
using System.Threading.Tasks;
using Application.Authors;
using Application.Authors.Entities;
using Application.Authors.Queries.GetAuthorById;
using Application.Common.Exceptions;
using NSubstitute;
using Shouldly;
using Xunit;

public class GetMemberByIdHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPassThrough_Query()
    {
        // Arrange
        var query = new GetMemberByIdQuery
        {
            Id = Guid.Empty
        };

        var context = Substitute.For<IMembersRepository>();
        var handler = new GetMemberByIdHandler(context);
        var token = new CancellationTokenSource().Token;

        _ = context.GetMemberById(Arg.Any<Guid>(), token).Returns(new Member(Guid.Empty, "FirstName", "LastName"));

        // Act
        var result = await handler.Handle(query, token);

        // Assert
        _ = await context.Received(1).GetMemberById(query.Id, token);

        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<Member>();

        result.Id.ShouldBe(Guid.Empty);
        result.FirstName.ShouldBe("FirstName");
        result.LastName.ShouldBe("LastName");
        result.Loans.ShouldBeNull();
    }


    [Fact]
    public async Task Handle_ShouldThrowException_DoesNotExist()
    {
        // Arrange
        var query = new GetMemberByIdQuery
        {
            Id = Guid.Empty
        };

        var context = Substitute.For<IMembersRepository>();
        var handler = new GetMemberByIdHandler(context);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await handler.Handle(query, token));

        // Assert
        _ = await context.Received(1).GetMemberById(query.Id, token);

        exception.Message.ShouldBe("The Author with the supplied id was not found.");
    }
}
