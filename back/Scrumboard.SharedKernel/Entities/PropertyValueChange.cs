namespace Scrumboard.SharedKernel.Entities;

public record PropertyValueChange<TProperty>(TProperty OldValue, TProperty NewValue)
{
    public static implicit operator PropertyValueChange<TProperty>(
        (TProperty OldValue, TProperty NewValue) change) 
        => new(change.OldValue, change.NewValue);
}
