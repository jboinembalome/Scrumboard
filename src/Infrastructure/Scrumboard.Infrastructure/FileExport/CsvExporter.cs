using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Scrumboard.Infrastructure.Abstractions.FileExport;

namespace Scrumboard.Infrastructure.FileExport;

public class CsvExporter<T> : ICsvExporter<T> where T : class
{
    public async Task<byte[]> ExportToCsvAsync(IEnumerable<T> records, CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();
        using (var streamWriter = new StreamWriter(memoryStream))
        {
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
            await csvWriter.WriteRecordsAsync<T>(records, cancellationToken);
        }

        return memoryStream.ToArray();
    }
}