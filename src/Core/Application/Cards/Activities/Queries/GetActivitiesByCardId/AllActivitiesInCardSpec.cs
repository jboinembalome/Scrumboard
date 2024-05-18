﻿using Ardalis.Specification;
using Scrumboard.Domain.Cards.Activities;

namespace Scrumboard.Application.Cards.Activities.Queries.GetActivitiesByCardId;

internal sealed class AllActivitiesInCardSpec : Specification<Activity>, ISingleResultSpecification
{
    public AllActivitiesInCardSpec(int cardId)
    {
        Query.Include(a => a.Card);
        Query.Include(a => a.Adherent);

        Query.Where(l => l.Card.Id == cardId);
    }
}