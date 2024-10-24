﻿namespace Csg.Data.Sql;

/// <summary>
///     Used to specified the T-SQL comparison operator used on filters.
/// </summary>
public enum SqlOperator : short
{
    Equal,
    NotEqual,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual
}