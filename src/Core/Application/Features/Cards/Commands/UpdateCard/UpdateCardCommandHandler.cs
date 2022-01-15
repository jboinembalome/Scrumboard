using AutoMapper;
using MediatR;
using Scrumboard.Application.Dto;
using Scrumboard.Application.Exceptions;
using Scrumboard.Application.Features.Boards.Specifications;
using Scrumboard.Application.Features.Cards.Specifications;
using Scrumboard.Application.Features.Labels.Specifications;
using Scrumboard.Application.Interfaces.Persistence;
using Scrumboard.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Scrumboard.Application.Features.Cards.Commands.UpdateCard
{
    public class UpdateCardCommandHandler : IRequestHandler<UpdateCardCommand, UpdateCardCommandResponse>
    {
        private readonly IAsyncRepository<Card, int> _cardRepository;
        private readonly IAsyncRepository<Label, int> _labelRepository;
        private readonly IAsyncRepository<Board, int> _boardRepository;
        private readonly IMapper _mapper;

        public UpdateCardCommandHandler(
            IMapper mapper,
            IAsyncRepository<Card, int> cardRepository,
            IAsyncRepository<Label, int> labelRepository,
            IAsyncRepository<Board, int> boardRepository)
        {
            _mapper = mapper;
            _cardRepository = cardRepository;
            _labelRepository = labelRepository;
            _boardRepository = boardRepository;
        }

        public async Task<UpdateCardCommandResponse> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
        {
            var updateCardCommandResponse = new UpdateCardCommandResponse();

            var specification = new CardWithAllExceptComment(request.Id);
            var cardToUpdate = await _cardRepository.FirstOrDefaultAsync(specification, cancellationToken);

            if (cardToUpdate == null)
                throw new NotFoundException(nameof(Card), request.Id);

            _mapper.Map(request, cardToUpdate, typeof(UpdateCardCommand), typeof(Card));

            // TODO: Add label
            // Retrieve the new label to add it to the board 
            //var newLabel = cardToUpdate.Labels.FirstOrDefault(l => l.Id == 0);

            //await GetLabelsInCardIfExist(cardToUpdate, cancellationToken);

            await _cardRepository.UpdateAsync(cardToUpdate, cancellationToken);

            //await AddLabelInBoard(cardToUpdate.ListBoard.Board.Id, newLabel, cancellationToken);

            updateCardCommandResponse.Card = _mapper.Map<CardDetailDto>(cardToUpdate);

            return updateCardCommandResponse;
        }

        /// <summary>
        /// Get the label that are not new in the database. 
        /// </summary>
        /// <remarks>
        /// Otherwise EF Core will try to create a new label with an existing id.
        /// </remarks>
        /// <param name="card">Card that contains labels.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        private async Task GetLabelsInCardIfExist(Card card, CancellationToken cancellationToken)
        {
            var label = card.Labels.FirstOrDefault(l => l.Id > 0);
            if (label != null)
            {
                //var specification = new LabelWithBoardAndCardsSpec(label.Id);
                //var existinglabel = await _labelRepository.FirstOrDefaultAsync(specification, cancellationToken);
                //if (existinglabel != null)
                //{
                //    var labels = card.Labels.Where(l => l.Id != label.Id).ToList();
                //    labels.Add(existinglabel);
                //    card.Labels = labels;
                //}
            }
        }

        /// <summary>
        /// Add a label in the board. 
        /// </summary>
        /// <param name="boardId">Board id.</param>
        /// <param name="labels">List of labels.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        private async Task AddLabelInBoard(int boardId, Label label, CancellationToken cancellationToken)
        {
            if (label != null && boardId > 0)
            {
                var specification = new BoardWithAllSpec(boardId);
                var board = await _boardRepository.FirstOrDefaultAsync(specification, cancellationToken);
                if (board != null)
                {
                    //board.Labels.Add(label);

                    //await _boardRepository.UpdateAsync(board, cancellationToken);
                }
            }
        }
    }
}
