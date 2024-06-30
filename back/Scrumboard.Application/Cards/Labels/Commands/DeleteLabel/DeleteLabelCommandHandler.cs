using MediatR;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence.Cards.Labels;

namespace Scrumboard.Application.Cards.Labels.Commands.DeleteLabel;

internal sealed class DeleteLabelCommandHandler(
    ILabelsRepository labelsRepository)
    : IRequestHandler<DeleteLabelCommand>
{
    public async Task Handle(
        DeleteLabelCommand request, 
        CancellationToken cancellationToken)
    {
        var labelToDelete = await labelsRepository.TryGetByIdAsync(request.LabelId, cancellationToken);

        if (labelToDelete is null)
            throw new NotFoundException(nameof(Card), request.LabelId);

        await labelsRepository.DeleteAsync(labelToDelete.Id, cancellationToken);
    }
}
