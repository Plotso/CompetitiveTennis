namespace CompetitiveTennis;
public static class Constants
{
    public const string AdministratorRoleName = "Administrator";
    public const string SystemUser = "SysUser@competitivetennis.com";

    public const string AuthenticationCookieName = "Authentication";
    public const string AuthorizationHeaderName = "Authorization";
    public const string AuthorizationHeaderValuePrefix = "Bearer";

    public static class ErrorCodes
    {
        public const int UnexpectedErrorCode = -666;
        public const int InvalidInputErrorCode = -777;
        public const int ProvidedDataNotFound = -888;
    }
        
}