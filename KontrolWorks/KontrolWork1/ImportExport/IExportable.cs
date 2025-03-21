namespace KontrolWork1.ImportExport;

public interface IExportable
{
    string Accept(IDataExporterVisitor visitor);
}