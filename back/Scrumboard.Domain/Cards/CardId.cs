using System.Diagnostics.CodeAnalysis;
using Scrumboard.SharedKernel.Types;

namespace Scrumboard.Domain.Cards;

public sealed class CardId(
    int value) : IntStrongId<CardId>(value), IStrongId<CardId, int>
{
    [return: NotNullIfNotNull(nameof(id))]
    public static implicit operator CardId?(int? id)
        => id is null ? null : new CardId(id.Value);
}
