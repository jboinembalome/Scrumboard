using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Scrumboard.Application.Cards.Dtos;
using Scrumboard.Domain.Cards.Attachments;

namespace Scrumboard.Application.Cards.Attachments;

internal sealed class AttachmentProfile : Profile
{
    public AttachmentProfile()
    {
        // Read
        CreateMap<Attachment, AttachmentDto>()
            .EqualityComparison((src, dest) => src.Id == dest.Id);
    }
}