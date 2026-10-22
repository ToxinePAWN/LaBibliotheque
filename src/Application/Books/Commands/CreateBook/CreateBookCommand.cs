namespace CleanMinimalApi.Application.Movies.Commands.CreateBook;

using Entities;
using MediatR;

public class CreateBookCommand : IRequest<Book>
{
    public string Title { get; init; }
    public string Author { get; init; }
    public int Copies { get; init; }
}
