using System;

namespace Scrumboard.Application.Dto
{
    public class BoardSettingDto
    {
        public Guid Id { get; set; }
        public CustomColorDto CustomColor { get; set; }
        public bool Subscribed { get; set; }
        public bool CardCoverImage { get; set; }
    }
}
