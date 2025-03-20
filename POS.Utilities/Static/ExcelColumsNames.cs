namespace POS.Utilities.Static
{
    public class ExcelColumsNames
    {
        public static List<TableColum> GetColumns(IEnumerable<(string ColumnsName, string PropertyName)> columnsProperties)
        {
            var columns = new List<TableColum>();

            foreach (var (ColumName, PropertyName) in columnsProperties)
            {
                var column = new TableColum()
                {
                    Label = ColumName,
                    PropertyName = PropertyName
                };

                columns.Add(column);
            }

            return columns;
        }

        public static List<(string ColumnsName, string PropertyName)> GetColumnsCategories()
        {
            var columnsProperties = new List<(string ColumnsName, string PropertyName)>
            {
                ("NOMBRE","Name"),
                ("DESCRIPTION","Description"),
                ("FECHA CREACION","AuditCreateDate"),
                ("ESTADO","StateCategory")
            };

            return columnsProperties;
        }

        public static List<(string ColumnsName, string PropertyName)> GetColumnsProviders()
        {
            var columnsProperties = new List<(string ColumnsName, string PropertyName)>
            {
                ("NOMBRE","Name"),
                ("CORREO","Email"),
                ("TIPO DE DOCUMENTO","DocumentType"),
                ("NUMERO DE DOCUMENTO","DocumentNumber"),
                ("DIRECCION","Address"),
                ("TELEFONO","Phone"),
                ("FECHA DE CREACION","AuditCreateDate"),
                ("ESTADO","StateProvider")
            };

            return columnsProperties;
        } 
        public static List<(string ColumnsName, string PropertyName)> GetColumnsWarehouses()
        {
            var columnsProperties = new List<(string ColumnsName, string PropertyName)>
            {
                ("NOMBRE","Name"),
                ("FECHA DE CREACION","AuditCreateDate"),
                ("ESTADO","StateWarehouse")
            };

            return columnsProperties;
        }
    }
}
