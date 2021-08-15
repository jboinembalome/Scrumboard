using AutoMapper;
using MediatR;
using Scrumboard.Application.Exceptions;
using Scrumboard.Application.Features.Boards.Specifications;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Scrumboard.Application.Features.Boards.Commands.UpdateBoard
{
    public class UpdateBoardCommandHandler : IRequestHandler<UpdateBoardCommand>
    {
        private readonly IAsyncRepository<Board, int> _boardRepository;
        private readonly IMapper _mapper;

        public UpdateBoardCommandHandler(IMapper mapper, IAsyncRepository<Board, int> boardRepository)
        {
            _mapper = mapper;
            _boardRepository = boardRepository;
        }

        public async Task<Unit> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
        {
            var specification = new UpdateBoardSpec(request.BoardId);
            var boardToUpdate = await _boardRepository.FirstOrDefaultAsync(specification, cancellationToken);

            if (boardToUpdate == null)
                throw new NotFoundException(nameof(Board), request.BoardId);

            _mapper.Map(request, boardToUpdate, typeof(UpdateBoardCommand), typeof(Board));

            await _boardRepository.UpdateAsync(boardToUpdate, cancellationToken);

            return Unit.Value;
        }
    }
}
