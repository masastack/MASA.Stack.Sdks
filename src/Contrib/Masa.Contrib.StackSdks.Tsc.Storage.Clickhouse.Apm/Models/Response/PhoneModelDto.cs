// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Tsc.Storage.Clickhouse.Apm.Models.Response;

public class PhoneModelDto
{
    public string Model { get; set; } = default!;

    public string Type { get; set; } = default!;

    public string Brand { get; set; } = default!;
    public string BrandName { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string CodeAlis { get; set; } = default!;
    public string ModeName { get; set; } = default!;
    public string VerName { get; set; } = default!;
}