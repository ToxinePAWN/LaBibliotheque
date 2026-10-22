namespace CleanMinimalApi.Application.Movies.Queries.GetMovies;

using Entities;
using MediatR;

public class GetBooksQuery : IRequest<List<Book>>
{
}
