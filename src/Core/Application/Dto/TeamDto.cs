using System;
using System.Collections.Generic;

namespace Scrumboard.Application.Dto
{
    public class TeamDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> UserIds { get; set; }
    }
}
