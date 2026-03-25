// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Sapp.Model;

public class AppEntryDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;

    public string WebFullIcon { get; set; } = string.Empty;

    public string EntryUrl { get; set; } = string.Empty;
}
