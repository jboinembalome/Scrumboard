
using System.Collections.Generic;
using Scrumboard.Application.Cards.Dtos;

namespace Scrumboard.Application.ListBoards.Dtos;

public class ListBoardDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Position { get; set; }
    public IEnumerable<CardDto> Cards { get; set; }
}