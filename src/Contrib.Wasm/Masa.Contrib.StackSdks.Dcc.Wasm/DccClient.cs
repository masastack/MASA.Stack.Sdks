namespace Masa.Contrib.StackSdks.Dcc;

public class DccClient : IDccClient
{
    public DccClient(ICaller caller, IMultiEnvironmentUserContext userContext, IMasaStackConfig masaStackConfig)
    {
        LabelService = new LabelService(caller);
        OpenApiService = new OpenApiService(caller, userContext, masaStackConfig);
    }

    public ILabelService LabelService { get; init; }

    public IOpenApiService OpenApiService { get; init; }
}