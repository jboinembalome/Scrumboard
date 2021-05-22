using Scrumboard.Domain.Interfaces;

namespace Scrumboard.Domain.Entities
{
    public class Adherent: IEntity<int>
    {
        public int Id { get; set; }
        public string IdentityGuid { get; set; }
    }
}
