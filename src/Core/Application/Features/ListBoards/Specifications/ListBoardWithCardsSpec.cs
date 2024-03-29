﻿using Ardalis.Specification;
using Scrumboard.Domain.Entities;

namespace Scrumboard.Application.Features.ListBoards.Specifications
{
    public class ListBoardWithCardsSpec : Specification<ListBoard>, ISingleResultSpecification
    {
        public ListBoardWithCardsSpec(int listBoardId)
        {
            Query.Where(l => l.Id == listBoardId);

            Query.Include(l => l.Cards);
        }
    }
}
