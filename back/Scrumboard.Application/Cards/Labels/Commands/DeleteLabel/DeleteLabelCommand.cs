using MediatR;

namespace Scrumboard.Application.Cards.Labels.Commands.DeleteLabel;

public sealed class DeleteLabelCommand : IRequest
{
    public int LabelId { get; set; }
}
