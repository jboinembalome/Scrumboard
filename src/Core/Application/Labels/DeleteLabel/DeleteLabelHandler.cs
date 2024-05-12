using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Scrumboard.Application.Exceptions;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Application.Labels.DeleteLabel;

public class DeleteLabelHandler : IRequestHandler<DeleteLabelCommand>
{
    private readonly IAsyncRepository<Label, int> _labelRepository;

    public DeleteLabelHandler(IAsyncRepository<Label, int> labelRepository)
    {
        _labelRepository = labelRepository;
    }

    public async Task<Unit> Handle(DeleteLabelCommand request, CancellationToken cancellationToken)
    {
        var labelToDelete = await _labelRepository.GetByIdAsync(request.LabelId, cancellationToken);

        if (labelToDelete == null)
            throw new NotFoundException(nameof(Card), request.LabelId);

        await _labelRepository.DeleteAsync(labelToDelete, cancellationToken);

        return Unit.Value;
    }
}