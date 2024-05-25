using MediatR;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Boards.Labels.Commands.DeleteLabel;

internal sealed class DeleteLabelCommandHandler(IAsyncRepository<Label, int> labelRepository)
    : IRequestHandler<DeleteLabelCommand>
{
    public async Task Handle(
        DeleteLabelCommand request, 
        CancellationToken cancellationToken)
    {
        var labelToDelete = await labelRepository.GetByIdAsync(request.LabelId, cancellationToken);

        if (labelToDelete == null)
            throw new NotFoundException(nameof(Card), request.LabelId);

        await labelRepository.DeleteAsync(labelToDelete, cancellationToken);
    }
}
