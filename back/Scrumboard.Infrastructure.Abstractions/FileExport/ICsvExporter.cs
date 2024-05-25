namespace Scrumboard.Infrastructure.Abstractions.FileExport;

public interface ICsvExporter<T> where T : class
{
    Task<byte[]> ExportToCsvAsync(IEnumerable<T> records, CancellationToken cancellationToken = default);
}