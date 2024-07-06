using AutoMapper;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Infrastructure.Persistence.Cards.Labels;

internal sealed class LabelProfile : Profile
{
    public LabelProfile()
    {
        // Write
        CreateMap<Label, LabelDao>();

        // Read
        CreateMap<LabelDao, Label>();
    }
}
