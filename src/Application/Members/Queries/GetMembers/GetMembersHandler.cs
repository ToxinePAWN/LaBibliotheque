namespace CleanMinimalApi.Application.Authors.Queries.GetAuthors;

using System.Threading;
using System.Threading.Tasks;
using Entities;
using MediatR;

public class GetMembersHandler(IMembersRepository repository) : IRequestHandler<GetMemberQuery, List<Member>>
{
    public async Task<List<Member>> Handle(GetMemberQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetMembers(cancellationToken);
    }
}
