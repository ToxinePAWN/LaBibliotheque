namespace CleanMinimalApi.Application.Movies.Queries.GetMovieById;

using System.ComponentModel.DataAnnotations;
using Entities;
using MediatR;

public class GetBookByIdQuery : IRequest<Book>
{
    [Required]
    public Guid Id { get; init; }
}
