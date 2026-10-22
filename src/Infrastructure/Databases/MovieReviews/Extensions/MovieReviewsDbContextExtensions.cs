namespace CleanMinimalApi.Infrastructure.Databases.MovieReviews.Extensions;

using System;
using Bogus;
using Models;

internal static class MovieReviewsDbContextExtensions
{
    public static MovieReviewsDbContext AddData(this MovieReviewsDbContext context)
    {
        var authors = new Faker<MemberModel>()
            .RuleFor(a => a.Id, _ => Guid.NewGuid())
            .RuleFor(a => a.FirstName, f => f.Person.FirstName)
            .RuleFor(a => a.LastName, f => f.Person.LastName)
            .RuleFor(a => a.DateCreated, f => f.Date.Past())
            .RuleFor(a => a.DateModified, f => f.Date.Past())
            .Generate(15);

        context.AddRange(authors);

        var movies = new Faker<BookModel>()
            .RuleFor(m => m.Id, _ => Guid.NewGuid())
            .RuleFor(m => m.Title, f => f.Company.CatchPhrase())
            .RuleFor(m => m.Author, f => f.Person.FullName)
            .RuleFor(m => m.Copies, f => f.Random.Number(1, 3))
            .RuleFor(m => m.DateCreated, f => f.Date.Past())
            .RuleFor(m => m.DateModified, f => f.Date.Past())
            .Generate(50);

        context.AddRange(movies);

        var reviews = new Faker<LoanModel>()
            .RuleFor(r => r.Id, _ => Guid.NewGuid())
            .RuleFor(r => r.BookId, f => f.PickRandom(movies).Id)
            .RuleFor(r => r.MemberId, f => f.PickRandom(authors).Id)
            .RuleFor(r => r.StartDate, f => f.Date.Recent(40))
            .RuleFor(r => r.DateCreated, f => f.Date.Past())
            .RuleFor(r => r.DateModified, f => f.Date.Past())
            .Generate(30);

        context.AddRange(reviews);

        _ = context.SaveChanges();

        return context;
    }
}
