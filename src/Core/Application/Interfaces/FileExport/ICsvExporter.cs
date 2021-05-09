using System.Collections.Generic;

namespace Scrumboard.Application.Interfaces.FileExport
{
    public interface ICsvExporter<T> where T : class
    {
        byte[] ExportToCsv(List<T> records);
    }
}
