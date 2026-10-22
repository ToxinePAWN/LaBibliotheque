namespace CleanMinimalApi.Application.Movies.Queries.GetMovies;

using System.Threading;
using System.Threading.Tasks;
using Entities;
using MediatR;

public class GetBooksHandler(IBooksRepository repository) : IRequestHandler<GetBooksQuery, List<Book>>
{
    public async Task<List<Book>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetBooks(cancellationToken);
    }
}
