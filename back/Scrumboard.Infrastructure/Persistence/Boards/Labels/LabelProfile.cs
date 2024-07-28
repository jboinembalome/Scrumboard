using AutoMapper;
using Scrumboard.Domain.Boards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

namespace Scrumboard.Infrastructure.Persistence.Boards.Labels;

internal sealed class LabelProfile : Profile
{
    public LabelProfile()
    {
        // Write
        CreateMap<LabelCreation, LabelDao>();
        CreateMap<LabelEdition, LabelDao>();

        CreateMap<int, LabelDao>()
            .ConstructUsing(labelId => new LabelDao
            {
                Id = labelId
            });

        // Read
        CreateMap<LabelDao, Label>();

        CreateMap<LabelDao, int>()
            .ConstructUsing(labelDao => labelDao.Id);
    }
}
