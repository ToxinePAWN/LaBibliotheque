namespace CleanMinimalApi.Infrastructure.Databases.MovieReviews.Configuration;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

internal class BookConfiguration : EntityConfiguration<BookModel>
{
    public override void Configure(EntityTypeBuilder<BookModel> builder)
    {
        base.Configure(builder);

        builder.HasMany<LoanModel>().WithOne().HasForeignKey(l => l.BookId);
    }
}
