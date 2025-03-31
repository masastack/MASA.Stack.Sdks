// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;

public class AuthPaginationModel<TEntity> where TEntity : class
{
    public long Total { get; }

    public List<TEntity> Items { get; }

    public AuthPaginationModel()
    {
        Items = new List<TEntity>();
    }

    [JsonConstructor]
    public AuthPaginationModel(long total, List<TEntity> items)
    {
        Total = total;
        Items = items;
    }
}
