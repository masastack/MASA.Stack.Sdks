// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.StackSdks.Config.Consts;

public static class IdentityClaimConsts
{
    public const string ACCOUNT = "account";
    public const string ROLES = "roles";
    public const string ENVIRONMENT = "environment";
    public const string CURRENT_TEAM = "current_team";
    public const string STAFF = "staff_id";
    public const string PHONE_NUMBER = "phoneNumber";
    public const string EMAIL = "email";
    public const string USER_ID = "sub";
    public const string USER_NAME = "userName";
    public const string IMPERSONATOR_USER_ID = $"{DEFAULT_PREFIX}/impersonatorUserId";
    public const string DOMAIN_NAME = $"{DEFAULT_PREFIX}/domainName";
    private const string DEFAULT_PREFIX = "https://masastack.com/security/identity/claims";
}
