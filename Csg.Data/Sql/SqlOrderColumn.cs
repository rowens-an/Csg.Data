﻿using System;

namespace Csg.Data.Sql;

public class SqlOrderColumn : ISqlStatementElement
{
    public string ColumnName { get; set; }
    public DbSortDirection SortDirection { get; set; }

    #region ISqlStatementElement Members

    void ISqlStatementElement.Render(SqlTextWriter writer, SqlBuildArguments args)
    {
        writer.WriteSortColumn(ColumnName, SortDirection);
    }

    #endregion

    public static SqlOrderColumn Parse(string s)
    {
        var parts = s.Split(' ');
        var result = new SqlOrderColumn();

        result.ColumnName = parts[0];

        if (parts.Length == 2)
            result.SortDirection = parts[1].StartsWith("ASC", StringComparison.OrdinalIgnoreCase)
                ? DbSortDirection.Ascending
                : DbSortDirection.Descending;

        return result;
    }
}