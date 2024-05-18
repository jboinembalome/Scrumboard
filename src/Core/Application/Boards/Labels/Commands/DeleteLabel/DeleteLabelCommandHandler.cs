using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Scrumboard.Application.Common.Exceptions;
using Scrumboard.Domain.Boards;
using Scrumboard.Domain.Cards;
using Scrumboard.Infrastructure.Abstractions.Persistence;

namespace Scrumboard.Application.Boards.Labels.Commands.DeleteLabel;

internal sealed class DeleteLabelCommandHandler : IRequestHandler<DeleteLabelCommand>
{
    private readonly IAsyncRepository<Label, int> _labelRepository;

    public DeleteLabelCommandHandler(IAsyncRepository<Label, int> labelRepository)
    {
        _labelRepository = labelRepository;
    }

    public async Task<Unit> Handle(
        DeleteLabelCommand request, 
        CancellationToken cancellationToken)
    {
        var labelToDelete = await _labelRepository.GetByIdAsync(request.LabelId, cancellationToken);

        if (labelToDelete == null)
            throw new NotFoundException(nameof(Card), request.LabelId);

        await _labelRepository.DeleteAsync(labelToDelete, cancellationToken);

        return Unit.Value;
    }
}