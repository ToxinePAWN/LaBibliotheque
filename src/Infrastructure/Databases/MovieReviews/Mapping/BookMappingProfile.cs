namespace CleanMinimalApi.Infrastructure.Databases.MovieReviews.Mapping;

using AutoMapper;
using Application = Application.Movies.Entities;
using Infrastructure = Models;

internal class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        _ = this.CreateMap<Application.Book, Infrastructure.BookModel>()
            .ForMember(d => d.DateCreated, o => o.Ignore())
            .ForMember(d => d.DateModified, o => o.Ignore())
            .ReverseMap();

    }
}
