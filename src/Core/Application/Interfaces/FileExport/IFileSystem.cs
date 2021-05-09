using System.Threading;
using System.Threading.Tasks;

namespace Scrumboard.Application.Interfaces.FileExport
{
    public interface IFileSystem
    {
        Task<bool> SavePicture(string pictureName, string pictureBase64, CancellationToken cancellationToken);
    }
}
