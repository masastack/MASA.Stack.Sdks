// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Enum;

public enum OperatorType
{
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,
    Equal,
    EqualIgnoreCase,
    StartsWith,
    NotEndsWith,
    NotEqualIgnoreCase,
    InCollection,
    NotInCollection,
    IsNull,
    IsNotNull,
    MatchesRegex,
    NotMatchRegex,
    Contains,
    NotContains
}