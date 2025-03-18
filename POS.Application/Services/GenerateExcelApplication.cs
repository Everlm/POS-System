using POS.Application.Interfaces;
using POS.Infrastructure.FileExcel;
using POS.Utilities.Static;

namespace POS.Application.Services
{
    public class GenerateExcelApplication : IGenerateExcelApplication
    {
        private readonly IGenerateExcel _generateExcel;

        public GenerateExcelApplication(IGenerateExcel generateExcel)
        {
            _generateExcel = generateExcel;
        }

        public byte[] GenerateToExcel<T>(IEnumerable<T> data, List<(string ColumsName, string PropertyName)> columns)
        {
            var excelColumns = ExcelColumsNames.GetColumns(columns);
            var memoryStreamExcel = _generateExcel.GenerateToExcel(data, excelColumns);
            var fileBytes = memoryStreamExcel.ToArray();

            return fileBytes;
        }
    }
}
