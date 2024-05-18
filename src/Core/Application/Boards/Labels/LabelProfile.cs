using System.Linq;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Application.Boards.Dtos;
using Scrumboard.Domain.Boards;

namespace Scrumboard.Application.Boards.Labels;

internal sealed class LabelProfile : Profile
{
    public LabelProfile()
    {
        // Read
        CreateMap<Label, LabelDto>()
            .EqualityComparison((src, dest) => src.Id == dest.Id)
            .ForMember(dest => dest.CardIds, opt => opt.MapFrom(src => src.Cards.Select(c => c.Id)));
    }
}