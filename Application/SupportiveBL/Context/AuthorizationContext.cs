namespace Application.SupportiveBL.Context
{
    public class AuthorizationContext : IAuthorizationContext
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RoleId { get; set; }
    }
}
