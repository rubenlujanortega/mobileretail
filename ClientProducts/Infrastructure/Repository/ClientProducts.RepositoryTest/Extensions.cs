using Moq.Language;
using Moq.Language.Flow;
using System;
using System.Data;
using System.Runtime.Serialization;

namespace ClientProducts.RepositoryTest
{
    internal static class Extensions
    {
        internal static DataTable AddColumns(this DataTable dataTable, params string[] columns)
        {
            foreach (string column in columns)
            {
                dataTable.Columns.Add(column);
            }
            return dataTable;
        }

        internal static DataSet AddTable(this DataSet dataSet, Func<DataTable> dataTable = null)
        {
            dataSet = dataSet ?? new DataSet();
            dataSet.Tables.Add(dataTable != null ? dataTable() : new DataTable());
            return dataSet;
        }

        internal static DataTable AddRow(this DataTable dataTable, params object[] values)
        {
            dataTable = dataTable ?? new DataTable();

            if (values != null)
            {
                dataTable.Rows.Add(values);
            }
            else
            {
                dataTable.Rows.Add(dataTable.NewRow());
            }

            return dataTable;
        }
    }
}
