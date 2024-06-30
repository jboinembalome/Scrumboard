using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Infrastructure.Persistence.Cards.Labels;

internal sealed class LabelProfile : Profile
{
    public LabelProfile()
    {
        // Write
        CreateMap<Label, LabelDao>()
            .EqualityComparison((dest, src) => dest.Id == src.Id);

        // Read
        CreateMap<LabelDao, Label>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);
    }
}
