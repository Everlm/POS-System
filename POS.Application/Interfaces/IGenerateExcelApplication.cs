namespace POS.Application.Interfaces
{
    public interface IGenerateExcelApplication
    {
        byte[] GenerateToExcel<T>(IEnumerable<T> data, List<(string ColumsName, string PropertyName)> columns);
    }
}
