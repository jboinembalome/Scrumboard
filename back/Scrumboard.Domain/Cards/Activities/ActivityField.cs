using Scrumboard.Domain.Cards.Activities.Errors;
using Scrumboard.SharedKernel.ValueObjects;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value

namespace Scrumboard.Domain.Cards.Activities;

public sealed class ActivityField : ValueObject
{
    static ActivityField()
    {
    }

    private ActivityField()
    {
    }

    private ActivityField(string field) => Field = field;

    public static ActivityField From(string code)
    {
        var ActivityField = new ActivityField { Field = code };

        if (!SupportedActivityFields.Contains(ActivityField))
        {
            throw new UnsupportedActivityFieldException(code);
        }

        return ActivityField;
    }
    
    public static ActivityField DueDate => new("Due date");
    
    public static ActivityField ListBoard => new("ListBoard");

    public static ActivityField Assignees => new("Assignees");

    public static ActivityField Label => new("Label");

    public string Field { get; private set; }

    public static implicit operator string(ActivityField activityField) => activityField.ToString();

    public static explicit operator ActivityField(string field) => From(field);

    public override string ToString() => Field;

    public static IEnumerable<ActivityField> SupportedActivityFields
    {
        get
        {
            yield return DueDate;
            yield return ListBoard;
            yield return Assignees;
            yield return Label;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Field;
    }
}
