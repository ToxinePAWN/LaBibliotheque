namespace CleanMinimalApi.Infrastructure.Databases.MovieReviews.Mapping;

using AutoMapper;
using Application = Application.Authors.Entities;
using Infrastructure = Models;

internal class MemberMappingProfile : Profile
{
    public MemberMappingProfile()
    {
        _ = this.CreateMap<Application.Member, Infrastructure.MemberModel>()
            .ForMember(d => d.DateCreated, o => o.Ignore())
            .ForMember(d => d.DateModified, o => o.Ignore())
            .ReverseMap();
    }
}
