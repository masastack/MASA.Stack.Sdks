// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Dcc;

public interface IDccClient
{
    public ILabelService LabelService { get; }

    public IOpenApiService OpenApiService { get; }
}
