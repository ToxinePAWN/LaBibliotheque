namespace CleanMinimalApi.Infrastructure.Databases.MovieReviews.Configuration;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

internal class MemberConfiguration : EntityConfiguration<MemberModel>
{
    public override void Configure(EntityTypeBuilder<MemberModel> builder)
    {
        base.Configure(builder);

        builder.HasMany<LoanModel>().WithOne().HasForeignKey(l => l.MemberId);
    }
}
