namespace CleanMinimalApi.Infrastructure.Databases.MovieReviews;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Models;

internal class MovieReviewsDbContext(DbContextOptions<MovieReviewsDbContext> options) : DbContext(options)
{
    public DbSet<MemberModel> Authors { get; set; }
    public DbSet<BookModel> Books { get; set; }
    public DbSet<LoanModel> Loans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        _ = modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
