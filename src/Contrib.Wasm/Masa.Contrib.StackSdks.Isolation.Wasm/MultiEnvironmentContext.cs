namespace Masa.Contrib.StackSdks.Isolation;

public class MultiEnvironmentContext : IMultiEnvironmentContext, IMultiEnvironmentSetter
{
    public string CurrentEnvironment { get; private set; } = string.Empty;

    public void SetEnvironment(string environment) => CurrentEnvironment = environment;
}
