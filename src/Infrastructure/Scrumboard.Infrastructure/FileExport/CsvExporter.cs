using CsvHelper;
using System.Globalization;
using Scrumboard.Infrastructure.Abstractions.FileExport;

namespace Scrumboard.Infrastructure.FileExport;

internal sealed class CsvExporter<T> : ICsvExporter<T> where T : class
{
    public async Task<byte[]> ExportToCsvAsync(IEnumerable<T> records, CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();

        await using (var streamWriter = new StreamWriter(memoryStream))
        {
            await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
            await csvWriter.WriteRecordsAsync<T>(records, cancellationToken);
        }

        return memoryStream.ToArray();
    }
}