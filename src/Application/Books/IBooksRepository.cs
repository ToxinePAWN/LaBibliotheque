namespace CleanMinimalApi.Application.Movies;

using System.Threading.Tasks;
using Entities;

public interface IBooksRepository
{
    public Task<List<Book>> GetBooks(CancellationToken cancellationToken);

    public Task<Book> GetBookById(Guid id, CancellationToken cancellationToken);

    public Task<bool> BookExists(Guid id, CancellationToken cancellationToken);
}
