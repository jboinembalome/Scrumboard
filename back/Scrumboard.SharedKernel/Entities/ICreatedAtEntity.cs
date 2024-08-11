namespace Scrumboard.SharedKernel.Entities;

public interface ICreatedAtEntity
{
    UserId CreatedBy { get; set; }
    DateTimeOffset CreatedDate { get; set; }
}
