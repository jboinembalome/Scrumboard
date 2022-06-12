using Microsoft.AspNetCore.SignalR;
using Scrumboard.Application.Features.Boards.Commands.UpdateBoard;
using System.Threading.Tasks;

namespace Scrumboard.Infrastructure.Notification.Hubs
{
    public class BoardHub : Hub
    {
        public async Task UpdateBoard(UpdateBoardCommandResponse updateBoard)
        {
            await Clients.All.SendAsync("UpdatedBoard", updateBoard);
        }
    }
}
