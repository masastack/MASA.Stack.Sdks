namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class AddOperationLogModel
{
    /// <summary>
    /// 操作人ID
    /// </summary>
    public Guid? Operator { get; set; }

    /// <summary>
    /// 操作人名称
    /// </summary>
    public string? OperatorName { get; set; }

    /// <summary>
    /// 操作类型
    /// </summary>
    public OperationTypes OperationType { get; set; }

    /// <summary>
    /// 操作时间
    /// </summary>
    public DateTime OperationTime { get; set; }

    /// <summary>
    /// 操作描述
    /// </summary>
    public string OperationDescription { get; set; } = "";

    /// <summary>
    /// 客户端ID
    /// </summary>
    public string? ClientId { get; set; }
    public AddOperationLogModel(
        Guid? operatorId,
        string? operatorName,
        OperationTypes operationType,
        DateTime operationTime,
        string operationDescription,
        string? clientId)
    {
        Operator = operatorId;
        OperatorName = operatorName;
        OperationType = operationType;
        OperationTime = operationTime;
        OperationDescription = operationDescription;
        ClientId = clientId;
    }
}
