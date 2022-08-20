
using FileUtility.App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUtility.App.Common
{
    public static class CommonHelper
    {
        public static (DataTable updatedDataTable,bool isValid) ValidateHeaderAndUpdate(DataTable data, List<ColumnHeader> columnNames)
        {
            bool isValid = false;
            if (data == null || columnNames.Count==0)
            {
                isValid = false;
            }

            foreach (DataColumn column in data.Columns)
            {
                var columnHeader = columnNames.Where(m => m.FieldName.Equals(column.ColumnName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (columnHeader != null)
                {
                    data.Columns[column.ColumnName].ColumnName = columnHeader.FieldNameMap;
                    isValid = true;
                }
                else
                {
                    isValid = false;
                    break;
                }
            }

            return (data,isValid);
        }
    }
}
