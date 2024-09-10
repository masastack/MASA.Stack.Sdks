namespace Masa.Contrib.StackSdks.Dcc.Service;

public class LabelService: ILabelService
{
    private readonly ICaller _caller;

    public LabelService(ICaller caller)
    {
        _caller = caller;
    }

    public async Task<List<LabelModel>> GetListByTypeCodeAsync(string typeCode)
    {
        var requestUri = $"api/v1/{typeCode}/labels";
        var result = await _caller.GetAsync<List<LabelModel>>(requestUri);

        return result ?? new();
    }
}
