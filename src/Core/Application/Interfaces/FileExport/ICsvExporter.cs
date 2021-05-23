using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Scrumboard.Application.Interfaces.FileExport
{
    public interface ICsvExporter<T> where T : class
    {
        Task<byte[]> ExportToCsvAsync(IEnumerable<T> records, CancellationToken cancellationToken = default);
    }
}
