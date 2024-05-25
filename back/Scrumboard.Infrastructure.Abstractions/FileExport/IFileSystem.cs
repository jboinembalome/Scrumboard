namespace Scrumboard.Infrastructure.Abstractions.FileExport;

public interface IFileSystem
{
    Task<bool> SavePicture(string pictureName, string pictureBase64, CancellationToken cancellationToken);
}