using System.Collections.Generic;

namespace Scrumboard.Application.Dto
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<AdherentDto> Adherents { get; set; }
    }
}
