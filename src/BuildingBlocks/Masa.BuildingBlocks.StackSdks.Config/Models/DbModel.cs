// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Config.Models;

public class DbModel
{
    public string Server { get; set; } = default!;

    public int Port { get; set; } = 1433;

    public string Database { get; set; } = default!;

    public string UserId { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string? DbType { get; set; }

    public string ToString(string database)
    {
        return DbType switch
        {
            "PostgreSql" => $"Host={Server};Port={Port};Username={UserId};Password={Password};Database={database};",
            "MySql" => $"Server={Server};Port={Port};Database={database};Uid={UserId};Pwd={Password};",
            _ => $"Server={Server},{Port};Database={database};User Id={UserId};Password={Password};TrustServerCertificate=true;",
        };
    }
}