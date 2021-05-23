
namespace Scrumboard.Application.Dto
{
    public class BoardDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public BoardSettingDto BoardSetting { get; set; }
    }
}
