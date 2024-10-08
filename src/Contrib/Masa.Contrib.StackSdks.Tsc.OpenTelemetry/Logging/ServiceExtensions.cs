// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Microsoft.Extensions.Logging;

public static class ServiceExtensions
{
    public static ILoggingBuilder AddMasaOpenTelemetry(this ILoggingBuilder builder, Action<OpenTelemetryLoggerOptions> configure)
    {
        return builder.AddOpenTelemetry(options =>
          {
              options.IncludeScopes = true;
              options.IncludeFormattedMessage = true;
              options.ParseStateValues = true;
              configure?.Invoke(options);
          });
    }
}
