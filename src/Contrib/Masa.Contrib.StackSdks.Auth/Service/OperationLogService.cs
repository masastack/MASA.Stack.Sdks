namespace Masa.Contrib.StackSdks.Auth.Service;

public class OperationLogService : IOperationLogService
{
    readonly ICaller _caller;

    const string PARTY = "api/operationLog/";

    public OperationLogService(ICaller caller)
    {
        _caller = caller;
    }

    public async Task AddLogAsync(AddOperationLogModel logModel)
    {
        var requestUri = $"{PARTY}add";
        await _caller.PostAsync(requestUri, logModel);
    }
}
