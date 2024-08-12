using Scrumboard.SharedKernel.Types;

namespace Scrumboard.SharedKernel.Entities;

public interface IModifiedAtEntity
{
    UserId? LastModifiedBy { get; set; }
    DateTimeOffset? LastModifiedDate { get; set; }
}
