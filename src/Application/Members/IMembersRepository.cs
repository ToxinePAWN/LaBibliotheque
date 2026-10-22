namespace CleanMinimalApi.Application.Authors;

using System.Threading.Tasks;
using Entities;

public interface IMembersRepository
{
    public Task<List<Member>> GetMembers(CancellationToken cancellationToken);

    public Task<Member> GetMemberById(Guid id, CancellationToken cancellationToken);

    public Task<bool> MemberExists(Guid id, CancellationToken cancellationToken);
}
