namespace Masa.Contrib.StackSdks.Tsc.Storage.Clickhouse.Apm.Models.Response;

public class EnvironmentAppDto
{
    public string AppId { get; set; } = default!;

    public AppTypes AppType { get; set; }

    public string ProjectId { get; set; } = default!;

    public string AppDescription { get; set; } = default!;
}
