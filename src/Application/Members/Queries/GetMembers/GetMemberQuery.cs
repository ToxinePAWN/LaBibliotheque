namespace CleanMinimalApi.Application.Authors.Queries.GetAuthors;

using Entities;
using MediatR;

public class GetMemberQuery : IRequest<List<Member>>
{
}
