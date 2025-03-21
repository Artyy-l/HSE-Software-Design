using KontrolWork1.Domain;

namespace KontrolWork1.ImportExport;

public interface IDataExporterVisitor
{
    void Visit(BankAccount account);
    void Visit(Category category);
    void Visit(Operation operation);
}