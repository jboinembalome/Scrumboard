using AutoMapper;
using Scrumboard.Domain.Common;

namespace Scrumboard.Web.Api.Common.Profiles;

internal sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        // TODO: Use an enum for Colour...
        // Write
        CreateMap<string, Colour>()
            .ConstructUsing(x => Colour.From(x));
        
        // Read
        CreateMap<Colour, string>()
            .ConstructUsing(x => x.Code);
    }
}
