// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class DynamicRuleConditionModel
{
    public LogicalOperator LogicalOperator { get; set; }

    public string FieldName { get; set; } = string.Empty;

    public OperatorTypes OperatorType { get; set; }

    public string Value { get; set; } = string.Empty;

    public DynamicRoleDataType DataType { get; set; }
}
