using Domains.Models;
using MediatR;

namespace Application.Tasks.Queries.QUserInfo
{
    public class GetUserByUsernameQuery : IRequest<UserInfo>
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
