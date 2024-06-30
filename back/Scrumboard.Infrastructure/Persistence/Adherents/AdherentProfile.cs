using AutoMapper;
using Scrumboard.Domain.Adherents;

namespace Scrumboard.Infrastructure.Persistence.Adherents;

internal sealed class AdherentProfile : Profile
{
    public AdherentProfile()
    {
        // Write
        CreateMap<Adherent, AdherentDao>();
        
        // Read
        CreateMap<AdherentDao, Adherent>();
    }
}
