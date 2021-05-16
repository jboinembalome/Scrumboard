using AutoMapper;
using MediatR;
using Scrumboard.Application.Exceptions;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Scrumboard.Application.Features.Boards.Commands.UpdateBoard
{
    public class UpdateBoardCommandHandler : IRequestHandler<UpdateBoardCommand>
    {
        private readonly IAsyncRepository<Board, Guid> _boardRepository;
        private readonly IMapper _mapper;

        public UpdateBoardCommandHandler(IMapper mapper, IAsyncRepository<Board, Guid> boardRepository)
        {
            _mapper = mapper;
            _boardRepository = boardRepository;
        }

        public async Task<Unit> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
        {
            var boardToUpdate = await _boardRepository.GetByIdAsync(request.BoardId);

            if (boardToUpdate == null)
                throw new NotFoundException(nameof(Board), request.BoardId);

            var validator = new UpdateBoardCommandValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
                throw new ValidationException(validationResult.Errors);

            _mapper.Map(request, boardToUpdate, typeof(UpdateBoardCommand), typeof(Board));

            await _boardRepository.UpdateAsync(boardToUpdate);

            return Unit.Value;
        }
    }
}
