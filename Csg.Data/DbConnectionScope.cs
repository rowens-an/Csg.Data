﻿using System;
using System.Data;

namespace Csg.Data;

/// <summary>
///     This class attempts to open a given database connection when it is initialized, and, when disposed, closes the
///     connection if it was not already open.
/// </summary>
public class DbConnectionScope : IDisposable
{
    private readonly IDbConnection _connection;

    /// <summary>
    ///     Initializes a new instance with the given connection, attempting to open the connection if it is not already open.
    /// </summary>
    /// <param name="connection"></param>
    public DbConnectionScope(IDbConnection connection)
    {
        _connection = connection;
        if (_connection.State != ConnectionState.Open)
        {
            _connection.Open();
            OwnsConnection = true;
        }
    }

    /// <summary>
    ///     Gets a value that indicates if the constructor opened the connection.
    /// </summary>
    public bool OwnsConnection { get; }

    /// <summary>
    ///     Closes the underlying connection if <see cref="OwnsConnection" /> is true.
    /// </summary>
    public void Dispose()
    {
        if (OwnsConnection) _connection.Close();
    }
}