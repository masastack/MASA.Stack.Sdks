// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.StackSdks.Auth.Service;

public class ProjectService : IProjectService
{
    readonly ICaller _caller;
    readonly IUserContext _userContext;

    const string PARTY = "api/project/";

    public ProjectService(ICaller caller, IUserContext userContext)
    {
        _caller = caller;
        _userContext = userContext;
    }

    public async Task<List<ProjectModel>> GetGlobalNavigations(string clientId, Guid? userId = null)
    {
        userId ??= _userContext.GetUserId<Guid>();
        var requestUri = $"{PARTY}navigations?userId={userId}&clientId={clientId}";
        return await _caller.GetAsync<List<ProjectModel>>(requestUri) ?? new();
    }

    public async Task<List<ProjectModel>> GetNavigationsByAppId(params string[] appIds)
    {
        var validAppIds = Array.FindAll(appIds ?? Array.Empty<string>(), appId => !string.IsNullOrWhiteSpace(appId));
        if (validAppIds.Length == 0)
        {
            return new();
        }

        var requestUri = BuildNavigationsByAppIdsRequestUri(validAppIds);

        return await _caller.GetAsync<List<ProjectModel>>(requestUri) ?? new();
    }

    public async Task<NavDetailModel?> GetMenuDetailAsync(Guid menuId)
    {
        var requestUri = $"{PARTY}menus/detail";
        return await _caller.GetAsync<object, NavDetailModel>(requestUri, new { menuId });
    }

    public async Task UpdateMenuAsync(UpdateNavModel input)
    {
        var requestUri = $"{PARTY}menus/meta";
        await _caller.PutAsync(requestUri, input);
    }

    public async Task<List<ProjectModel>> GetUIAndMenusAsync()
    {
        var requestUri = $"{PARTY}GetUIAndMenus";
        return await _caller.GetAsync<List<ProjectModel>>(requestUri) ?? new();
    }

    static string BuildNavigationsByAppIdsRequestUri(IEnumerable<string> appIds)
    {
        var queryValue = Uri.EscapeDataString(string.Join(',', appIds));
        return $"{PARTY}byAppIds?appIds={queryValue}";
    }
}
