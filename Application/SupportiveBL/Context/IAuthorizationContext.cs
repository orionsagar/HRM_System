namespace Application.SupportiveBL.Context
{
    public interface IAuthorizationContext
    {
         string UserId { get; set; }
        public string UserName { get; set; }
        public string RoleId { get; set; }
    }
}
