using KontrolWork1.Domain;

namespace KontrolWork1.ImportExport;

public static class ExportableExtensions
{
    public static void Accept(this BankAccount account, IDataExporterVisitor visitor)
    {
        visitor.Visit(account);
    }

    public static void Accept(this Category category, IDataExporterVisitor visitor)
    {
        visitor.Visit(category);
    }

    public static void Accept(this Operation operation, IDataExporterVisitor visitor)
    {
        visitor.Visit(operation);
    }
}