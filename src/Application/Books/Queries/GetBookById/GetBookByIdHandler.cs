namespace CleanMinimalApi.Application.Movies.Queries.GetMovieById;

using System.Threading;
using System.Threading.Tasks;
using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;

public class GetBookByIdHandler(IBooksRepository repository) : IRequestHandler<GetBookByIdQuery, Book>
{
    public async Task<Book> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetBookById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Book);

        return result;
    }
}
