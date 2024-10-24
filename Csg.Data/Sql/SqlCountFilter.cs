﻿using System.Collections.Generic;
using System.Data;

namespace Csg.Data.Sql;

/// <summary>
///     Creates a filter condition matching {ColumnName} { IN | NOT IN } (SELECT {SubQueryColumn} FROM {SubQueryTable}
///     WHERE {SubQueryFilters})
/// </summary>
public class SqlCountFilter : ISqlFilter
{
    private List<ISqlFilter> _subQueryFilters;

    public SqlCountFilter(ISqlTable table, ISqlTable subQueryTable, string subQueryColumn, SqlOperator @operator,
        int countValue)
    {
        Table = table;
        SubQueryColumn = subQueryColumn;
        SubQueryTable = subQueryTable;
        CountOperator = @operator;
        CountValue = countValue;
    }

    /// <summary>
    ///     Gets or sets the table to filter.
    /// </summary>
    public ISqlTable Table { get; set; }

    /// <summary>
    ///     Gets or sets the column in the sub-query to match the left column against.
    /// </summary>
    public string SubQueryColumn { get; set; }

    /// <summary>
    ///     Gets or sets the sub-query table expression
    /// </summary>
    public ISqlTable SubQueryTable { get; set; }

    /// <summary>
    ///     Gets a collection of filters to apply to the sub-query.
    /// </summary>
    public ICollection<ISqlFilter> SubQueryFilters
    {
        get
        {
            if (_subQueryFilters == null) _subQueryFilters = new List<ISqlFilter>();
            return _subQueryFilters;
        }
    }

    /// <summary>
    ///     Gets or sets the count value to use for comparison when SubQueryMode is Count
    /// </summary>
    public int CountValue { get; set; }

    /// <summary>
    ///     Gets or sets the operator to use when SubQueryMode is Count
    /// </summary>
    public SqlOperator CountOperator { get; set; } = SqlOperator.GreaterThanOrEqual;

    void ISqlStatementElement.Render(SqlTextWriter writer, SqlBuildArguments args)
    {
        args.AssignAlias(SubQueryTable);

        var subquery = new SqlSelectBuilder(SubQueryTable);
        var subQueryColumn = new SqlColumn(SubQueryTable, SubQueryColumn);
        subQueryColumn.Aggregate = SqlAggregate.Count;
        subQueryColumn.Alias = "Cnt";
        subquery.Columns.Add(subQueryColumn);

        foreach (var filter in SubQueryFilters) subquery.Filters.Add(filter);

        writer.WriteBeginGroup();

        writer.WriteBeginGroup();
        subquery.Render(writer, args);
        writer.WriteEndGroup();

        writer.WriteSpace();
        writer.WriteOperator(CountOperator);
        writer.WriteSpace();
        writer.WriteParameter(args.CreateParameter(CountValue, DbType.Int32));

        writer.WriteEndGroup();
    }
}