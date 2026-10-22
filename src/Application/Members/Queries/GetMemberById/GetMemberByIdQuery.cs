namespace CleanMinimalApi.Application.Authors.Queries.GetAuthorById;

using System.ComponentModel.DataAnnotations;
using Entities;
using MediatR;

public class GetMemberByIdQuery : IRequest<Member>
{
    [Required]
    public Guid Id { get; init; }
}
