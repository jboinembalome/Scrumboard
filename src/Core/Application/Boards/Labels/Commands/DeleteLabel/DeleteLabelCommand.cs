using MediatR;

namespace Scrumboard.Application.Boards.Labels.Commands.DeleteLabel;

public class DeleteLabelCommand : IRequest
{
    public int LabelId { get; set; }
}