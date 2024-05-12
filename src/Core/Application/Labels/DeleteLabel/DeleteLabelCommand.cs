using MediatR;

namespace Scrumboard.Application.Labels.DeleteLabel;

public class DeleteLabelCommand : IRequest
{
    public int LabelId { get; set; }
}