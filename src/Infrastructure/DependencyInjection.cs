namespace CleanMinimalApi.Infrastructure;

using System;
using Application.Authors;
using Application.Movies;
using Application.Loans;
using Databases.MovieReviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        _ = services.AddDbContext<MovieReviewsDbContext>(options =>
            options.UseInMemoryDatabase($"Movies-{Guid.NewGuid()}"), ServiceLifetime.Singleton);

        _ = services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        _ = services.AddSingleton<EntityFrameworkBookLoansRepository>();

        _ = services.AddSingleton<IMembersRepository>(p =>
            p.GetRequiredService<EntityFrameworkBookLoansRepository>());
        _ = services.AddSingleton<IBooksRepository>(x =>
            x.GetRequiredService<EntityFrameworkBookLoansRepository>());
        _ = services.AddSingleton<ILoansRepository>(x =>
            x.GetRequiredService<EntityFrameworkBookLoansRepository>());

        _ = services.AddSingleton(TimeProvider.System);

        return services;
    }
}
