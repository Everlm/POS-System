﻿namespace POS.Utilities.Static
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
        public static List<(string ColumnsName, string PropertyName)> GetColumnsProducts()
        {
            var columnsProperties = new List<(string ColumnsName, string PropertyName)>
            {
                ("NOMBRE","Name"),
                ("CATEGORIA","Category"),
                ("CODIGO","Code"),
                ("STOCK MINIMO","StockMin"),
                ("STOCK MAXIMO","StockMax"),
                ("PRECIO DE VENTA UNIDAD","UnitSalePrice"),
                ("FECHA DE CREACION","AuditCreateDate"),
                ("ESTADO","StateProduct")
            };

            return columnsProperties;
        }

        public static List<(string ColumnsName, string PropertyName)> GetColumnsPurchases()
        {
            var columnsProperties = new List<(string ColumnsName, string PropertyName)>
            {
                ("PROVEEDOR","Provider"),
                ("ALMACEN","Warehouse"),
                ("MONTO TOTAL","TotalAmout"),
                ("FECHA DE COMPRA","DateOfPurchase")
            };

            return columnsProperties;
        } 
        public static List<(string ColumnsName, string PropertyName)> GetColumnsClients()
        {
            var columnsProperties = new List<(string ColumnsName, string PropertyName)>
            {
                ("TIPO DE DOCUMENTO","DocumentType"),
                ("NUMERO DE DOCUMENTO","DocumentNumber"),
                ("NOMBRE","Name"),
                ("DIRECCION","Address"),
                ("TELEFONO","Phone"),
                ("CORREO","Email"),
                ("ESTADO","StateClient"),
                ("FECHA DE CREACION","AuditCreateDate")
            };

            return columnsProperties;
        } 
        
        public static List<(string ColumnsName, string PropertyName)> GetColumnsSale()
        {
            var columnsProperties = new List<(string ColumnsName, string PropertyName)>
            {
                ("Comprobante","VoucherDescription"),
                ("Numero comprobante","VoucherNumber"),
                ("Fecha","AuditCreateDate"),
                ("Bodega","Warehouse"),
                ("Observacion","Observation"),
                ("Monto total","TotalAmout")
            };

            return columnsProperties;
        }
    }
}
