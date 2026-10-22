namespace CleanMinimalApi.Infrastructure.Databases.MovieReviews.Mapping;

using AutoMapper;
using Application = Application.Loans.Entities;
using Infrastructure = Models;

internal class LoanMappingProfile : Profile
{
    public LoanMappingProfile()
    {
        _ = this.CreateMap<Application.Loan, Infrastructure.LoanModel>()
            .ForMember(d => d.MemberId, o => o.Ignore())
            .ForMember(d => d.BookId, o => o.Ignore())
            .ForMember(d => d.DateCreated, o => o.Ignore())
            .ForMember(d => d.DateModified, o => o.Ignore())
            .ReverseMap();
    }
}
