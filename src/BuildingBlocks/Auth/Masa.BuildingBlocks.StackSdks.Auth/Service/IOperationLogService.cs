namespace Masa.BuildingBlocks.StackSdks.Auth.Service;

public interface IOperationLogService
{
    Task AddLogAsync(AddOperationLogModel logModel);
}
