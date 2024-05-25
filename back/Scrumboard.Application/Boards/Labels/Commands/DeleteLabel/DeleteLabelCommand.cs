using MediatR;

namespace Scrumboard.Application.Boards.Labels.Commands.DeleteLabel;

public sealed class DeleteLabelCommand : IRequest
{
    public int LabelId { get; set; }
}