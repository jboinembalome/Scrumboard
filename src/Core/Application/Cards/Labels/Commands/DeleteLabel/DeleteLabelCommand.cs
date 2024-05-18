using MediatR;

namespace Scrumboard.Application.Cards.Labels.Commands.DeleteLabel;

public class DeleteLabelCommand : IRequest
{
    public int LabelId { get; set; }
}