using System.Reflection;

namespace Masa.BuildingBlocks.StackSdks.Base;

public class StackSdksBase
{
    public StackSdksBase(string name, string type = "http")
    {
        Name = name;
        Version = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
        UserAgent = $"masastack_sdk/{Version} ({name}; {type})";
    }

    public virtual string Name { get; set; }

    public virtual string UserAgent { get; set; }

    public virtual string Version { get; set; }
}
