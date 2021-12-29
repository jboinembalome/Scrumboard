using System;

namespace Scrumboard.Application.Dto
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public AdherentDto Adherent { get; set; }
    }
}
