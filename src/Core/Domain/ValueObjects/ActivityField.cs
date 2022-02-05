using Scrumboard.Domain.Common;
using Scrumboard.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Scrumboard.Domain.ValueObjects
{
    public class ActivityField : ValueObject
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
                throw new UnsupportedActivityFieldException(code);

            return ActivityField;
        }

        public static ActivityField Card => new("Card");

        public static ActivityField Name => new("Name");

        public static ActivityField Description => new("Description");

        public static ActivityField DueDate => new("Due date");

        public static ActivityField Member => new("Member");

        public static ActivityField Checklist => new("Checklist");

        public static ActivityField ChecklistItem => new("Checklist item");

        public static ActivityField Label => new("Label");

        public static ActivityField Comment => new("Comment");

        public string Field { get; private set; }

        public static implicit operator string(ActivityField ActivityField) => ActivityField.ToString();

        public static explicit operator ActivityField(string field) => From(field);

        public override string ToString() => Field;

        protected static IEnumerable<ActivityField> SupportedActivityFields
        {
            get
            {
                yield return Card;
                yield return Name;
                yield return Description;
                yield return DueDate;
                yield return Member;
                yield return Checklist;
                yield return ChecklistItem;
                yield return Label;
                yield return Comment;
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Field;
        }
    }

}
