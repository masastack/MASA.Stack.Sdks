// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

[assembly: InternalsVisibleTo("Masa.Contrib.StackSdks.Tsc.Tests")]
namespace Masa.Contrib.StackSdks.Tsc;

internal class TscClient : ITscClient
{
    internal TscClient(ICaller caller)
    {
        LogService = new LogService(caller);
        MetricService = new MetricService(caller);
        TraceService = new TraceService(caller);
        ApmService = new ApmService(caller);
        TopologyService = new TopologyService(caller);
    }

    public ILogService LogService { get; }

    public IMetricService MetricService { get; }

    public ITraceService TraceService { get; }

    public IApmService ApmService { get; }

    public ITopologyService TopologyService { get; }
}
