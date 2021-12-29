using Ardalis.Specification;
using Scrumboard.Domain.Entities;
using System.Linq;

namespace Scrumboard.Application.Features.Labels.Specifications
{
    public class AllLabelsInBoardSpec : Specification<Label>, ISingleResultSpecification
    {
        public AllLabelsInBoardSpec(int boardId)
        {
            Query.Include(l => l.Cards)
                .ThenInclude(c => c.ListBoard)
                .ThenInclude(l => l.Board);

            Query.Where(l => l.Cards.Any(c => c.ListBoard.Board.Id == boardId)); 
        }
    }
}
