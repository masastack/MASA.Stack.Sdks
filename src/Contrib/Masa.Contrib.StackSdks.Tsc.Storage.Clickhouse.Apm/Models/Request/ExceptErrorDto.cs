// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Tsc.Storage.Clickhouse.Apm.Models.Request;

public class ExceptErrorDto
{
    public string Id { get; set; } = default!;

    public string Environment { get; set; } = default!;

    public string Project { get; set; } = default!;

    public string Service { get; set; } = default!;

    public string Type { get; set; } = default!;

    public string Message { get; set; } = default!;

    public string Comment { get; set; } = default!;

    public bool IsDeleted { get; set; } = default!;

    public string Creator { get; set; } = default!;

    public string Modifier { get; set; } = default!;

    public DateTime CreationTime { get; set; }

    public DateTime ModificationTime { get; set; }
}
