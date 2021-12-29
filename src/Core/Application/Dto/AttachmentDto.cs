using Scrumboard.Domain.Enums;

namespace Scrumboard.Application.Dto
{
    public class AttachmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public AttachmentType AttachmentType { get; set; }
    }
}
