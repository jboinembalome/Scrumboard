﻿using Ardalis.Specification;
using Scrumboard.Domain.Cards;

namespace Scrumboard.Application.Cards.Comments.Specifications;

internal sealed class CommentWithAdherentAndCardSpec : Specification<Comment>, ISingleResultSpecification<Comment>
{
    public CommentWithAdherentAndCardSpec(int commentId)
    {
        Query.Where(c => c.Id == commentId);

        Query.Include(c => c.Adherent);
        Query.Include(c => c.Card)
            .ThenInclude(c => c.Activities);
    }
}
