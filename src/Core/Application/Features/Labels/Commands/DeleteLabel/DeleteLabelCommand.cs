using MediatR;

namespace Scrumboard.Application.Features.Labels.Commands.DeleteLabel
{
    public class DeleteLabelCommand : IRequest
    {
        public int LabelId { get; set; }
    }
}
