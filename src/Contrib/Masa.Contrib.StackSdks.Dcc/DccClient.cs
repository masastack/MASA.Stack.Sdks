// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

[assembly: InternalsVisibleTo("Masa.Contrib.StackSdks.Dcc.Tests")]

namespace Masa.Contrib.StackSdks.Dcc;

internal class DccClient : IDccClient
{
    public DccClient(IDistributedCacheClient distributedCacheClient, ICallerFactory callerFactory)
    {
        LabelService = new LabelService(distributedCacheClient);
        OpenApiService = new OpenApiService(callerFactory);
    }

    public ILabelService LabelService { get; }

    public IOpenApiService OpenApiService { get; }
}
