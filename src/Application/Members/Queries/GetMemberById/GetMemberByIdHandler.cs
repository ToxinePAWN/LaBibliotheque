namespace CleanMinimalApi.Application.Authors.Queries.GetAuthorById;

using System.Threading;
using System.Threading.Tasks;
using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;

public class GetMemberByIdHandler(IMembersRepository repository) : IRequestHandler<GetMemberByIdQuery, Member>
{
    public async Task<Member> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetMemberById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Member);

        return result;
    }
}
