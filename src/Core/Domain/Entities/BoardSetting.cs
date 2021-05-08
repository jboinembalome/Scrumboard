using Scrumboard.Domain.Enums;
using System;

namespace Scrumboard.Domain.Entities
{
    public class BoardSetting
    {
        public Guid BoardSettingId { get; set; }
        public CustomColor CustomColor { get; set; }
        public bool Subscribed { get; set; }
        public bool CardCoverImage { get; set; }
    }
}
