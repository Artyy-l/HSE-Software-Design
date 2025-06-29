using KontrolWork1.Managers;

namespace KontrolWork1.ImportExport;

public abstract class DataImporterBase
{
    public void Import(string filePath, 
        BankAccountManager accountManager, 
        CategoryManager categoryManager, 
        OperationManager operationManager)
    {
        var fileContent = ReadFile(filePath);
        var parsedData = ParseData(fileContent);
        ProcessData(parsedData, accountManager, categoryManager, operationManager);
    }
    
    protected virtual string ReadFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Файл не найден.", filePath);
        return File.ReadAllText(filePath);
    }
    
    protected abstract dynamic ParseData(string fileContent);
    protected abstract void ProcessData(dynamic parsedData, 
        BankAccountManager accountManager, 
        CategoryManager categoryManager, 
        OperationManager operationManager);
}